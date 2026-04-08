# property-management-coursework

## Team Roles

- Krish (Leader): Invoice generation, structural damage handling, cleaning job handling, integration and bug fixing
- Aanchal (Secretary): Input validation helper support, documentation, coordination and meeting records
- Saiyam (Dev 1): Plumbing module, validation logic, partial database interaction
- Manav (Dev 2): Electric module, menu integration, data storage logic
- Justin (Tester): Unit testing, bug reporting, minor bug fixing and validation improvements

## Project Features

- Tenant management
- Complaint handling (plumbing, electric, structural, cleaning)
- Invoice generation (monthly and custom)
- Maintenance job tracking
- Input validation and system checks
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
