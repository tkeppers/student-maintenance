<#
.SYNOPSIS
    Phase 1 club name cleanup: normalizes Students.stud_club values against the
    canonical Club_Parameters.club_id spelling, and reports/creates missing clubs.

.DESCRIPTION
    Dry-run by default - prints a full report of proposed changes without
    modifying anything. Pass -Apply to actually perform the updates/inserts.

    Comparison (case-insensitive, whitespace-normalized: trim + collapse
    internal runs of spaces) is done in PowerShell, not SQL, because Jet's
    string comparison is already case-insensitive and would hide the very
    mismatches this script looks for.

.PARAMETER DatabasePath
    Path to the .mdb file. Defaults to DatabasePath in App.config.

.PARAMETER DatabasePassword
    Jet OLEDB database password. Defaults to DatabasePassword in App.config.

.PARAMETER Apply
    Actually perform the updates/inserts. Without this switch, only a report
    is printed.

.EXAMPLE
    .\cleanup-club-names.ps1 -DatabasePath "C:\scratch\DojoBosu-scratch.mdb"
    .\cleanup-club-names.ps1 -DatabasePath "C:\scratch\DojoBosu-scratch.mdb" -Apply
#>
[CmdletBinding()]
param(
    [string]$DatabasePath,
    [string]$DatabasePassword,
    [switch]$Apply
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

function Normalize-ClubName {
    param([string]$Value)
    if ($null -eq $Value) { return '' }
    $trimmed = $Value.Trim()
    $collapsed = [System.Text.RegularExpressions.Regex]::Replace($trimmed, '\s+', ' ')
    return $collapsed
}

Write-Host "Target database: $DatabasePath"
Write-Host $(if ($Apply) { "Mode: APPLY (changes will be written)" } else { "Mode: DRY RUN (no changes will be written)" })

$connectionString = "Provider=Microsoft.ACE.OLEDB.16.0;Data Source=$DatabasePath;Jet OLEDB:Database Password=$DatabasePassword;"

Add-Type -AssemblyName System.Data

$connection = New-Object System.Data.OleDb.OleDbConnection($connectionString)
$connection.Open()

try {
    # --- Load stud_club values from Students ---
    # Deliberately NOT "SELECT DISTINCT" here: Jet/ACE string comparison (and
    # therefore DISTINCT/GROUP BY) is case-insensitive, so a SQL-side DISTINCT
    # would silently fold 'pearson' and 'Pearson' into a single row and hide
    # exactly the mismatches this script exists to find. Pull every raw value
    # and de-duplicate ourselves using ordinal (case-sensitive) comparison.
    $rawStudClubValues = New-Object System.Collections.Generic.List[string]
    $cmd = $connection.CreateCommand()
    $cmd.CommandText = "SELECT stud_club FROM Students WHERE stud_club IS NOT NULL"
    $reader = $cmd.ExecuteReader()
    while ($reader.Read()) {
        $rawStudClubValues.Add([string]$reader['stud_club'])
    }
    $reader.Close()
    $studClubValues = $rawStudClubValues | Sort-Object -Unique -CaseSensitive

    # --- Load club_id values from Club_Parameters ---
    $clubIds = New-Object System.Collections.Generic.List[string]
    $cmd = $connection.CreateCommand()
    $cmd.CommandText = "SELECT club_id FROM Club_Parameters"
    $reader = $cmd.ExecuteReader()
    while ($reader.Read()) {
        $clubIds.Add([string]$reader['club_id'])
    }
    $reader.Close()

    # Map: normalized key -> canonical club_id (first match wins)
    $canonicalByNormalized = @{}
    foreach ($clubId in $clubIds) {
        $norm = (Normalize-ClubName $clubId).ToLowerInvariant()
        if (-not $canonicalByNormalized.ContainsKey($norm)) {
            $canonicalByNormalized[$norm] = $clubId
        }
    }

    $renameActions = New-Object System.Collections.Generic.List[object]
    $newClubActions = New-Object System.Collections.Generic.List[object]

    foreach ($studClub in $studClubValues) {
        $norm = (Normalize-ClubName $studClub).ToLowerInvariant()
        if ($canonicalByNormalized.ContainsKey($norm)) {
            $canonical = $canonicalByNormalized[$norm]
            if ($studClub -cne $canonical) {
                $renameActions.Add([PSCustomObject]@{
                    Current   = $studClub
                    Canonical = $canonical
                })
            }
        } else {
            $trimmedName = Normalize-ClubName $studClub
            $newClubActions.Add([PSCustomObject]@{
                Current = $studClub
                NewName = $trimmedName
            })
        }
    }

    Write-Host "`n=== Report: Students.stud_club values requiring rename to canonical Club_Parameters.club_id ==="
    if ($renameActions.Count -eq 0) {
        Write-Host "  (none)"
    } else {
        foreach ($action in $renameActions) {
            Write-Host "  '$($action.Current)'  ->  '$($action.Canonical)'"
            if ($action.Canonical -eq 'Windsong' -or $action.Current -eq 'Windsong') {
                Write-Warning "  Refusing safety check: a proposed change touches the literal value 'Windsong'."
            }
        }
    }

    Write-Host "`n=== Report: stud_club values with NO match in Club_Parameters (will create new club) ==="
    if ($newClubActions.Count -eq 0) {
        Write-Host "  (none)"
    } else {
        foreach ($action in $newClubActions) {
            Write-Host "  '$($action.Current)'  ->  new Club_Parameters row: club_id = club_name = '$($action.NewName)'"
        }
    }

    # --- Informational: Club_Parameters rows with zero students ---
    $studClubNormSet = New-Object System.Collections.Generic.HashSet[string]
    foreach ($studClub in $studClubValues) {
        $studClubNormSet.Add((Normalize-ClubName $studClub).ToLowerInvariant()) | Out-Null
    }
    $emptyClubs = New-Object System.Collections.Generic.List[string]
    foreach ($clubId in $clubIds) {
        $norm = (Normalize-ClubName $clubId).ToLowerInvariant()
        if (-not $studClubNormSet.Contains($norm)) {
            $emptyClubs.Add($clubId)
        }
    }
    Write-Host "`n=== Report: Club_Parameters rows with zero students (informational only, not modified) ==="
    if ($emptyClubs.Count -eq 0) {
        Write-Host "  (none)"
    } else {
        foreach ($clubId in $emptyClubs) {
            Write-Host "  '$clubId'"
        }
    }

    if (-not $Apply) {
        Write-Host "`nDry run complete. No changes were made. Re-run with -Apply to perform these changes."
        return
    }

    Write-Host "`n=== Applying changes ==="

    function New-TextParam {
        param([string]$Value, [int]$Size = 100)
        $p = New-Object System.Data.OleDb.OleDbParameter
        $p.OleDbType = [System.Data.OleDb.OleDbType]::VarWChar
        $p.Size = $Size
        $p.Value = $Value
        return $p
    }

    foreach ($action in $renameActions) {
        if ($action.Canonical -eq 'Windsong' -or $action.Current -eq 'Windsong') {
            throw "Aborting: refusing to modify any row touching the literal value 'Windsong' (Current='$($action.Current)', Canonical='$($action.Canonical)')."
        }
        # Parameters are positional: order must match placeholder order in SQL.
        $updateCmd = $connection.CreateCommand()
        $updateCmd.CommandText = "UPDATE Students SET stud_club = ? WHERE stud_club = ?"
        $updateCmd.Parameters.Add((New-TextParam -Value $action.Canonical)) | Out-Null
        $updateCmd.Parameters.Add((New-TextParam -Value $action.Current)) | Out-Null
        $rowsAffected = $updateCmd.ExecuteNonQuery()
        Write-Host "  Updated $rowsAffected row(s): '$($action.Current)' -> '$($action.Canonical)'"
    }

    foreach ($action in $newClubActions) {
        $insertCmd = $connection.CreateCommand()
        $insertCmd.CommandText = "INSERT INTO Club_Parameters (club_id, club_name, club_active, annual_dues) VALUES (?, ?, ?, ?)"
        $insertCmd.Parameters.Add((New-TextParam -Value $action.NewName)) | Out-Null
        $insertCmd.Parameters.Add((New-TextParam -Value $action.NewName)) | Out-Null
        $activeParam = New-Object System.Data.OleDb.OleDbParameter
        $activeParam.OleDbType = [System.Data.OleDb.OleDbType]::Boolean
        $activeParam.Value = $true
        $insertCmd.Parameters.Add($activeParam) | Out-Null
        $duesParam = New-Object System.Data.OleDb.OleDbParameter
        $duesParam.OleDbType = [System.Data.OleDb.OleDbType]::Currency
        $duesParam.Value = 0
        $insertCmd.Parameters.Add($duesParam) | Out-Null
        $insertCmd.ExecuteNonQuery() | Out-Null
        Write-Host "  Inserted new Club_Parameters row: club_id = club_name = '$($action.NewName)'"

        # If the stud_club spelling itself needs normalizing (e.g. trimmed whitespace),
        # update Students rows to the new canonical (trimmed) spelling too.
        if ($action.Current -cne $action.NewName) {
            $updateCmd = $connection.CreateCommand()
            $updateCmd.CommandText = "UPDATE Students SET stud_club = ? WHERE stud_club = ?"
            $updateCmd.Parameters.Add((New-TextParam -Value $action.NewName)) | Out-Null
            $updateCmd.Parameters.Add((New-TextParam -Value $action.Current)) | Out-Null
            $rowsAffected = $updateCmd.ExecuteNonQuery()
            Write-Host "    Updated $rowsAffected Students row(s) to trimmed spelling '$($action.NewName)'"
        }
    }

    Write-Host "`nApply complete."
}
finally {
    $connection.Close()
}
