# Folder Structure Documentation

## Solution Overview
The `CashCanvasExpenseManagementHub.sln` solution is organized into a modular structure to ensure scalability, maintainability, and clear separation of concerns.

## Folder Structure

```plaintext
CashCanvasExpenseManagementHub.sln
│
├── CashCanvas.Core
│   ├── DTOs/
│   ├── Entities/
│   └── View Models/
├── CashCanvas.Data
│   ├── Interfaces/
│   ├── Migrations/
│   └── Repositories/
├── CashCanvas.Services
│   ├── BusinessLogic/
│   ├── ServiceInterfaces/
│   └── Utilities/
├── CashCanvas.Web
│   ├── Controllers/
│   ├── Views/
│   ├── wwwroot/
|   ├── Extensions/
│   └── Middleware/
└── CashCanvas.Common
    ├── Constants/
    ├── Mappers/
    └── SharedUtilities/
```

## Key Highlights

### **CashCanvas.Core**
- Contains core business models, interfaces, and utility functions.
- Acts as the foundation for the application logic.

### **CashCanvas.Data**
- Manages database entities, migrations, and repository patterns.
- Ensures data persistence and retrieval.

### **CashCanvas.Services**
- Implements business logic and service interfaces.
- Includes helper classes for reusable functionalities.

### **CashCanvas.Web**
- Handles the web application layer with controllers, views, and middleware.
- Includes static assets in the `wwwroot` folder.

### **CashCanvas.Common**
- Provides shared constants, extension methods, and utilities used across the solution.

## Advanced Markdown Features

### **Table Representation**
| Folder Name         | Description                                   |
|---------------------|-----------------------------------------------|
| `CashCanvas.Core`   | Core business logic and foundational models. |
| `CashCanvas.Data`   | Database management and repository patterns. |
| `CashCanvas.Services` | Business logic and reusable helpers.        |
| `CashCanvas.Web`    | Web application layer and static assets.     |
| `CashCanvas.Common` | Shared utilities and constants.              |

