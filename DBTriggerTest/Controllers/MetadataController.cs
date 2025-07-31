using Microsoft.AspNetCore.Mvc;
using DBTriggerTest.Services;
using DBTriggerTest.Models;
using DBTriggerTest.Data;

namespace DBTriggerTest.Controllers
{
    public class MetadataPageViewModel
    {
        public List<EntityMetadata> EntityMetadata { get; set; }
        public List<DbTableMetadata> DbSchema { get; set; }
    }

    public class MetadataController : Controller
    {
        private readonly AppDbContext _context;

        public MetadataController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var entities = MetadataScanner.GetEntitiesFromNamespace("DBTriggerTest.Models");

            // Custom order list
            var preferredOrder = new List<string> { "Customer", "Product", "Purchase" };

            // Sort: first show preferred entities in defined order, then everything else alphabetically
            entities = entities
                .OrderBy(e =>
                    preferredOrder.Contains(e.EntityName)
                        ? preferredOrder.IndexOf(e.EntityName)
                        : int.MaxValue)
                .ThenBy(e => e.EntityName)
                .ToList();

            var dbSchema = DbSchemaScanner.GetDatabaseSchema(_context);
            var vm = new MetadataPageViewModel
            {
                EntityMetadata = entities,
                DbSchema = dbSchema
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult ExportEntitiesAsCsv()
        {
            var entities = MetadataScanner.GetEntitiesFromNamespace("DBTriggerTest.Models");

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Entity,Property,Type,Nullable,Attributes,IsNavigation");

            foreach (var entity in entities)
            {
                foreach (var prop in entity.Properties)
                {
                    sb.AppendLine($"\"{entity.EntityName}\",\"{prop.PropertyName}\",\"{prop.TypeName}\",\"{prop.IsNullable}\",\"{prop.Attributes}\",\"{prop.IsNavigation}\"");
                }
            }

            var bytes = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
            return File(bytes, "text/csv", "EntityMetadata.csv");
        }

        [HttpGet]
        public IActionResult ExportDbSchemaAsCsv()
        {
            var schema = DbSchemaScanner.GetDatabaseSchema(_context);

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Table,Column,Type,Nullable,IsPrimaryKey,IsForeignKey,FKTable,FKColumn");

            foreach (var table in schema)
            {
                foreach (var column in table.Columns)
                {
                    sb.AppendLine($"\"{table.TableName}\",\"{column.ColumnName}\",\"{column.DataType}\",\"{column.IsNullable}\",\"{column.IsPrimaryKey}\",\"{column.IsForeignKey}\",\"{column.ForeignKeyTable}\",\"{column.ForeignKeyColumn}\"");
                }
            }

            var bytes = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
            return File(bytes, "text/csv", "DatabaseSchema.csv");
        }
    }
}