# property-management-coursework
## System Architecture

The project follows a layered architecture:

- Menu Layer: Handles user interaction through console menus
- Service Layer: Contains business logic (invoice generation, job handling)
- Repository Layer: Handles database interaction using SQL queries
- Model Layer: Represents system entities such as Tenant, Complaint and Invoice generation

This separation improves maintainability and clarity of the system.

## Key Features

- Tenant management (add, update, view)
- Complaint handling system
- Monthly invoice generation
- Custom invoice generation
- Input validation and error handling
- Unit testing using MSTest

## Technologies Used

- C#
- .NET Console Application
- Microsoft.Data.SqlClient
- MSTest for testing
