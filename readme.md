# EFCommenter

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

After running migrations, the summaries will appear as table and column comments in the database.

```csharp
/// <summary>
/// Represents a person entity.
/// </summary>
public class Person
{
    public int Id { get; set; }

    /// <summary>
    /// The full name of the person.
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

---

## Notes

* Make sure your EF Core project targets **.NET 8** or later.
* XML documentation file must be enabled for the summaries to be read.
* Tested with **PostgreSQL** and **SQL Server**.
