using System.Reflection;
using DBTriggerTest.Models;

namespace DBTriggerTest.Services
{
    public static class MetadataScanner
    {
        public static List<EntityMetadata> GetEntitiesFromNamespace(string @namespace)
        {
            var types = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t =>
                    t.IsClass &&
                    t.Namespace == @namespace &&
                    t.GetProperties().Any(p => p.CanRead && p.CanWrite && !p.PropertyType.Namespace.StartsWith("System.Collections")) &&
                    !t.Name.EndsWith("Metadata"))
                .ToList();

            var entities = new List<EntityMetadata>();

            foreach (var type in types)
            {
                var entity = new EntityMetadata
                {
                    EntityName = type.Name
                };

                foreach (var prop in type.GetProperties())
                {
                    entity.Properties.Add(new PropertyMetadata
                    {
                        PropertyName = prop.Name,
                        TypeName = prop.PropertyType.Name,
                        IsNullable = Nullable.GetUnderlyingType(prop.PropertyType) != null ||
                                     (!prop.PropertyType.IsValueType && prop.PropertyType != typeof(string)),
                        Attributes = string.Join(", ", prop.GetCustomAttributes().Select(a => a.GetType().Name)),
                        IsNavigation = prop.PropertyType.IsClass && prop.PropertyType != typeof(string)
                    });
                }

                entities.Add(entity);
            }

            return entities;
        }
    }
}