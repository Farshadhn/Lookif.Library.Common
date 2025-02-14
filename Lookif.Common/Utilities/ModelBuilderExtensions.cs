using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Pluralize.NET;
using Lookif.Library.Common.Attributes.Database;

namespace Lookif.Library.Common.Utilities;

public static class ModelBuilderExtensions
{
    /// <summary>
    /// Makes it singular
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void AddSingularizingTableNameConvention(this ModelBuilder modelBuilder)
    {
        Pluralizer pluralizer = new Pluralizer();
        foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
        {
            string tableName = entityType.GetTableName();
            entityType.SetTableName(pluralizer.Singularize(tableName));
        }
    }

    /// <summary>
    /// Pluralizing table name like Post to Posts or Person to People
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void AddPluralizingTableNameConvention(this ModelBuilder modelBuilder)
    {
        Pluralizer pluralizer = new Pluralizer();
        foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
        {
            string tableName = entityType.GetTableName();
            entityType.SetTableName(pluralizer.Pluralize(tableName));
        }
    }

    /// <summary>
    /// Set NEWSEQUENTIALID() sql function for all columns named "Id"
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void AddSequentialGuidForIdConvention(this ModelBuilder modelBuilder)
    {
        modelBuilder.AddDefaultValueSqlConvention("Id", typeof(Guid), "NEWSEQUENTIALID()");
    }

    /// <summary>
    /// Set DefaultValueSql for specific property name and type
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <param name="propertyName">Name of property wants to set DefaultValueSql for</param>
    /// <param name="propertyType">Type of property wants to set DefaultValueSql for </param>
    /// <param name="defaultValueSql">DefaultValueSql like "NEWSEQUENTIALID()"</param>
    public static void AddDefaultValueSqlConvention(this ModelBuilder modelBuilder, string propertyName, Type propertyType, string defaultValueSql)
    {
        foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
        {
            IMutableProperty property = entityType.GetProperties().SingleOrDefault(p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
            if (property != null && property.ClrType == propertyType)
                property.SetDefaultValueSql(defaultValueSql);
        }
    }

    /// <summary>
    /// Set DeleteBehavior.Restrict by default for relations
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void AddRestrictDeleteBehaviorConvention(this ModelBuilder modelBuilder)
    { 

        IEnumerable<IMutableForeignKey> cascadeFKs = modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);
        foreach (IMutableForeignKey fk in cascadeFKs)
        {
            var deleteBehavior = GetDeleteBehavior(fk);
            fk.DeleteBehavior = deleteBehavior;
        }


        DeleteBehavior GetDeleteBehavior(IMutableForeignKey foreignKey)
        {
            // Check each property in the foreign key
            foreach (var property in foreignKey.Properties)
            {
                var propertyInfo = property.PropertyInfo;
                if (propertyInfo != null && propertyInfo.GetCustomAttribute<OnDelete>() != null)
                {
                    return propertyInfo.GetCustomAttribute<OnDelete>().deleteBehavior;
                }
            }

            // Or check the navigation property if necessary
            var navigation = foreignKey.PrincipalToDependent;
            if (navigation != null)
            {
                var navigationProperty = navigation.PropertyInfo;
                if (navigationProperty != null && navigationProperty.GetCustomAttribute<OnDelete>() != null)
                {
                    return navigationProperty.GetCustomAttribute<OnDelete>().deleteBehavior; 
                }
            }

            return DeleteBehavior.Restrict;
        }
    }

    /// <summary>
    /// Dynamicaly load all IEntityTypeConfiguration with Reflection
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <param name="assemblies">Assemblies contains Entities</param>
    public static void RegisterEntityTypeConfiguration(this ModelBuilder modelBuilder, params Assembly[] assemblies)
    {
        MethodInfo applyGenericMethod = typeof(ModelBuilder).GetMethods().First(m => m.Name == nameof(ModelBuilder.ApplyConfiguration));

        IEnumerable<Type> types = assemblies.SelectMany(a => a.GetExportedTypes())
            .Where(c => c.IsClass && !c.IsAbstract && c.IsPublic);

        foreach (Type type in types)
        {
            foreach (Type iface in type.GetInterfaces())
            {
                if (iface.IsConstructedGenericType && iface.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                {
                    MethodInfo applyConcreteMethod = applyGenericMethod.MakeGenericMethod(iface.GenericTypeArguments[0]);
                    applyConcreteMethod.Invoke(modelBuilder, new[] { Activator.CreateInstance(type) });
                }
            }
        }
    }

    /// <summary>
    /// Dynamically register all Entities that inherit from specific BaseType
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <param name="assemblies">Assemblies contains Entities</param>
    public static void RegisterAllEntities<TBaseType, TBaseTemporal>(this ModelBuilder modelBuilder, params Assembly[] assemblies)
    {
        IEnumerable<Type> types = assemblies.SelectMany(a => a.GetExportedTypes())
            .Where(c => c.IsClass && !c.IsAbstract && c.IsPublic && typeof(TBaseType).IsAssignableFrom(c));

        foreach (Type type in types)
        {
            if (typeof(TBaseTemporal).IsAssignableFrom(type))
                modelBuilder.Entity(type).ToTable(e => e.IsTemporal());
            else
                modelBuilder.Entity(type);
        }
    }
}
