# Windsong Dojo Student Management — Claude Code Guide

Windows Forms desktop application for Windsong Dojo (Oklahoma City) to manage student records,
attendance, and belt promotions across four martial arts: Aikido, Judo, Jyodo, and Iaido. Since
August 2026 it also tracks the wider **Kaze Uta Budo Kai (KUBK)** organization — the other member
dojos, their rosters, ranks, promotions, and annual dues.

- **Framework:** .NET Framework 4.8, WinForms (WinExe), runs as **32-bit** (AnyCPU + Prefer32Bit)
- **Current version:** 2.7.0.0 (`Properties\AssemblyInfo.cs`)
- **Database:** Microsoft Access `.mdb` (Jet OLEDB 4.0), password-protected

---

## Current state — read this first

Work lives on branch **`phase-2-kubk-repository-domain`** (the name is stale — it now carries
Phases 1–5 plus two extra reports). 18 commits ahead of `bug-fixes` at `c4e212d`.

**Phases 1–5 of the KUBK plan are complete.** Plans live in `..\KUBK-Implementation-Plans\`.
Phase 6 (SQLite migration) has not been started.

**220 NUnit tests pass.** Solution builds clean with no warnings.

The only uncommitted file is `DojoStudentManagement.csproj.user` — a local VS debug setting
(`StartArguments = maintenance`). It is tracked despite `*.user` being in `.gitignore`, so it
shows as permanently dirty. Deliberately left uncommitted.

### Commits this session (oldest first)

| Commit | What |
|---|---|
| `6386dca` | Phase 1 migration scripts + this file |
| `a7441ad` | Phase 2 repository & domain layer |
| `aff594a` | Phase 3 dojo management & KUBK roster UI |
| `42d3a28` | Fix: `IsValidStudent` no longer opens a message box |
| `f071a0b` | Phase 4 KUBK promotion flow |
| `0eb037f` | Fix: promotions work on an unmigrated database |
| `1a8b855` | Omit Windsong from the KUBK roster (user decision) |
| `b18e0fa` | User's toolbar/button icons |
| `3554383` | Fix: no orphaned data when a promotion or dojo save fails |
| `65789ca` | Dues record no amount instead of zero |
| `9bccc78` | Phase 5 rank re-verification + KUBK reports |
| `c7064ca` | Fix: resolve grid rows by `Tag`, not row index |
| `fd9454b` | Fix: four remaining review findings |
| `7a45eff` | Fix: five findings from a second review |
| `fed365b` | Student activity report + roster keeps its sort |
| `439e802` | Highlight promotion-eligible students |
| `c40ac55` | Sign-in history report |
| `77a5c24` | Sign-in history shows a running hours total |
| (uncommitted) | Add Student workflow for member dojos |

---

## Databases — important

| Path | What it is |
|---|---|
| `..\..\DojoBosu.mdb` | The **development** database. Migrated. **This is not production.** |
| `..\..\DojoBosu.mdb.backup-20260823-182606` | Pre-migration snapshot. Useful for testing old-schema compatibility. |
| (elsewhere) | **Production has NOT been migrated.** Ask before touching it. |

Approximate dev-database sizes: 1,761 students · 2,421 `StudArts` rows · 3,534 promotions ·
41,714 sign-ins (2015→2026) · 38 dojos · 1,558 Windsong students / 203 at member dojos.

Because production is unmigrated, the app is written to **degrade gracefully** on the old schema
(see "Schema capability checks" below). That is load-bearing, not defensive.

### Migration scripts — `Database Migration\`

```powershell
# Idempotent. Adds KUBK columns, KUBK_Dues, index; seeds club_active; stamps Windsong ranks verified.
.\migrate-kubk-schema.ps1 -DatabasePath "<path>" -DatabasePassword "<pw>"

# Dry-run by default; -Apply runs inside a transaction and is all-or-nothing.
.\cleanup-club-names.ps1 -DatabasePath "<path>" -DatabasePassword "<pw>" [-Apply]
```

Always run against a copy first. Both scripts have been exercised against real data, including
their failure paths (oversized club names, interrupted runs, mid-apply aborts).

---

## Build, test, run

**`dotnet test` does not work here.** SDK 9 cannot create an x86 task host for the WinForms
project (`MSB4216`). This is an environment problem, not a code problem. Use:

```powershell
# Build (tests project builds the app too)
& "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\amd64\MSBuild.exe" `
    "StudentManagementTests\DojoStudentManagementTests.csproj" /p:Configuration=Debug /nologo /v:minimal

# Test
& "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe" `
    "StudentManagementTests\bin\Debug\net48\DojoStudentManagementTests.dll"
```

Run: `bin\Debug\DojoStudentManagement.exe` (kiosk) or `... maintenance` (admin).

> **`bin\Debug\DojoStudentManagement.exe.config` is regenerated from `App.config` on every
> build.** To test against a scratch copy, edit the *built* config after building — and expect a
> rebuild to reset it to the dev database.

---

## Verifying against a real database (harness pattern)

Unit tests use fakes and cannot catch SQL or form-wiring bugs — several real defects this session
were found only this way. The pattern: a small console exe that loads the built app assembly by
reflection, drives real code against a **scratch copy**, and prints what the database actually
holds.

```powershell
# Compile with the ROSLYN compiler. The legacy Framework64\v4.0.30319\csc.exe is C# 5 and
# rejects expression-bodied members, ?., and string interpolation.
& "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\Roslyn\csc.exe" `
    /target:exe /platform:x86 /out:"$sp\Harness.exe" /r:System.dll /r:System.Core.dll `
    /r:System.Data.dll /r:System.Windows.Forms.dll "$sp\Harness.cs"

Copy-Item "$sp\Harness.exe"        "$bin\Harness.exe"        -Force
Copy-Item "$sp\Harness.exe.config" "$bin\Harness.exe.config" -Force   # ← do not skip
& "$bin\Harness.exe"
```

Three rules learned the hard way:

1. **`/platform:x86`** — the app uses Jet, which is 32-bit only.
2. **Always copy the `.config`.** Without `DatabasePath`, `DatabaseExistsAndIsValid()` opens a
   modal "Database is invalid or inaccessible" dialog **on the user's screen** and returns empty
   data that looks like a bug.
3. **Delete the harness from `bin\Debug` afterwards.** Leaving them pollutes the build output.

Harnesses can also drive real forms (`Activator.CreateInstance`, hook `Shown`, read private
fields by reflection, `form.Close()`), which is how the grid-sorting bugs were proved and fixed.

---

## OLE DB providers

| Context | Provider |
|---|---|
| The app, and any x86 harness | `Microsoft.Jet.OLEDB.4.0` |
| 64-bit PowerShell, migration scripts | `Microsoft.ACE.OLEDB.16.0` |

ACE 16.0 is registered 64-bit only on this machine; Jet is 32-bit only. Using the wrong one gives
"provider is not registered on the local machine".

---

## Architecture

```
Forms (UI)  →  *Functions classes (business logic)  →  IDataRepository / DataRepository  →  .mdb
```

Forms render and collect input only. All SQL lives in `DataRepository`. `IDataRepository` exists
so tests can inject a fake — there are two fake implementations across the test project
(`FakeKubkDataRepository` in `KubkManagementFunctionsTests.cs`, shared by the other KUBK test
files, and `FakeDataRepository` in `StudentMaintenanceFunctionsBugFixTests.cs`) — and **every new
interface member must be added to both** or the test project stops compiling.

### Business-logic classes

| Class | Responsibility |
|---|---|
| `StudentMaintenanceFunctions` | Windsong student load/validate |
| `StudentSignInFunctions` | Kiosk sign-in validation and eligibility |
| `KubkManagementFunctions` | Dojos, KUBK roster, dues, promotions, verification, member-dojo registration, KUBK reports |
| `StudentActivityReportFunctions` | Windsong activity report + promotion eligibility |
| `SignInHistoryReportFunctions` | Sign-in history report + running hours totals |
| `CsvWriter` | RFC 4180 CSV escaping, UTF-8 **with BOM** so Excel reads names correctly |

Report assembly is deliberately static and pure where possible, so it is unit-testable without a
database. Forms only render what these return.

---

## Data model

### Tables

| Table | Key | Purpose |
|---|---|---|
| `Students` | `stud_id` | Master records. `stud_club` is the dojo, as **text** |
| `StudArts` | `StudArt_ID` + `studArt_art` | Per-(student, art) enrollment, rank, hours |
| `Signin_History` | `sign_id` | Append-only attendance log |
| `Promo_History` | `promo_id` | Append-only promotion log |
| `Promo_Requirements` | `rank_art` + `rank_id` | Promotion criteria per art per rank |
| `Arts` | `art_id` | Available arts and hours per class |
| `Ranks` | `rank_id` | Rank ladder: WHITE(1) → HACHIDAN(13), `rank_next` = "NA" at top |
| `Club_Parameters` | `club_id` | One row per dojo |
| `KUBK_Dues` | `dues_id` | **Added Phase 1.** Unique index on (`dues_student`, `dues_year`) |

### Columns added by the Phase 1 migration

`Club_Parameters`: `club_instructor`, `club_instructor_email`, `club_active`, `club_notes`
`StudArts`: `studArt_rank_verified` · `Promo_History`: `promo_recommended_by`
`App.config`: `KubkAnnualDuesAmount` (default 70)

### Access column widths that bite

`club_id` **TEXT(10)** · `club_name` TEXT(30) · `club_addr1/2` TEXT(30) · `club_addr3` TEXT(25) ·
`club_phone` TEXT(50) · `club_instructor`/`_email` TEXT(100)

**`Students` columns are much narrower than they look.** `stud_firstname` **TEXT(15)** ·
`stud_lastname` **TEXT(20)** · `stud_email` TEXT(70) · `stud_homephone` TEXT(17) ·
`stud_workphone` TEXT(13) · `stud_club` TEXT(50) · `stud_city` TEXT(25) · `stud_addr1/2` TEXT(30) ·
`stud_state` TEXT(2) · `stud_zip` TEXT(10) · `stud_status`/`stud_gender` TEXT(1).

Anything longer fails the insert. `KubkAddStudentUI` sets `MaxLength` on its text boxes to match;
**`StudentAddUI` does not**, so a long name typed on the Windsong screen still fails at save time.

`Students` NOT NULL: `stud_id`, `stud_status`, `stud_gender`, `stud_lastname`, `stud_club`.
Inserting without those fails. `stud_birthdate` **is** nullable.

`stud_id` is a real AutoNumber, and `SELECT @@IDENTITY` on the same open connection returns it —
verified against the dev database with both Jet (x86) and ACE 16.0. That is what
`AddNewStudent(student, out int)` uses.

---

## Forms

| Form | Opens from |
|---|---|
| `StudentSignIn` | Program entry (kiosk) |
| `StudentMaintenanceUI` | Program entry (`maintenance`) |
| `StudentAddUI`, `StudentAddModifyArtUI`, `PromoteStudentUI` | Maintenance toolbar/buttons |
| `PromotionCriteriaUI`, `ApplicationSettingsUI`, `AboutProgramUI` | Maintenance menus |
| `StudentSignInHistoryUI`, `StudentPromotionHistoryUI` | Maintenance (View History) |
| **`DojoManagementUI`** | KUBK ▸ Member Dojos… |
| **`KubkRosterUI`** | KUBK ▸ Student Roster… |
| **`KubkPromoteStudentUI`** | KubkRosterUI ▸ Promote Student… |
| **`KubkAddStudentUI`** | KubkRosterUI ▸ Add Student… |
| **`RankCorrectionUI`** | KubkRosterUI ▸ Correct Rank… |
| **`KubkReportsUI`** | KUBK ▸ Reports ▸ Rank Register… / Unpaid Dues… |
| **`StudentActivityReportUI`** | Reports ▸ Student Activity… |
| **`SignInHistoryReportUI`** | Reports ▸ Sign-In History… |

The **KUBK** and **Reports** toolbar buttons are `ToolStripDropDownButton`s. `Art Maintenance`
(`toolStripButton2`) remains a disabled, unimplemented placeholder.

> `toolStripButton3` is the Reports button. **Do not rename it** — its icon is stored in
> `StudentMaintenanceUI.resx` under the key `toolStripButton3.Image`, and renaming the control
> would make the VS designer drop the image on its next save.

---

## Gotchas — each of these caused a real bug

### Data access

- **OleDb parameters are positional, not named.** Add them in exactly the order the placeholders
  appear, including any conditionally-included ones.
- **`GetStudentTable()` (parameterless) is Windsong-only.** Use `GetStudentTable(dojoFilter)`;
  `null` means all dojos. `StudentMaintenanceFunctions.PopulateStudentData` still uses the
  Windsong-only overload — anything needing member-dojo students must not go through it.
- **`DeleteStudentArt(id, null)` deletes *every* art for that student** — the art predicate is
  dropped when the name is null. `RemoveStudentArtEnrollment` guards against this.
- **Jet `DISTINCT`/`GROUP BY` is case-insensitive**, which hides case mismatches. Pull raw rows
  and de-duplicate in code when case matters.
- **Jet has no `COUNT(DISTINCT ...)`.**
- **`GetSchema("Columns", restrictions)` throws under ACE 16.0.** Fetch the whole schema and
  filter in code.
- **A YESNO column added by `ALTER TABLE` defaults to False, not NULL** — you cannot detect
  "unseeded" by looking for NULLs.
- **Jet cannot `LEFT JOIN` on a compound condition with a parameter.** Fetch separately and merge
  in memory (see the dues merge in `GetKubkRoster`).
- **A `Student` with no birthdate set holds `DateTime.MinValue`, whose year 1 is outside the range
  Access accepts** and fails the insert. `SetStudentCommandParameters` writes `DBNull` for it, so
  an unknown birthdate is stored as null — which is how `StudentActivityReportFunctions` already
  reads that column. Only the KUBK registration flow leaves it unset; the Windsong form always
  supplies a date from its picker.

### Schema capability checks

`DataRepository.SupportsRankVerifiedColumn` / `SupportsRecommendedByColumn` are cached per
instance and make the promotion SQL adapt to whichever columns exist. This keeps **Windsong
promotions working on an unmigrated database** — without it the shared promotion transaction
fails silently. Do not remove until production is migrated.

### WinForms grids

- **Unbound `DataGridView` columns default to `SortMode.Automatic`**, so clicking a header sorts
  the rows. **Never map a row index into a backing list.** Every grid stores its entry on
  `DataGridViewRow.Tag` and reads it back from there.
- **Rebuilding rows loses the sort but keeps the sort glyph**, so the list silently reshuffles
  while still claiming to be sorted. Capture `SortedColumn`/`SortOrder` before rebuilding and
  re-apply afterwards (`KubkRosterUI.ReapplySort`).
- **`CellContentClick` only fires for mouse clicks on a checkbox glyph.** For a checkbox column
  that must react to the keyboard too, commit on `CurrentCellDirtyStateChanged` and act on
  `CellValueChanged`, guarded by a suppression flag so rebuilding rows doesn't re-enter.

### Adding a student to a member dojo

`KubkRosterUI ▸ Add Student…` opens `KubkAddStudentUI`, which is deliberately **not** `StudentAddUI`:

- **The dojo is a dropdown of active member dojos, never text.** `Students.stud_club` has to match
  `Club_Parameters.club_id` exactly or the student falls off every per-dojo view — the reason
  `cleanup-club-names.ps1` exists. `StudentAddUI` still has a free-text "Home Dojo" box defaulting
  to "Windsong"; it can technically create a member-dojo student, and that path is the one that
  produces orphans.
- **An art and rank are required.** `GetKubkRoster` inner-joins `StudArts`, so a student with no
  enrollment never appears on the roster and the add looks like it silently failed.
- **The new rank is stored unverified**, so the student lands in the existing re-verification
  queue (amber row, one click on Verify Rank) rather than arriving pre-blessed.
- **"Held since" feeds `studArt_begin` *and* `studArt_prodate`.** Defaulting it to today would show
  a transferring yondan as newly promoted in the roster's Years at Rank column and the rank
  register.
- **Same-name students trigger a warning, not a block.** There are already 8 duplicate Windsong
  names (see Outstanding work), genuine namesakes exist, and there is no merge tool.
- **The two inserts are not one transaction** (Jet cannot span them from here). A failed `StudArts`
  insert deletes the student just created, so a failed add leaves nothing behind; if that
  compensating delete also fails, the message names the student id for manual cleanup.

After a successful add the roster switches to that student's dojo if it was filtered to a
different one — otherwise the student is invisible and the add reads as a failure.

### Domain

- **`PromotionCriteria.GetNextPromotionCriteria` only writes its fields when it finds exactly one
  match.** A shared instance silently keeps the previous student's thresholds, and its stale
  `NextRank` slips past the "no criteria on record" guard in `IsEligibleForPromotion`. **Build a
  fresh `PromotionCriteria` per row.**
- **Rank corrections must not write `Promo_History`.** `CorrectStudentRank` updates the rank and
  the verified stamp only; the Serilog line is the audit trail. Real promotions go through
  `UpdateStudentPromotion`.
- **Business logic must not open message boxes.** `IsValidStudent` used to, which blocked the
  test suite for hours at a time. `BaseRepository.DatabaseExistsAndIsValid()` still does — worth
  fixing if it ever needs to run unattended.

---

## Coding conventions

- PascalCase for types/methods/properties; camelCase for locals and private fields.
- **No direct `MessageBox`** — use `MessageService`.
- **Parameterized SQL only, and only inside repository classes.**
- **Multi-step writes use `OleDbTransaction`** with rollback.
- **Update methods should verify affected rows.** `CorrectStudentRank` and
  `SetStudentActiveStatus` require exactly one, so a vanished record reports failure instead of a
  false success.
- **Confirm before writing.** A cancelled action must leave no trace; where two writes cannot be
  one transaction (enroll-then-promote), the first is compensated on failure.
- **Tests are in-memory only** — fakes, no database, no file I/O.

---

## Outstanding work

### Phase 6 — SQLite migration (not started)
See `..\KUBK-Implementation-Plans\phase-6-sqlite-migration.md`. Two things agreed for it:

- **Numeric dojo key.** `Club_Parameters.club_id` is a natural text key that `Students.stud_club`
  copies, which is why Club ID is read-only once a dojo exists — renaming would orphan students.
  A surrogate `dojo_id` belongs in the schema rewrite, not a standalone migration.
- Revisit whether `KUBK_Dues.dues_amount` is worth keeping at all.

### Decisions waiting on the user

1. **"Non-Windsong Students" checkbox is broken and always has been.** `RefreshStudentList()`
   loads the Windsong-only `GetStudentTable()`, so unchecking a filter cannot reveal rows that
   were never loaded. Fixing it is two one-liners (`GetStudentTable(null)` in
   `RefreshStudentList` *and* in `PopulateStudentData`, or the detail panel blanks) — but it
   would let the Windsong screen edit all 1,761 students, overlapping `KubkRosterUI`. Options:
   wire it up, or remove the checkbox. **Now that member-dojo students are added and managed
   entirely from `KubkRosterUI`, removing the checkbox is the cleaner of the two.**
2. **Duplicate student records.** 8 Windsong names appear on two records; 5 follow an
   active-shadows-inactive re-enrolment pattern (e.g. GREG ABLES: inactive id 7650 holds
   Judo HACHIDAN since 1983; active id 9626 was created 3/21/2026 at WHITE). Merging means moving
   `StudArts` rows to the surviving id. Needs the user to choose the survivor in each case.
3. **The dues feature may not be wanted.** Deliberately minimal (one checkbox column,
   `SetDuesPaid`) so it is cheap to remove.
4. **Branch rename** — `phase-2-kubk-repository-domain` covers far more than Phase 2.
5. **Migrate production** when ready, on a copy first.

### Known weakness in the testing

Every real defect this session lived in **form event handling or repository SQL**, which the
194 tests do not touch — they use fakes. Two independent review passes found 12 genuine bugs
between them. Treat "tests pass" as weak evidence for those layers; verify against a scratch
database with a harness, and consider a third review before production use.

### Older items still open

- `LogFileLocation` in `App.config` points at the `.mdb` file, not a directory (masked by a
  fallback to `%LOCALAPPDATA%`).
- `DatabasePassword` is plaintext in `App.config`.
- `DataRepository` is now ~1,900 lines and wants splitting.
- `PromotionCriteriaUI` still tracks edits via `DataRow.RowState` (fragile —
  `DojoManagementUI` deliberately does not).
- `PromotionCriteria.RankFee` is dead code, always 0.
- `StudentTests.cs` has a commented-out `CreateIneligibleStudentArts` helper.
