-- Create the business events table 
CREATE TABLE BusinessEvent (
    EventId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    EntityType NVARCHAR(200) NOT NULL,
    EntityId NVARCHAR(200) NOT NULL,
    EventType NVARCHAR(200) NOT NULL,
    SchemaVersion INT NOT NULL,
    EventTimestamp DATETIMEOFFSET NOT NULL,
    CorrelationId NVARCHAR(200) NOT NULL,
    ActorId NVARCHAR(200) NOT NULL,
    ActorType NVARCHAR(200) NOT NULL,
    EntityData NVARCHAR(MAX) NOT NULL,
    INDEX IX_BusinessEvents_Lookup (EntityType, EntityId, EventTimestamp)
);

-- Create Schema version table 
CREATE TABLE SchemaVersions (
    EntityType VARCHAR(50) NOT NULL,
    Version INT NOT NULL,
    SchemaDefinition NVARCHAR(MAX) NOT NULL,
    CreatedDate DATETIMEOFFSET NOT NULL,
    PRIMARY KEY (EntityType, Version)
);