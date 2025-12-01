# EFCommenter

[![NuGet](https://img.shields.io/nuget/v/EFCommenter.svg)](https://www.nuget.org/packages/EFCommenter)
[![NuGet](https://img.shields.io/nuget/dt/EFCommenter.svg)](https://www.nuget.org/packages/EFCommenter)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)


All summaries of entities, properties, and enums will be added as comments on the corresponding database tables and columns.

---

## Features

* Automatically adds **table comments** from entity class summaries.
* Adds **column comments** from property summaries.
* Supports **enum types** as column comments.
* Works with **SQL Server** and **PostgreSQL**.
* Easy to integrate into your EF Core project using a single extension method.

---

## Installation

```bash
dotnet add package EFCommenter
```

---

## Usage

Enable XML documentation in your project by adding this to your `.csproj`:

```xml
<PropertyGroup>
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
</PropertyGroup>
```

Then, call the extension method in your `DbContext`:

```csharp
using EFCommenter;
using Microsoft.EntityFrameworkCore;

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.AddEntitiesComments();
}
```

---

## Example

Your entities and enums include some comments.

```csharp
/// <summary>
/// The class declered a person!!!
/// </summary>
public class Person
{
    public int Id { get; set; }

    /// <summary>
    /// The full name of the person!!!
    /// </summary>
    public string Name { get; set; } = "";

    public PersonType Type { get; set; }
}

public enum PersonType
{
    Admin,
    User,
    Guest
}
```

In the migration adding, the summaries will be automatically added as table and column comments in the created migration file, and they are ready to update the database comments.

```csharp
migrationBuilder.CreateTable(
    name: "People",
    columns: table => new
    {
        Id = table.Column<int>(type: "int", nullable: false)
            .Annotation("SqlServer:Identity", "1, 1"),
        Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "The full name of the person!!!"),
        Type = table.Column<int>(type: "int", nullable: false, comment: "0: Admin | \n1: User | \n2: Guest | ")
    },
    constraints: table =>
    {
        table.PrimaryKey("PK_People", x => x.Id);
    },
    comment: "The class declered a person!!!");
```
---

## Notes

* Make sure your EF Core project targets **.NET 8** or later.
* XML documentation file must be enabled for the summaries to be read.
* Tested with **PostgreSQL** and **SQL Server**.
