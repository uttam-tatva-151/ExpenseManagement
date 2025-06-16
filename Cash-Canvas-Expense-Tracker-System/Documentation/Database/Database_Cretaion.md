## **Features**
- **Database Models**:
  - Comprehensive models for entities such as `User`, `Transaction`, `Bill`, `Payment`, `Reminder`, `Budget`, `ErrorLog`, and `RefreshToken`.
- **AppDbContext**:
  - Centralized database context for managing database operations.
- **Database Schema**:
  - Automatically generated schema based on models using Entity Framework Core.
---

## **Steps to Create Database Schema**

### **1. Define Models**
The following models were created to represent the entities in the system:
- **User**:
  - Represents application users.
- **Transaction**:
  - Tracks income and expenses.
- **Bill**:
  - Represents recurring or one-time bills.
- **Payment**:
  - Tracks payments made for bills.
- **Reminder**:
  - Stores reminder details for bills.
- **Budget**:
  - Stores budget settings for categories.
- **ErrorLog**:
  - Captures application errors for debugging.
- **RefreshToken**:
  - Manages JWT refresh tokens.

Each model includes properties, relationships, and constraints to ensure data integrity.

---

### **2. Create AppDbContext**
The `AppDbContext` class was created to manage database operations. It includes `DbSet` properties for all models and is configured to use PostgreSQL as the database provider.

#### **AppDbContext Code Structure**:
```csharp
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Bill> Bills { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Reminder> Reminders { get; set; }
    public DbSet<Budget> Budgets { get; set; }
    public DbSet<ErrorLog> ErrorLogs { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
}
```
---
### **3. Configure Database Connection**

The database connection string was added to `appsettings.json` to connect the application to the PostgreSQL database.

---
### **4. Generate Migrations**
Entity Framework Core was used to generate migrations based on the defined models and `AppDbContext`.

#### **Command to Generate Migration**:
```
dotnet ef migrations add InitialCreate
```
This command creates a migration file in the `Migrations` folder, which includes the schema for all models.

---
### **5. Apply Migrations**

The generated migrations were applied to the PostgreSQL database to create the schema.

#### **Command to Apply Migration**:
```
dotnet ef database update
```
This command creates tables in the database based on the models defined in `AppDbContext`.
