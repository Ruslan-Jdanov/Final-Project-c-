# Computer Equipment Warehouse (консольное приложение)

Briefly: a small console program for searching for items in a computer equipment warehouse based on the text file inventory.txt.

Requirements:
- .NET 6 or higher

Launch:
1. Copy all project files to the folder.
2. Open a terminal in the project folder.
3. Execute:
   - dotnet new console --force
   - dotnet run

Data file:
- inventory.txt (string format: ID|Name|Category|Price|Quantity|Description)
- Comments in the file begin with `#`.

Commands in the application:
- help — show commands
- list [order] [asc|desc] — show list of products (order: name|price|category|id)
- search name <text>
- search id <ID>
- search category <category>
- search price <min> <max>
- show <ID>
- reload — reload data from file
- exit — exit, ig