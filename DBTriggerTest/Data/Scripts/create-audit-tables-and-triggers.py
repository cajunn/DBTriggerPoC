import os
 
# ========== Your table list ==========
tables = [
    ("Products", "Id"),
    ("Customers", "Id"),
    ("Purchases", "Id"),
]
 
# ========== Create output folder ==========
output_dir = "output"
os.makedirs(output_dir, exist_ok=True)
 
output_path = os.path.join(output_dir, "audit_triggers.sql")
 
# ========== Generate SQL ==========
with open(output_path, 'w', encoding='utf-8') as f:
    for table_name, pk in tables:
        audit_table = f"{table_name}Audit"
        trigger_name = f"trg_{table_name}_Audit"
 
        sql = f"""
-- ==========================
-- Audit Table for {table_name}
-- ==========================
CREATE TABLE {audit_table} (
    AuditId INT IDENTITY(1,1) PRIMARY KEY,
    {pk} INT,
    Operation NVARCHAR(10),
    ModifiedBy NVARCHAR(128),
    ModifiedAt DATETIME DEFAULT GETDATE(),
    OldValues NVARCHAR(MAX),
    NewValues NVARCHAR(MAX)
);
GO
 
-- ==========================
-- Trigger for {table_name}
-- ==========================
CREATE OR ALTER TRIGGER {trigger_name}
ON {table_name}
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
 
    DECLARE @username NVARCHAR(128) = SUSER_SNAME();
    DECLARE @oldValues NVARCHAR(MAX);
    DECLARE @newValues NVARCHAR(MAX);
 
    -- UPDATE
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
    BEGIN
        SELECT @oldValues = (SELECT * FROM deleted FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
        SELECT @newValues = (SELECT * FROM inserted FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
        INSERT INTO {audit_table} ({pk}, Operation, ModifiedBy, ModifiedAt, OldValues, NewValues)
        SELECT d.{pk}, 'UPDATE', @username, GETDATE(), @oldValues, @newValues
        FROM inserted d;
    END
    -- INSERT
    ELSE IF EXISTS (SELECT * FROM inserted)
    BEGIN
        SELECT @newValues = (SELECT * FROM inserted FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
        INSERT INTO {audit_table} ({pk}, Operation, ModifiedBy, ModifiedAt, NewValues)
        SELECT i.{pk}, 'INSERT', @username, GETDATE(), @newValues
        FROM inserted i;
    END
    -- DELETE
    ELSE IF EXISTS (SELECT * FROM deleted)
    BEGIN
        SELECT @oldValues = (SELECT * FROM deleted FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);
        INSERT INTO {audit_table} ({pk}, Operation, ModifiedBy, ModifiedAt, OldValues)
        SELECT d.{pk}, 'DELETE', @username, GETDATE(), @oldValues
        FROM deleted d;
    END
END;
GO
 
"""
        f.write(sql)
 
print(f"✅ SQL written to: {output_path}")