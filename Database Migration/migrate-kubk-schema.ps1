<#
.SYNOPSIS
    Phase 1 KUBK schema migration: adds new columns/tables to support tracking
    students, ranks, promotions, and annual dues across all KUBK dojos.

.DESCRIPTION
    Idempotent - safe to re-run. Checks schema before making any change.
    Uses Microsoft.ACE.OLEDB.16.0 (required for 64-bit PowerShell).

.PARAMETER DatabasePath
    Path to the .mdb file to migrate. Defaults to the DatabasePath value in
    DojoStudentManagement\App.config (relative to this script's solution folder).

.PARAMETER DatabasePassword
    Jet OLEDB database password. If not supplied, read from App.config.

.EXAMPLE
    .\migrate-kubk-schema.ps1 -DatabasePath "C:\scratch\DojoBosu-scratch.mdb"
#>
[CmdletBinding()]
param(
    [string]$DatabasePath,
    [string]$DatabasePassword
)

$ErrorActionPreference = 'Stop'

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$solutionDir = Split-Path -Parent $scriptDir
$appConfigPath = Join-Path $solutionDir 'App.config'

if (-not $DatabasePath -or -not $DatabasePassword) {
    if (-not (Test-Path $appConfigPath)) {
        throw "App.config not found at '$appConfigPath' and DatabasePath/DatabasePassword were not both supplied."
    }
    [xml]$configXml = Get-Content $appConfigPath
    $appSettings = $configXml.configuration.appSettings.add

    if (-not $DatabasePath) {
        $DatabasePath = ($appSettings | Where-Object { $_.key -eq 'DatabasePath' }).value
    }
    if (-not $DatabasePassword) {
        $DatabasePassword = ($appSettings | Where-Object { $_.key -eq 'DatabasePassword' }).value
    }
}

if (-not $DatabasePath) { throw "DatabasePath could not be determined." }
if (-not (Test-Path $DatabasePath)) { throw "Database file not found: $DatabasePath" }

Write-Host "Target database: $DatabasePath"

$connectionString = "Provider=Microsoft.ACE.OLEDB.16.0;Data Source=$DatabasePath;Jet OLEDB:Database Password=$DatabasePassword;"

Add-Type -AssemblyName System.Data

$connection = New-Object System.Data.OleDb.OleDbConnection($connectionString)
$connection.Open()

try {
    function Get-ExistingTables {
        $schema = $connection.GetSchema('Tables')
        $names = @()
        foreach ($row in $schema.Rows) {
            $names += [string]$row['TABLE_NAME']
        }
        return $names
    }

    function Get-ExistingColumns {
        param([string]$TableName)
        # Note: OleDbConnection.GetSchema('Columns', restrictions) throws
        # "The parameter is incorrect" against Microsoft.ACE.OLEDB.16.0, so
        # fetch the full column schema and filter in PowerShell instead.
        $schema = $connection.GetSchema('Columns')
        $names = @()
        foreach ($row in $schema.Rows) {
            if ([string]$row['TABLE_NAME'] -eq $TableName) {
                $names += [string]$row['COLUMN_NAME']
            }
        }
        return $names
    }

    function Invoke-Ddl {
        param([string]$Sql, [string]$Description)
        Write-Host "  Executing: $Description"
        $cmd = $connection.CreateCommand()
        $cmd.CommandText = $Sql
        $cmd.ExecuteNonQuery() | Out-Null
    }

    function Add-ColumnIfMissing {
        param(
            [string]$TableName,
            [string]$ColumnName,
            [string]$ColumnType
        )
        $existingColumns = Get-ExistingColumns -TableName $TableName
        if ($existingColumns -contains $ColumnName) {
            Write-Host "  SKIP: $TableName.$ColumnName already exists"
            return $false
        }
        $sql = "ALTER TABLE $TableName ADD COLUMN $ColumnName $ColumnType"
        Invoke-Ddl -Sql $sql -Description "ALTER TABLE $TableName ADD COLUMN $ColumnName $ColumnType"
        return $true
    }

    Write-Host "`n=== Step 1: Club_Parameters columns ==="
    Add-ColumnIfMissing -TableName 'Club_Parameters' -ColumnName 'club_instructor' -ColumnType 'TEXT(100)' | Out-Null
    Add-ColumnIfMissing -TableName 'Club_Parameters' -ColumnName 'club_instructor_email' -ColumnType 'TEXT(100)' | Out-Null
    $clubActiveJustCreated = Add-ColumnIfMissing -TableName 'Club_Parameters' -ColumnName 'club_active' -ColumnType 'YESNO'
    Add-ColumnIfMissing -TableName 'Club_Parameters' -ColumnName 'club_notes' -ColumnType 'MEMO' | Out-Null

    Write-Host "`n=== Step 2: StudArts columns ==="
    Add-ColumnIfMissing -TableName 'StudArts' -ColumnName 'studArt_rank_verified' -ColumnType 'DATETIME' | Out-Null

    Write-Host "`n=== Step 3: Promo_History columns ==="
    Add-ColumnIfMissing -TableName 'Promo_History' -ColumnName 'promo_recommended_by' -ColumnType 'TEXT(100)' | Out-Null

    Write-Host "`n=== Step 4: KUBK_Dues table ==="
    $existingTables = Get-ExistingTables
    if ($existingTables -contains 'KUBK_Dues') {
        Write-Host "  SKIP: KUBK_Dues table already exists"
    } else {
        $sql = @"
CREATE TABLE KUBK_Dues (
    dues_id COUNTER PRIMARY KEY,
    dues_student LONG NOT NULL,
    dues_year LONG NOT NULL,
    dues_paid_date DATETIME,
    dues_amount CURRENCY
)
"@
        Invoke-Ddl -Sql $sql -Description "CREATE TABLE KUBK_Dues"
    }

    Write-Host "`n=== Step 5: idx_dues_student_year unique index ==="
    $indexSchema = $connection.GetSchema('Indexes')
    $indexNames = @()
    foreach ($row in $indexSchema.Rows) {
        if ([string]$row['TABLE_NAME'] -eq 'KUBK_Dues') {
            $indexNames += [string]$row['INDEX_NAME']
        }
    }
    if ($indexNames -contains 'idx_dues_student_year') {
        Write-Host "  SKIP: idx_dues_student_year already exists"
    } else {
        Invoke-Ddl -Sql "CREATE UNIQUE INDEX idx_dues_student_year ON KUBK_Dues (dues_student, dues_year)" `
                   -Description "CREATE UNIQUE INDEX idx_dues_student_year"
    }

    Write-Host "`n=== Step 6: Default club_active to true for existing rows ==="
    # Jet/ACE has no concept of NULL for a YESNO column added via ALTER TABLE -
    # new rows come back as False, not NULL. So this seed step only makes sense
    # (and is only safe to re-run without clobbering later user edits) the one
    # time the column is actually created.
    if ($clubActiveJustCreated) {
        $updateCmd = $connection.CreateCommand()
        $updateCmd.CommandText = "UPDATE Club_Parameters SET club_active = true"
        $rowsAffected = $updateCmd.ExecuteNonQuery()
        Write-Host "  Set club_active = true for $rowsAffected existing row(s)"
    } else {
        Write-Host "  SKIP: club_active column already existed (not re-seeding, to avoid overwriting later edits)"
    }

    Write-Host "`n=== Step 7: Stamp Windsong StudArts rows as rank-verified ==="
    $cmd = $connection.CreateCommand()
    $cmd.CommandText = @"
SELECT COUNT(*) FROM StudArts
WHERE studArt_rank_verified IS NULL
AND StudArt_ID IN (SELECT stud_id FROM Students WHERE stud_club = 'Windsong')
"@
    $unverifiedCount = [int]$cmd.ExecuteScalar()
    if ($unverifiedCount -gt 0) {
        Write-Host "  Stamping $unverifiedCount Windsong StudArts row(s) with studArt_rank_verified = Now()"
        $updateCmd = $connection.CreateCommand()
        $updateCmd.CommandText = @"
UPDATE StudArts SET studArt_rank_verified = Now()
WHERE StudArt_ID IN (SELECT stud_id FROM Students WHERE stud_club = 'Windsong')
AND studArt_rank_verified IS NULL
"@
        $updateCmd.ExecuteNonQuery() | Out-Null
    } else {
        Write-Host "  SKIP: no unverified Windsong StudArts rows remain"
    }

    Write-Host "`nMigration complete."
}
finally {
    $connection.Close()
}
