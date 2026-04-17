# 🗄️ Property Management Database Setup

This document contains the full SQL script used to create and initialise the database for the Property Management System.


## 🧾 FULL DATABASE SCRIPT

```sql

-- =============================================
-- DATABASE CREATION
-- =============================================
CREATE DATABASE PropertyManagementDB;
GO

USE PropertyManagementDB;
GO


-- =============================================
-- TABLE: FLATS
-- Stores fixed flat information (10 flats)
-- =============================================
CREATE TABLE Flats (
    FlatId INT PRIMARY KEY,
    Floor INT NOT NULL DEFAULT 0,
    Bedrooms INT NOT NULL,
    BaseRent DECIMAL(10,2) NOT NULL
);


-- =============================================
-- TABLE: TENANTS
-- Stores tenant details and flat assignment
-- =============================================
CREATE TABLE Tenants (
    TenantId INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    FlatId INT NOT NULL,
    MoveInDate DATE NOT NULL,

    CONSTRAINT FK_Tenants_Flats
    FOREIGN KEY (FlatId) REFERENCES Flats(FlatId)
);


-- =============================================
-- TABLE: COMPLAINTS
-- Stores tenant complaints (plumbing, electrical, etc.)
-- =============================================
CREATE TABLE Complaints (
    ComplaintId INT IDENTITY(1,1) PRIMARY KEY,
    TenantId INT NOT NULL,
    Category NVARCHAR(50) NOT NULL,
    Description NVARCHAR(255) NOT NULL,
    Status NVARCHAR(50) NOT NULL DEFAULT 'Open',
    CreatedAt DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_Complaints_Tenants
    FOREIGN KEY (TenantId) REFERENCES Tenants(TenantId)
);


-- =============================================
-- TABLE: MAINTENANCE JOBS
-- Stores jobs created from complaints with cost
-- =============================================
CREATE TABLE MaintenanceJobs (
    JobId INT IDENTITY(1,1) PRIMARY KEY,
    TenantId INT NOT NULL,
    FlatId INT NOT NULL,
    JobType NVARCHAR(50) NOT NULL,
    JobDate DATE NOT NULL,
    Cost DECIMAL(10,2) NOT NULL,
    Notes NVARCHAR(255),

    CONSTRAINT FK_Jobs_Tenants
    FOREIGN KEY (TenantId) REFERENCES Tenants(TenantId),

    CONSTRAINT FK_Jobs_Flats
    FOREIGN KEY (FlatId) REFERENCES Flats(FlatId)
);


-- =============================================
-- TABLE: INVOICES
-- Stores invoice header (rent + extras)
-- =============================================
CREATE TABLE Invoices (
    InvoiceId INT IDENTITY(1,1) PRIMARY KEY,
    TenantId INT NOT NULL,
    PeriodMonth INT NOT NULL,
    PeriodYear INT NOT NULL,
    BaseRent DECIMAL(10,2) NOT NULL,
    ExtrasTotal DECIMAL(10,2) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_Invoices_Tenants
    FOREIGN KEY (TenantId) REFERENCES Tenants(TenantId)
);


-- =============================================
-- TABLE: INVOICE LINES
-- Stores individual invoice items
-- =============================================
CREATE TABLE InvoiceLines (
    LineId INT IDENTITY(1,1) PRIMARY KEY,
    InvoiceId INT NOT NULL,
    Description NVARCHAR(255) NOT NULL,
    Amount DECIMAL(10,2) NOT NULL,
    Category NVARCHAR(50),

    CONSTRAINT FK_Lines_Invoices
    FOREIGN KEY (InvoiceId) REFERENCES Invoices(InvoiceId)
);


-- =============================================
-- SEED DATA: FLATS
-- Predefined 10 flats with rent values
-- =============================================
INSERT INTO Flats (FlatId, Floor, Bedrooms, BaseRent) VALUES
(1, 0, 1, 900),
(2, 0, 2, 1200),
(3, 0, 1, 900),
(4, 0, 2, 1200),
(5, 0, 1, 900),
(6, 0, 2, 1200),
(7, 0, 1, 900),
(8, 0, 2, 1200),
(9, 0, 1, 900),
(10, 0, 2, 1200);


-- =============================================
-- OPTIONAL CLEANUP SCRIPT
-- Used to reset database data if needed
-- =============================================
-- DELETE FROM InvoiceLines;
-- DELETE FROM Invoices;
-- DELETE FROM MaintenanceJobs;
-- DELETE FROM Complaints;
-- DELETE FROM Tenants;
