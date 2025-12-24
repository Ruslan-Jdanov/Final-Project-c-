# Computer Equipment Warehouse (Warehouse Search System)

Console application to search products in a warehouse inventory.

## Requirements implemented
- Multilayer architecture: Domain, Dal, Services, UI
- Reads inventory from `inventory.txt`
- Search by name, id, category, price range
- List with sorting
- Add / Save / Reload commands
- report.md included

## Environment Requirements

This project targets .NET 9 (net9.0). Before building, make sure you have .NET SDK 9.x installed.

Check the installed SDK version:
```bash
dotnet --list-sdks
```
If the output does not contain the string `9.x`, install the .NET 9 SDK:
- Windows / macOS / Linux: Download and install from the official [.NET downloads](https://dotnet.microsoft.com/en-us/download/dotnet/9.0).

After installation, check `dotnet --list-sdks` again. Then run the usual sequence:
```bash
dotnet restore
dotnet build
dotnet test ./Tests/FinalProject.Tests.csproj
dotnet run --project .
```

## Build & run
If you already have .NET 9

1. Build:
```
   dotnet build
```

2. Run:
```
   dotnet run --project .
```
(or run from your IDE)

3. Where to put `inventory.txt` if you have problem with it:
   - The app looks for `inventory.txt` in the application base directory (where the executable runs).
   - If not found there, it will try the current working directory.
   - For development, place `inventory.txt` in the repository root (so `dotnet run` finds it), or next to the built executable.

## Commands (in app)
- help — show commands
- list [order] [asc|desc] — list all products. order: name|price|category|id (default: name asc)
  Example: `list price desc`
- search name <text> — partial, case-insensitive
- search id <ID> — exact ID
- search category <category>
- search price <min> <max>
- show <ID> — print full details
- add — interactive add (use `save` to persist)
- save — write current products to `inventory.txt`
- reload — reload data from file
- exit — exit

## Tests
A test project (xUnit) is included under `Tests/`. To run tests:
```
dotnet test ./Tests
```

If you have a different target framework, update the Tests project `<TargetFramework>` accordingly.