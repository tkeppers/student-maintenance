# Windsong Dojo Student Management — Claude Code Guide

## Project Overview

Windows Forms desktop application for Windsong Dojo (Oklahoma City) to manage student records,
attendance, and belt promotions across four martial arts: Aikido, Judo, Jyodo, and Iaido.

- **Framework:** .NET Framework 4.8, WinForms (WinExe)
- **Current version:** 2.7.0.0 (set in `Properties\AssemblyInfo.cs`)
- **Database:** Microsoft Access `.mdb` (Jet OLEDB 4.0), password-protected

---

## Build & Run Commands

### Build
```
msbuild DojoStudentManagement.csproj /p:Configuration=Debug
msbuild DojoStudentManagement.csproj /p:Configuration=Release
```
Or open `DojoStudentManagement.sln` in Visual Studio 2022 and use Ctrl+Shift+B.

### Run
```
# Kiosk mode (student check-in)
bin\Debug\DojoStudentManagement.exe

# Admin/maintenance mode
bin\Debug\DojoStudentManagement.exe maintenance
```

### Run Tests
```
dotnet test StudentManagementTests\DojoStudentManagementTests.csproj
```
Tests use NUnit 3.12 and target net48. No database connection is required — all tests use
in-memory objects only.

### First-Time Setup
1. Copy `App.config.sample` to `App.config`
2. Set `DatabasePath` to the actual `.mdb` file location
3. Set `DatabasePassword` to the database password
4. Set `LogFileLocation` to a writable **directory** path (not a file path)

---

## Architecture

### Launch Modes (`Program.cs`)
`RunApplication()` checks `args[0]` for `"maintenance"` (case-insensitive):
- No args → `StudentSignIn` form (kiosk)
- `maintenance` → `StudentMaintenanceUI` form (admin)

Both modes receive a `DataRepository` instance constructed at startup. If the database path is
invalid or unreachable, the app shows an error and exits before opening any form.

### Layer Breakdown

```
Forms (UI)
  └── *Functions classes (business logic)
        └── IDataRepository / DataRepository (data access)
              └── Microsoft Access .mdb (storage)
```

- **Forms** handle only display and user input; they call `*Functions` classes for logic.
- **`*Functions` classes** (`StudentMaintenanceFunctions`, `StudentSignInFunctions`, etc.) contain
  business rules and coordinate between the form and the repository.
- **`DataRepository`** executes all SQL against the Access database. It inherits from
  `BaseRepository` which owns the connection string and `ExecuteQuery()`.
- **`IDataRepository`** interface allows the test suite to substitute a fake repository without
  touching the database.
- **`MessageService`** (static) is the single place for all `MessageBox.Show` calls — use it
  instead of calling `MessageBox` directly from forms.

### Configuration (`App.config` / `ConfigurationManager`)
All settings are in `<appSettings>`:

| Key | Type | Purpose |
|-----|------|---------|
| `DatabasePath` | string | Path to the `.mdb` file |
| `DatabasePassword` | string | Jet OLEDB database password |
| `RepeatSignInHours` | double | Minimum hours between duplicate sign-ins (default 1.5) |
| `LogFileLocation` | string | **Directory** for log files (not a file path) |
| `ShowPromotionEligibilityOnSignIn` | bool | Show eligibility details on kiosk sign-in |

Settings can be changed at runtime via `ApplicationSettingsUI`; a restart is required for changes
to take effect. `BaseRepository` reads `DatabasePath` and `DatabasePassword` on every construction.

### Logging (Serilog)
- Configured in `Program.SetupLogging()` before any form opens.
- Monthly-rolling file sink: `SystemLog-.log` in `LogFileLocation`.
- Fallback chain if the configured path is unwritable:
  `LogFileLocation` → `%LOCALAPPDATA%\WindsongStudentMaintenance\Logs` → `%TEMP%\...`

---

## Data Model

### `Student`
Master record. Key calculated properties: `FullName`, `StudentAgeInYears` (uses 365.25 days/year).
`StudentArtsAndRanks` is a `List<StudentArtsAndRank>` populated by `PopulateArtsAndRanks()`.

`IsEligibleForPromotion(StudentArtsAndRank, PromotionCriteria)` checks four gates in order:
1. Minimum age
2. Cumulative training hours
3. Total years in art
4. Years at current rank

### `StudentArtsAndRank`
One row per (student, art) enrollment. `HoursInArt` is cumulative; `PromotionHours` records hours
at the time of the last promotion (so delta hours since promotion can be derived).
`PromoteStudentToNewLevel(string newRank)` updates rank and clears `NextRank`.

### `PromotionCriteria`
Loaded once at `StudentMaintenanceUI` startup into a `DataTable` held in memory.
`GetNextPromotionCriteria(StudentArtsAndRank)` looks up the matching row.
`RankFee` always returns 0 — fees are no longer charged but the property was kept.

### Database Tables
| Table | Primary Key | Purpose |
|-------|-------------|---------|
| `Students` | `stud_id` | Master student records |
| `StudArts` | `StudArt_ID` | Student–art enrollments and current rank |
| `Signin_History` | — | Append-only sign-in log |
| `Promo_History` | — | Append-only promotion log |
| `Promo_Requirements` | `rank_art`, `rank_id` | Promotion criteria per rank per art |
| `Arts` | `art_id` | Available arts and class hours |

`DataRepository` renames database columns to friendly names (e.g., `stud_lastName` →
`StudentLastName`) in every `SELECT` query so the rest of the code never sees raw column names.

---

## Forms

| Form | Mode | Opens from |
|------|------|-----------|
| `StudentSignIn` | Kiosk | Program entry point |
| `StudentMaintenanceUI` | Admin | Program entry point (`maintenance` arg) |
| `StudentAddUI` | Admin | StudentMaintenanceUI toolbar |
| `StudentAddModifyArtUI` | Admin | StudentMaintenanceUI (Add Art / Edit Art) |
| `PromoteStudentUI` | Admin | StudentMaintenanceUI (Promote Student) |
| `PromotionCriteriaUI` | Admin | StudentMaintenanceUI menu |
| `StudentSignInHistoryUI` | Admin | StudentMaintenanceUI (View History) |
| `StudentPromotionHistoryUI` | Admin | StudentMaintenanceUI (View History) |
| `ApplicationSettingsUI` | Admin | StudentMaintenanceUI menu |
| `AboutProgramUI` | Admin | StudentMaintenanceUI menu |

Dialog forms (`StudentAddUI`, `StudentAddModifyArtUI`, `PromoteStudentUI`) raise events
(e.g., `StudentAdded`) that the parent form subscribes to for refresh.

---

## Coding Conventions

- **Naming:** PascalCase for classes, methods, properties. camelCase for local variables and
  private fields. Form controls keep their Designer-generated names.
- **No direct `MessageBox` calls from forms** — always use `MessageService.ShowErrorMessage()`,
  `ShowInformationMessage()`, or `ShowAreYouSureMessage()`.
- **Parameterized queries everywhere** — `DataRepository` uses `OleDbParameter` for all
  INSERT/UPDATE. Never concatenate user input into SQL strings.
- **Transactions for multi-step writes** — promotion (update rank + insert history) and sign-in
  (insert history + update hours) both use `OleDbTransaction` with rollback on error.
- **`IDataRepository` for testability** — pass the interface, not the concrete class, to
  `*Functions` classes and forms so tests can inject a fake.
- **Tests use in-memory data only** — no database, no file I/O. Create domain objects directly
  in `[SetUp]` and helper methods. Use `[TestCase]` for data-driven coverage.

---

## Known Issues & Areas for Improvement

### Configuration Bug
`LogFileLocation` in the live `App.config` is set to the `.mdb` file path instead of a log
directory. The fallback to `%LOCALAPPDATA%` masks the error at runtime but logs go to the wrong
place. Fix: set `LogFileLocation` to a proper directory in `App.config`.

### Sensitive Credentials in Config
`DatabasePassword` is stored in plaintext in `App.config`. For a shared or version-controlled
setup, consider DPAPI encryption or Windows Credential Manager.

### `DataRepository` is a Large Class
All 16+ data-access methods live in one class (~600+ lines). Consider splitting into focused
repositories: `StudentRepository`, `SignInRepository`, `PromotionRepository`.

### Promotion Criteria Loaded as a Raw `DataTable`
`GetStudentPromotionRequirements()` returns a `DataTable` that `StudentMaintenanceUI` holds in a
field and passes around. Mapping it to `List<PromotionCriteria>` at load time would make the
calling code simpler and type-safe.

### `PromotionCriteriaUI` Tracks State with `DataRow.RowState`
The grid edit form relies on ADO.NET `RowState` (Modified/Added/Deleted) for its save logic.
This is fragile with Access; any accidental `AcceptChanges()` call silently loses pending edits.
An explicit change-tracking model (add/modify/delete lists) would be more reliable.

### No Input Sanitization on Free-Text Fields
Address, phone, and name fields have no max-length enforcement in code (only whatever the DB
column allows). Adding `MaxLength` constraints on TextBox controls and trimming on save would
prevent silent truncation by Access.

### Commented-Out Code in Tests
`StudentTests.cs` has a commented-out `CreateIneligibleStudentArts` helper method (lines 103–119).
Either complete the test coverage it was meant for or remove it.

### `RankFee` Property is Dead Code
`PromotionCriteria.RankFee` always returns 0. Remove it unless fees will be reinstated.

### No Automated UI Tests
All existing tests cover pure domain logic. The sign-in validation flow
(`StudentSignInFunctions`) and the promotion workflow are not covered. Adding NUnit tests with a
mock `IDataRepository` for these paths would catch regressions cheaply.
