# Online Library Management System

An ASP.NET Core MVC application for managing library books, user accounts, and borrowing operations.

## Prerequisites

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [SQL Server 2019+](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

## Getting Started

### 1. Clone the Repository
```bash
git clone https://github.com/yourusername/OnlineLibrary.git
cd OnlineLibrary
```

### 2. Install Required Packages
Run these commands in the project directory:
or just install the packages from the nuget packagage manager or whatever it was called
```bash
dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

### 3. Database Setup
Configure Connection String
Open appsettings.json and verify/update the connection string:
```bash
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=OnlineLibrary;Trusted_Connection=True;"
}
```
Run Migrations
```bash
dotnet ef migrations add InitialCreate --project OnlineLibrary
```
Update Database
```bash
dotnet ef database update --project OnlineLibrary
```

# And you should be ready to go?
