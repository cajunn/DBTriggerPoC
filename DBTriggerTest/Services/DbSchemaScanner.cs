using DBTriggerTest.Data;
using DBTriggerTest.Models;
using Microsoft.EntityFrameworkCore;

namespace DBTriggerTest.Services
{
    public class DbSchemaScanner
    {
        public static List<DbTableMetadata> GetDatabaseSchema(AppDbContext context)
        {
            var tables = new List<DbTableMetadata>();
            var model = context.Model;

            foreach (var entityType in model.GetEntityTypes())
            {
                var table = new DbTableMetadata
                {
                    TableName = entityType.GetTableName()
                };

                var primaryKeys = entityType.FindPrimaryKey()?.Properties
                                  .Select(p => p.Name)
                                  .ToHashSet() ?? new HashSet<string>();

                var foreignKeys = entityType.GetForeignKeys()
                    .SelectMany(fk => fk.Properties.Select(p => new
                    {
                        ColumnName = p.Name,
                        TargetTable = fk.PrincipalEntityType.GetTableName(),
                        TargetColumn = fk.PrincipalKey.Properties.First().GetColumnName()
                    }))
                    .ToDictionary(x => x.ColumnName, x => (x.TargetTable, x.TargetColumn));

                foreach (var prop in entityType.GetProperties())
                {
                    var columnName = prop.GetColumnName();
                    var column = new DbColumnMetadata
                    {
                        ColumnName = columnName,
                        DataType = prop.GetColumnType(),
                        IsNullable = prop.IsColumnNullable(),
                        IsPrimaryKey = primaryKeys.Contains(prop.Name),
                        IsForeignKey = foreignKeys.ContainsKey(prop.Name)
                    };

                    if (column.IsForeignKey)
                    {
                        column.ForeignKeyTable = foreignKeys[prop.Name].TargetTable;
                        column.ForeignKeyColumn = foreignKeys[prop.Name].TargetColumn;
                    }

                    table.Columns.Add(column);
                }

                tables.Add(table);
            }

            return tables;
        }
    }
}
