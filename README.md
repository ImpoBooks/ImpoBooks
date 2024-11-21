# ImpoBooks
Modern bookstore

## Project structure

**ImpoBooks.sln**\
&ensp;|-- ImpoBooks.Server\
&ensp;| &ensp;&ensp;  |-- Controllers\
&ensp;| &ensp;&ensp;  |-- DbInitializer\
&ensp;| &ensp;&ensp;  |-- Extensions\
&ensp;| &ensp;&ensp;  |-- Middleware\
&ensp;| &ensp;&ensp;  |-- Requests\
&ensp;| &ensp;&ensp;  |-- Responses\
&ensp;| &ensp;&ensp;  |-- Program.cs(Entry point)\
&ensp;|-- ImpoBooks.BusinessLogic\
&ensp;| &ensp;&ensp;  |-- Services\
&ensp;|-- ImpoBooks.DataAccess\
&ensp;| &ensp;&ensp; |-- Interfaces\
&ensp;| &ensp;&ensp; |-- Repositories\
&ensp;| &ensp;&ensp; |-- Entities\
&ensp;| &ensp;&ensp; |-- Extensions\
&ensp;|-- ImpoBooks.Infrastructure\
&ensp;| &ensp;&ensp; |-- Errors\
&ensp;| &ensp;&ensp;  |-- Hasher\
&ensp;| &ensp;&ensp;  |-- Providers\
&ensp;|-- ImpoBooks.Tests\
   &ensp;&ensp; |-- E2e\
   &ensp;&ensp; |-- Integration\
   &ensp;&ensp; |-- Unit\

## Developers

[Nazarii Horchynski](https://github.com/Nazg0r)

[Oleksandr Bondarenko](https://github.com/DreammyOleksandr)
    
## Setup

To build this application [.NET 8.0](https://dotnet.microsoft.com/ru-ru/download/dotnet/8.0) has to be installed on your machine.

**To build and run this application move to `src/ImpoBooks`**

```
cd src/ImpoBooks
```

**Then start server with a command**

```
dotnet run --launch-profile https
```
or
```
dotnet run --launch-profile http
```

## Tests

**To build and test this application move to `src/ImpoBooks`**

```
cd src/ImpoBooks
```
**Then run tests with a command**

```
dotnet test
```

