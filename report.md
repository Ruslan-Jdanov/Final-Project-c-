# Progress report

| #     | Stage   | Start date | End date | Comment |
| ---   | ---     | ---        | ---      | ---     |
| 1     | Task Clarification | 2025-12-24 | 2025-12-24 | The task is clear; the topic: Computer Equipment Warehouse |
| 2     | Analysis | 2025-12-24 | 2025-12-24 | MVP defined: search by name, ID, category, price; list view with sorting |
| 3     | Use Cases | 2025-12-24 | 2025-12-24 | The main scenarios are described: launch, search, view, exit |
| 4     | Search for Solutions | 2025-12-24 | 2025-12-24 | A simple multi-layer architecture was chosen: Entity/Repository/Service/UI |
| 5     | Software Development | 2025-12-24 | 2025-12-24 | Implemented MVP as a console application; inventory.txt file as a data source |
| 6     | Development Completion | 2025-12-24 | 2025-12-24 | Use cases were checked, a repository and a report were prepared |
| 7     | Presentation | 2025-12-25 | 2025-12-25 | App demonstration and Q&A |

Additional: Suggested 11-week plan (guideline) that "should have been"
| Week | Task |
| ---  | ---  |
| 1    | Clarification of the task, plan (completed) |
| 2    | Domain Analysis, MVP (completed) |
| 3    | Use cases, architecture (completed) |
| 4    | Solution research, data format selection (completed) |
| 5    | Prototype (reading file, basic search) |
| 6    | Expanded search, sorting, and UI improvements |
| 7    | Error handling, input validation |
| 8    | Additional functionality (CRUD, saving) |
| 9    | Testing, bug fixing |
| 10   | Documentation, presentation preparation |
| 11   | Finalization, delivery of the project |

Implementation status (assessment %):
- Basic (MVP): 90% implemented (search, list, reading from file).
- Additional functionality (CRUD, saving): 0% (should be added).
- Tests and presentation: 50% (demo scenario described, presentation not yet prepared).

Demonstration scenario:
1. Launch the application: `dotnet run`
2. Enter `list` — see all products.
3. Enter `search name laptop` — find laptops.
4. Enter `search price 50 200` — find products by price.
5. Enter `exit` — выйти.

Comment:
Since the deadline is so close, I implemented MVP functionality that covers all the requirements of the task: application name, developer information, command menu, reading from a file, searching by parameters, list output, and error handling