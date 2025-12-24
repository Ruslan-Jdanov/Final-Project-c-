# Computer Equipment Warehouse (console application)

Short: a small console application for searching products in a computer equipment warehouse. Data is stored in a pipe-separated text file (inventory.txt).

Requirements:
- .NET 6 or newer

How to run:
1. Copy project files to a folder.
2. Open terminal in that folder.
3. Run:
   - dotnet new console --force
   - dotnet run

Data file:
- inventory.txt (line format: ID|Name|Category|Price|Quantity|Description)
- Lines starting with `#` are treated as comments.

Commands:
- help — show commands
- list [order] [asc|desc] — show products (order keys: name|price|category|id)
- search name <text>
- search id <ID>
- search category <category>
- search price <min> <max>
- show <ID>
- add — add a new product interactively (product is added to memory; use `save` to persist)
- save — save current products to inventory.txt
- reload — reload data from inventory.txt (discard unsaved changes)
- exit — exit the application