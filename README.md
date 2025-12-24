# Computer Equipment Warehouse (Course Project)

Console app to search products from an inventory file.  
This repository contains a simple multilayer C# console application (Entities, Repositories, Services, UI).

## Requirements
- .NET 7.0 SDK (or compatible .NET SDK installed)

## Build and run
1. Build:
   dotnet build

2. Run:
   dotnet run

(If project root is different or dotnet run fails, run: dotnet run --project ./zxc.csproj)

## Commands (available in the app)
- help                          - Show help
- list [order] [asc|desc]       - Show all products. order: name|price|category|id (default: name asc)
  Example: list price desc
- search name <text>            - Search by name (partial, case-insensitive)
- search id <ID>                - Search by exact ID
- search category <category>    - Search by category
- search price <min> <max>      - Search by price range (e.g. search price 10 200)
- show <ID>                     - Show full product details by ID
- add                           - Add a new product (interactive)
- save                          - Save current products to inventory.txt
- reload                        - Reload data from file
- exit                          - Exit

## Inventory file format
Each non-comment line must be:
ID|Name|Category|Price|Quantity|Description

Lines starting with `#` are ignored.

Example:
CE-001|Gaming Laptop X15|Laptop|1499.99|10|High-performance gaming laptop with RTX.

## Tests
A test project using xUnit is provided in `zxc.Tests`. To run tests:

dotnet test