CREATE TRIGGER trg_Products_Audit
ON Products
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @username NVARCHAR(128) = SUSER_SNAME();  -- Get the current user
    DECLARE @oldValues NVARCHAR(MAX);
    DECLARE @newValues NVARCHAR(MAX);
    -- Handle inserts
    IF EXISTS (SELECT * FROM inserted)
    BEGIN
        SELECT @newValues = (
            SELECT *
            FROM inserted
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        );
        INSERT INTO ProductsAudit (ProductId, Operation, ModifiedBy, ModifiedAt, NewValues)
        SELECT
            i.Id,
            'INSERT',
            @username,
            GETDATE(),
            @newValues
        FROM inserted i;
    END
    -- Handle updates
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
    BEGIN
        SELECT @oldValues = (
            SELECT *
            FROM deleted
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        );
        SELECT @newValues = (
            SELECT *
            FROM inserted
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        );
        INSERT INTO ProductsAudit (ProductId, Operation, ModifiedBy, ModifiedAt, OldValues, NewValues)
        SELECT
            u.Id,
            'UPDATE',
            @username,
            GETDATE(),
            @oldValues,
            @newValues
        FROM inserted u
        JOIN deleted d ON u.Id = d.Id;
    END
    -- Handle deletes
    IF EXISTS (SELECT * FROM deleted)
    BEGIN
        SELECT @oldValues = (
            SELECT *
            FROM deleted
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        );
        INSERT INTO ProductsAudit (ProductId, Operation, ModifiedBy, ModifiedAt, OldValues)
        SELECT
            d.Id,
            'DELETE',
            @username,
            GETDATE(),
            @oldValues
        FROM deleted d;
    END
END;