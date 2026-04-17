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

## How does the software work

- This software is based on console output, there exists two different type of user to operate this software ,i.e. manager, tenant,
- Manager manages the listed flats starting from providing monthly invoices (custom extra charges included), tenant complaint handling module, etc.
- Tenant has access to raise a complaint, view their past complaints and check their complaint status, and can view their all type of invoices provided by the manager

## Manager menu

- This consists of different operations provided to the manager like adding/removing/viewing tenants, generate invoices for tenant, view open complaints by all tenants, change the complaint progress status so that it changes for the tenant perspective as well.

## Tenant menu

- This basically consists of options for tenant like view their invoices, raise a complaint, and view complaints or complaint's status.
