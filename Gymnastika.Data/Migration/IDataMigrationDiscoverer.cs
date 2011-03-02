using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Gymnastika.Data.Migration
{
    public interface IDataMigrationDiscoverer
    {
        IDataMigrationDiscoverer AddFromDirectory(string directoryPath);
        IDataMigrationDiscoverer AddFromDirectory(string directoryPath, Func<string, bool> filter);
        IDataMigrationDiscoverer AddFromAssembly(Assembly assembly);
        IDataMigrationDiscoverer AddFromAssembly(string assemblyPath);
        IDataMigrationDiscoverer AddFromAssemblyOf(string typeInTargetAssembly);
        IDataMigrationDiscoverer AddFromAssemblyOf<T>();
        IDataMigrationDiscoverer AddFromAssemblyOf(Type type);
        IDataMigrationDiscoverer Add(IDataMigration dataMigration);
        IEnumerable<IDataMigration> DataMigrations { get; }
    }
}
