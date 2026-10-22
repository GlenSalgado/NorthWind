-- Recreates the schema and seed data explicitly present in the NorthWind project.
-- Safe to rerun: creates missing databases/tables and inserts only missing seed IDs.
-- Does not recover runtime Orders, OrderDetails, DomainLogs, or Identity users.

SET NOCOUNT ON;
IF DB_ID(N'NorthWindDB') IS NULL CREATE DATABASE [NorthWindDB];
GO
USE [NorthWindDB];
GO

IF OBJECT_ID(N'dbo.Customers', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Customers
    (
        Id nchar(5) NOT NULL CONSTRAINT PK_Customers PRIMARY KEY,
        Name nvarchar(40) NOT NULL,
        CurrentBalance decimal(8,2) NOT NULL,
        Cedula nvarchar(10) NULL,
        Email nvarchar(100) NULL
    );
END;
IF COL_LENGTH(N'dbo.Customers', N'Cedula') IS NULL ALTER TABLE dbo.Customers ADD Cedula nvarchar(10) NULL;
IF COL_LENGTH(N'dbo.Customers', N'Email') IS NULL ALTER TABLE dbo.Customers ADD Email nvarchar(100) NULL;

IF OBJECT_ID(N'dbo.Products', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Products
    (
        Id int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Products PRIMARY KEY,
        Name nvarchar(40) NOT NULL,
        UnitsInStock smallint NOT NULL,
        UnitPrice decimal(8,2) NOT NULL
    );
END;

IF OBJECT_ID(N'dbo.Orders', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Orders
    (
        Id int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Orders PRIMARY KEY,
        CustomerId nchar(5) NOT NULL,
        ShipAddress nvarchar(60) NOT NULL,
        ShipCity nvarchar(15) NOT NULL,
        ShipCountry nvarchar(15) NOT NULL,
        ShipPostalCode nvarchar(10) NOT NULL,
        ShippingType int NOT NULL,
        DiscountType int NOT NULL,
        Discount float NOT NULL,
        OrderDate datetime2 NOT NULL,
        CONSTRAINT FK_Orders_Customers_CustomerId FOREIGN KEY (CustomerId) REFERENCES dbo.Customers(Id) ON DELETE CASCADE
    );
    CREATE INDEX IX_Orders_CustomerId ON dbo.Orders(CustomerId);
END;

IF OBJECT_ID(N'dbo.OrderDetails', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.OrderDetails
    (
        OrderId int NOT NULL,
        ProductId int NOT NULL,
        UnitPrice decimal(8,2) NOT NULL,
        Quantity smallint NOT NULL,
        CONSTRAINT PK_OrderDetails PRIMARY KEY (OrderId, ProductId),
        CONSTRAINT FK_OrderDetails_Orders_OrderId FOREIGN KEY (OrderId) REFERENCES dbo.Orders(Id) ON DELETE CASCADE,
        CONSTRAINT FK_OrderDetails_Products_ProductId FOREIGN KEY (ProductId) REFERENCES dbo.Products(Id) ON DELETE CASCADE
    );
    CREATE INDEX IX_OrderDetails_ProductId ON dbo.OrderDetails(ProductId);
END;

IF NOT EXISTS (SELECT 1 FROM dbo.Customers WHERE Id = N'ALFKI') INSERT dbo.Customers (Id,Name,CurrentBalance,Cedula,Email) VALUES (N'ALFKI',N'Alfreds Futterkiste',0,N'1851076057',N'alfreds@northwind.com.ec');
IF NOT EXISTS (SELECT 1 FROM dbo.Customers WHERE Id = N'ANATR') INSERT dbo.Customers (Id,Name,CurrentBalance,Cedula,Email) VALUES (N'ANATR',N'Ana Trujillo Emparedados y helados',0,N'1710034065',N'ana.trujillo@northwind.com.ec');
IF NOT EXISTS (SELECT 1 FROM dbo.Customers WHERE Id = N'ANTON') INSERT dbo.Customers (Id,Name,CurrentBalance,Cedula,Email) VALUES (N'ANTON',N'Antonio Moreno Taquería',100,N'0102030400',N'antonio.moreno@northwind.com.ec');
IF NOT EXISTS (SELECT 1 FROM dbo.Customers WHERE Id = N'AROUT') INSERT dbo.Customers (Id,Name,CurrentBalance,Cedula,Email) VALUES (N'AROUT',N'Around the Horn',50,N'1801234566',N'around.horn@northwind.com.ec');
IF NOT EXISTS (SELECT 1 FROM dbo.Customers WHERE Id = N'BERGS') INSERT dbo.Customers (Id,Name,CurrentBalance,Cedula,Email) VALUES (N'BERGS',N'Berglunds snabbköp',0,N'1802345676',N'berglunds@northwind.com.ec');
IF NOT EXISTS (SELECT 1 FROM dbo.Customers WHERE Id = N'BLAUS') INSERT dbo.Customers (Id,Name,CurrentBalance,Cedula,Email) VALUES (N'BLAUS',N'Blauer See Delikatessen',0,N'1803456787',N'blauer.see@northwind.com.ec');
IF NOT EXISTS (SELECT 1 FROM dbo.Customers WHERE Id = N'BLONP') INSERT dbo.Customers (Id,Name,CurrentBalance,Cedula,Email) VALUES (N'BLONP',N'Blondel père et fils',240,N'1804567897',N'blondel@northwind.com.ec');
IF NOT EXISTS (SELECT 1 FROM dbo.Customers WHERE Id = N'BOLID') INSERT dbo.Customers (Id,Name,CurrentBalance,Cedula,Email) VALUES (N'BOLID',N'Bólido Comidas preparadas',0,N'1805678909',N'bolido@northwind.com.ec');
IF NOT EXISTS (SELECT 1 FROM dbo.Customers WHERE Id = N'BONAP') INSERT dbo.Customers (Id,Name,CurrentBalance,Cedula,Email) VALUES (N'BONAP',N'Bon app''',0,N'1806789010',N'bonapp@northwind.com.ec');
IF NOT EXISTS (SELECT 1 FROM dbo.Customers WHERE Id = N'BOTTM') INSERT dbo.Customers (Id,Name,CurrentBalance,Cedula,Email) VALUES (N'BOTTM',N'Bottom-Dollar Markets',15,N'1807890122',N'bottom.dollar@northwind.com.ec');

SET IDENTITY_INSERT dbo.Products ON;
IF NOT EXISTS (SELECT 1 FROM dbo.Products WHERE Id = 1) INSERT dbo.Products (Id,Name,UnitPrice,UnitsInStock) VALUES (1,N'Chai',18.00,39);
IF NOT EXISTS (SELECT 1 FROM dbo.Products WHERE Id = 2) INSERT dbo.Products (Id,Name,UnitPrice,UnitsInStock) VALUES (2,N'Chang (Agotado)',19.00,0);
IF NOT EXISTS (SELECT 1 FROM dbo.Products WHERE Id = 3) INSERT dbo.Products (Id,Name,UnitPrice,UnitsInStock) VALUES (3,N'Aniseed Syrup',10.00,13);
IF NOT EXISTS (SELECT 1 FROM dbo.Products WHERE Id = 4) INSERT dbo.Products (Id,Name,UnitPrice,UnitsInStock) VALUES (4,N'Chef Anton''s Cajun Seasoning',22.00,53);
IF NOT EXISTS (SELECT 1 FROM dbo.Products WHERE Id = 5) INSERT dbo.Products (Id,Name,UnitPrice,UnitsInStock) VALUES (5,N'Chef Anton''s Gumbo Mix (Agotado)',21.35,0);
IF NOT EXISTS (SELECT 1 FROM dbo.Products WHERE Id = 6) INSERT dbo.Products (Id,Name,UnitPrice,UnitsInStock) VALUES (6,N'Grandma''s Boysenberry Spread',25.00,120);
IF NOT EXISTS (SELECT 1 FROM dbo.Products WHERE Id = 7) INSERT dbo.Products (Id,Name,UnitPrice,UnitsInStock) VALUES (7,N'Uncle Bob''s Organic Dried Pears',30.00,15);
IF NOT EXISTS (SELECT 1 FROM dbo.Products WHERE Id = 8) INSERT dbo.Products (Id,Name,UnitPrice,UnitsInStock) VALUES (8,N'Northwoods Cranberry Sauce',40.00,6);
IF NOT EXISTS (SELECT 1 FROM dbo.Products WHERE Id = 9) INSERT dbo.Products (Id,Name,UnitPrice,UnitsInStock) VALUES (9,N'Mishi Kobe Niku',97.00,29);
IF NOT EXISTS (SELECT 1 FROM dbo.Products WHERE Id = 10) INSERT dbo.Products (Id,Name,UnitPrice,UnitsInStock) VALUES (10,N'Ikura',31.00,31);
IF NOT EXISTS (SELECT 1 FROM dbo.Products WHERE Id = 11) INSERT dbo.Products (Id,Name,UnitPrice,UnitsInStock) VALUES (11,N'Queso Cabrales',21.00,22);
IF NOT EXISTS (SELECT 1 FROM dbo.Products WHERE Id = 12) INSERT dbo.Products (Id,Name,UnitPrice,UnitsInStock) VALUES (12,N'Queso Manchego La Pastora',38.00,86);
IF NOT EXISTS (SELECT 1 FROM dbo.Products WHERE Id = 13) INSERT dbo.Products (Id,Name,UnitPrice,UnitsInStock) VALUES (13,N'Konbu',6.00,24);
IF NOT EXISTS (SELECT 1 FROM dbo.Products WHERE Id = 14) INSERT dbo.Products (Id,Name,UnitPrice,UnitsInStock) VALUES (14,N'Tofu',23.25,35);
IF NOT EXISTS (SELECT 1 FROM dbo.Products WHERE Id = 15) INSERT dbo.Products (Id,Name,UnitPrice,UnitsInStock) VALUES (15,N'Genen Shouyu',15.50,39);
SET IDENTITY_INSERT dbo.Products OFF;
GO

IF DB_ID(N'NorthWindLogsDB') IS NULL CREATE DATABASE [NorthWindLogsDB];
GO
USE [NorthWindLogsDB];
GO
IF OBJECT_ID(N'dbo.DomainLogs', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.DomainLogs
    (
        Id int IDENTITY(1,1) NOT NULL CONSTRAINT PK_DomainLogs PRIMARY KEY,
        CreatedDate datetime2 NOT NULL,
        Information nvarchar(max) NOT NULL,
        UserName nvarchar(max) NOT NULL CONSTRAINT DF_DomainLogs_UserName DEFAULT (N'')
    );
END;
GO

-- The project config references this database, but its membership schema is provided by an external package.
-- Create the database here; do not invent AspNet tables or user records.
IF DB_ID(N'NorthWindUsersDB') IS NULL CREATE DATABASE [NorthWindUsersDB];
GO

-- ASP.NET Identity schema taken from the NorthWind.Membership.Backend.AspNetIdentity
-- package's embedded InitialCreate migration. Creates missing tables only.
USE [NorthWindUsersDB];
GO
IF OBJECT_ID(N'dbo.AspNetRoles', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.AspNetRoles
    (
        Id nvarchar(450) NOT NULL CONSTRAINT PK_AspNetRoles PRIMARY KEY,
        Name nvarchar(256) NULL,
        NormalizedName nvarchar(256) NULL,
        ConcurrencyStamp nvarchar(max) NULL
    );
END;
IF OBJECT_ID(N'dbo.AspNetUsers', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.AspNetUsers
    (
        Id nvarchar(450) NOT NULL CONSTRAINT PK_AspNetUsers PRIMARY KEY,
        FirstName nvarchar(max) NOT NULL,
        LastName nvarchar(max) NOT NULL,
        UserName nvarchar(256) NULL,
        NormalizedUserName nvarchar(256) NULL,
        Email nvarchar(256) NULL,
        NormalizedEmail nvarchar(256) NULL,
        EmailConfirmed bit NOT NULL,
        PasswordHash nvarchar(max) NULL,
        SecurityStamp nvarchar(max) NULL,
        ConcurrencyStamp nvarchar(max) NULL,
        PhoneNumber nvarchar(max) NULL,
        PhoneNumberConfirmed bit NOT NULL,
        TwoFactorEnabled bit NOT NULL,
        LockoutEnd datetimeoffset NULL,
        LockoutEnabled bit NOT NULL,
        AccessFailedCount int NOT NULL
    );
END;
IF OBJECT_ID(N'dbo.AspNetRoleClaims', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.AspNetRoleClaims
    (
        Id int IDENTITY(1,1) NOT NULL CONSTRAINT PK_AspNetRoleClaims PRIMARY KEY,
        RoleId nvarchar(450) NOT NULL,
        ClaimType nvarchar(max) NULL,
        ClaimValue nvarchar(max) NULL,
        CONSTRAINT FK_AspNetRoleClaims_AspNetRoles_RoleId FOREIGN KEY (RoleId) REFERENCES dbo.AspNetRoles(Id) ON DELETE CASCADE
    );
END;
IF OBJECT_ID(N'dbo.AspNetUserClaims', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.AspNetUserClaims
    (
        Id int IDENTITY(1,1) NOT NULL CONSTRAINT PK_AspNetUserClaims PRIMARY KEY,
        UserId nvarchar(450) NOT NULL,
        ClaimType nvarchar(max) NULL,
        ClaimValue nvarchar(max) NULL,
        CONSTRAINT FK_AspNetUserClaims_AspNetUsers_UserId FOREIGN KEY (UserId) REFERENCES dbo.AspNetUsers(Id) ON DELETE CASCADE
    );
END;
IF OBJECT_ID(N'dbo.AspNetUserLogins', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.AspNetUserLogins
    (
        LoginProvider nvarchar(450) NOT NULL,
        ProviderKey nvarchar(450) NOT NULL,
        ProviderDisplayName nvarchar(max) NULL,
        UserId nvarchar(450) NOT NULL,
        CONSTRAINT PK_AspNetUserLogins PRIMARY KEY (LoginProvider, ProviderKey),
        CONSTRAINT FK_AspNetUserLogins_AspNetUsers_UserId FOREIGN KEY (UserId) REFERENCES dbo.AspNetUsers(Id) ON DELETE CASCADE
    );
END;
IF OBJECT_ID(N'dbo.AspNetUserRoles', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.AspNetUserRoles
    (
        UserId nvarchar(450) NOT NULL,
        RoleId nvarchar(450) NOT NULL,
        CONSTRAINT PK_AspNetUserRoles PRIMARY KEY (UserId, RoleId),
        CONSTRAINT FK_AspNetUserRoles_AspNetRoles_RoleId FOREIGN KEY (RoleId) REFERENCES dbo.AspNetRoles(Id) ON DELETE CASCADE,
        CONSTRAINT FK_AspNetUserRoles_AspNetUsers_UserId FOREIGN KEY (UserId) REFERENCES dbo.AspNetUsers(Id) ON DELETE CASCADE
    );
END;
IF OBJECT_ID(N'dbo.AspNetUserTokens', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.AspNetUserTokens
    (
        UserId nvarchar(450) NOT NULL,
        LoginProvider nvarchar(450) NOT NULL,
        Name nvarchar(450) NOT NULL,
        Value nvarchar(max) NULL,
        CONSTRAINT PK_AspNetUserTokens PRIMARY KEY (UserId, LoginProvider, Name),
        CONSTRAINT FK_AspNetUserTokens_AspNetUsers_UserId FOREIGN KEY (UserId) REFERENCES dbo.AspNetUsers(Id) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_AspNetRoleClaims_RoleId' AND object_id = OBJECT_ID(N'dbo.AspNetRoleClaims'))
    CREATE INDEX IX_AspNetRoleClaims_RoleId ON dbo.AspNetRoleClaims(RoleId);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'RoleNameIndex' AND object_id = OBJECT_ID(N'dbo.AspNetRoles'))
    CREATE UNIQUE INDEX RoleNameIndex ON dbo.AspNetRoles(NormalizedName) WHERE NormalizedName IS NOT NULL;
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_AspNetUserClaims_UserId' AND object_id = OBJECT_ID(N'dbo.AspNetUserClaims'))
    CREATE INDEX IX_AspNetUserClaims_UserId ON dbo.AspNetUserClaims(UserId);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_AspNetUserLogins_UserId' AND object_id = OBJECT_ID(N'dbo.AspNetUserLogins'))
    CREATE INDEX IX_AspNetUserLogins_UserId ON dbo.AspNetUserLogins(UserId);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_AspNetUserRoles_RoleId' AND object_id = OBJECT_ID(N'dbo.AspNetUserRoles'))
    CREATE INDEX IX_AspNetUserRoles_RoleId ON dbo.AspNetUserRoles(RoleId);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'EmailIndex' AND object_id = OBJECT_ID(N'dbo.AspNetUsers'))
    CREATE INDEX EmailIndex ON dbo.AspNetUsers(NormalizedEmail);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UserNameIndex' AND object_id = OBJECT_ID(N'dbo.AspNetUsers'))
    CREATE UNIQUE INDEX UserNameIndex ON dbo.AspNetUsers(NormalizedUserName) WHERE NormalizedUserName IS NOT NULL;
GO
