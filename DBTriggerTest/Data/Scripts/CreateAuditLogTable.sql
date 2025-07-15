-- Create an Audit Log table.  Triggers live in the database, therefore the table and triggers will be defined in ../Data/Scripts. 
-- This is infrastructure - not business logic.  As a result, we want to seperate it to avoid cluttering the domain.  

CREATE TABLE ProductsAudit (
    AuditID INT IDENTITY(1,1) PRIMARY KEY,
    ProductId INT,
    Operation NVARCHAR(10),
    ModifiedBy NVARCHAR(128),
    ModifiedAt DATETIME,
    OldValues NVARCHAR(MAX),  -- Store old values as JSON or delimited string
    NewValues NVARCHAR(MAX)   -- Store new values as JSON or delimited string
    )