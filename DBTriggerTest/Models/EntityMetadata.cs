namespace DBTriggerTest.Models
{
    public class EntityMetadata
    {
        public string EntityName { get; set; }
        public List<PropertyMetadata> Properties { get; set; } = new();
    }

    public class PropertyMetadata
    {
        public string PropertyName { get; set; }
        public string TypeName { get; set; }
        public bool IsNullable { get; set; }
        public string Attributes { get; set; }
        public bool IsNavigation { get; set; }
    }

    public class DbTableMetadata
    {
        public string TableName { get; set; }
        public List<DbColumnMetadata> Columns { get; set; } = new();
    }

    public class DbColumnMetadata
    {
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public bool IsNullable { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsForeignKey { get; set; }
        public string ForeignKeyTable { get; set; }
        public string ForeignKeyColumn { get; set; }
    }
}