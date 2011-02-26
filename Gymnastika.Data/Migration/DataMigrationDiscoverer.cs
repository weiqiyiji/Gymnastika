using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Gymnastika.Common.Extensions;

namespace Gymnastika.Data.Migration
{
    public class DataMigrationDiscoverer : IDataMigrationDiscoverer
    {
        private IDictionary<string, IDataMigration> _internalDataMigrationStore;

        public DataMigrationDiscoverer()
        {
            _internalDataMigrationStore = new Dictionary<string, IDataMigration>();
        }

        public IDataMigrationDiscoverer AddFromDirectory(string directoryPath)
        {
            return AddFromDirectory(directoryPath, x => true);
        }

        public IDataMigrationDiscoverer AddFromDirectory(string directoryPath, Func<string, bool> filter)
        {
            if(filter == null) throw new ArgumentNullException("filter");

            string[] assemblyFileNames = Directory.GetFiles(directoryPath, "", SearchOption.AllDirectories);
            foreach (var assemblyFileName in assemblyFileNames.Where(filter))
            {
                AddFromAssembly(assemblyFileName);
            }

            return this;
        }

        public IDataMigrationDiscoverer AddFromAssembly(Assembly assembly)
        {
            IEnumerable<IDataMigration> dataMigrations =
                assembly.GetExportedTypes()
                    .Where(t => t.GetInterface(typeof(IDataMigration).FullName) != null)
                    .Select(t => (IDataMigration)Activator.CreateInstance(t));
            dataMigrations.ForEach(m => Add(m));
            return this;
        }

        public IDataMigrationDiscoverer AddFromAssembly(string assemblyPath)
        {
            return AddFromAssembly(Assembly.LoadFrom(assemblyPath));
        }

        public IDataMigrationDiscoverer AddFromAssemblyOf(string typeInTargetAssembly)
        {
            return AddFromAssemblyOf(Type.GetType(typeInTargetAssembly));
        }

        public IDataMigrationDiscoverer AddFromAssemblyOf(Type type)
        {
            Assembly assembly = Assembly.GetAssembly(type);
            IEnumerable<IDataMigration> dataMigrations =
                assembly.GetExportedTypes()
                    .Where(t => t.GetInterface(typeof(IDataMigration).FullName) != null)
                    .Select(t => (IDataMigration)Activator.CreateInstance(t));
            dataMigrations.ForEach(m => Add(m));
            return this;
        }

        public IDataMigrationDiscoverer AddFromAssemblyOf<T>()
        {
            return AddFromAssemblyOf(typeof(T));
        }

        public IDataMigrationDiscoverer Add(IDataMigration dataMigration)
        {
            IDataMigration migration = null;
            if(!_internalDataMigrationStore.TryGetValue(dataMigration.Version, out migration))
            {
                _internalDataMigrationStore.Add(dataMigration.Version, dataMigration);
            }
            return this;
        }

        public IEnumerable<IDataMigration> DataMigrations
        {
            get { return _internalDataMigrationStore.Values; }
        }
    }
}
