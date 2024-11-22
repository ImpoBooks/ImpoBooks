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

## Functional Capabilities

1. **User Registration/Login** - *Users can register a new account or log in to an existing one.*
2. **Retrieve User Information** - *Fetch detailed information about a specific user.*
3. **Fetch All Books from the Database** - *Retrieve a complete list of books stored in the database.*
4. **Create a Book Instance** - *Add a new book entry to the system.*
5. **Edit a Book Instance** - *Modify the details of an existing book.*
6. **Delete a Book Instance** - *Remove a specific book entry from the system.*
7. **View Product Page** - *Access detailed information about a specific product.*
8. **User Comments Creation** - *Allow users to leave comments on products.*
9. **Product Rating by Users** - *Enable users to rate products based on their experience.*
10. **Like/Dislike Comments** - *Users can like or dislike comments made by others.*

## Non-Functional Requirements Compliance

1. **Code Scalability** - *The system is designed to ensure that the codebase can handle increased load and complexity with minimal performance degradation.*
2. **System Reliability** - *The platform guarantees stable operation under various conditions, minimizing downtime and errors.*
3. **Maintainability** - *The codebase is structured to simplify updates, debugging, and enhancements, ensuring long-term supportability.*
4. **Performance** - *The system is optimized to deliver high-speed operation and responsiveness under typical and peak loads.*
5. **Portability** - *The application can be deployed and run across different platforms and environments with minimal adjustments.*

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

## Project deploy
You can visit the deployed project by following this [link](https://impobooks-f2f6hqcjfeccc7c4.westeurope-01.azurewebsites.net/swagger/index.html).

