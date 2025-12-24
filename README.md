# Computer Equipment Warehouse (консольное приложение)

Кратко: небольшая консольная программа для поиска товаров на складе компьютерной техники на основе текстового файла inventory.txt.

Требования:
- .NET 6 или выше

Запуск:
1. Скопируй все файлы проекта в папку.
2. Открой терминал в папке проекта.
3. Выполни:
   - dotnet new console --force
   - dotnet run

Файл данных:
- inventory.txt (формат строк: ID|Name|Category|Price|Quantity|Description)
- Комментарии в файле начинаются с `#`.

Команды в приложении:
- help — показать команды
- list [order] [asc|desc] — показать список товаров (order: name|price|category|id)
- search name <текст>
- search id <ID>
- search category <категория>
- search price <min> <max>
- show <ID>
- reload — перезагрузить данные из файла
- exit — выйти

Как изменить имя приложения / разработчика:
- В файле Program.cs заменить константы APP_NAME, VERSION, CREATION_DATE, DEVELOPER_NAME, DEVELOPER_EMAIL.

Если хочешь — могу:
- добавить команды для добавления/редактирования товаров с сохранением в файл,
- поменять формат на JSON,
- подготовить презентацию (слайды) и краткую речь для защиты.