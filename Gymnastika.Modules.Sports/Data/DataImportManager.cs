using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Modules.Sports.Data
{
    public class DataImportManager : IDataImportManager
    {
        IImporterCollection _collection;
        public DataImportManager(IImporterCollection collection)
        {
            _collection = collection;
        }

        public void ImportData()
        {
            foreach (IDataImporter importer in _collection.GetImporters())
            {
                if (importer.NeedImport())
                {
                    importer.ImportData();
                }
            }
        }

    }
}
