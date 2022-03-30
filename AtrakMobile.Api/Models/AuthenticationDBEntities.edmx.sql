
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/10/2018 12:53:42
-- Generated from EDMX file: E:\CodeGarage\WebApi\WebApisTokenAuth\WebApisTokenAuth\Models\AuthenticationDBEntities.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [TokenBasedAuthenticationDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ApplicationUsers_UserRoles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ApplicationUsers] DROP CONSTRAINT [FK_ApplicationUsers_UserRoles];
GO
IF OBJECT_ID(N'[dbo].[FK_Employees_Designation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [FK_Employees_Designation];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[ApplicationUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ApplicationUsers];
GO
IF OBJECT_ID(N'[dbo].[Designation]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Designation];
GO
IF OBJECT_ID(N'[dbo].[Employees]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Employees];
GO
IF OBJECT_ID(N'[dbo].[UserRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserRoles];
GO 

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'ApplicationUsers'
CREATE TABLE [dbo].[ApplicationUsers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [EmailID] nvarchar(50)  NULL,
    [UserName] nvarchar(50)  NOT NULL,
    [Password] varbinary(50)  NOT NULL,
    [RoleId] int  NOT NULL
);
GO

-- Creating table 'Designations'
CREATE TABLE [dbo].[Designations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Employees'
CREATE TABLE [dbo].[Employees] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [EmloyeeName] nvarchar(50)  NOT NULL,
    [DesignationId] int  NOT NULL,
    [Address] nvarchar(150)  NOT NULL,
    [Department] nvarchar(50)  NULL,
    [Salary] decimal(19,4)  NULL
);
GO

-- Creating table 'UserRoles'
CREATE TABLE [dbo].[UserRoles] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [RoleName] nvarchar(50)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'ApplicationUsers'
ALTER TABLE [dbo].[ApplicationUsers]
ADD CONSTRAINT [PK_ApplicationUsers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Designations'
ALTER TABLE [dbo].[Designations]
ADD CONSTRAINT [PK_Designations]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Employees'
ALTER TABLE [dbo].[Employees]
ADD CONSTRAINT [PK_Employees]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [ID] in table 'UserRoles'
ALTER TABLE [dbo].[UserRoles]
ADD CONSTRAINT [PK_UserRoles]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [RoleId] in table 'ApplicationUsers'
ALTER TABLE [dbo].[ApplicationUsers]
ADD CONSTRAINT [FK_ApplicationUsers_UserRoles]
    FOREIGN KEY ([RoleId])
    REFERENCES [dbo].[UserRoles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApplicationUsers_UserRoles'
CREATE INDEX [IX_FK_ApplicationUsers_UserRoles]
ON [dbo].[ApplicationUsers]
    ([RoleId]);
GO

-- Creating foreign key on [DesignationId] in table 'Employees'
ALTER TABLE [dbo].[Employees]
ADD CONSTRAINT [FK_Employees_Designation]
    FOREIGN KEY ([DesignationId])
    REFERENCES [dbo].[Designations]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Employees_Designation'
CREATE INDEX [IX_FK_Employees_Designation]
ON [dbo].[Employees]
    ([DesignationId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------