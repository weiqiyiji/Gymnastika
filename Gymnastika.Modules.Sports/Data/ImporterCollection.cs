using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Common.Extensions;

namespace Gymnastika.Modules.Sports.Data
{
    public interface IImporterCollection
    {
        IList<IDataImporter> GetImporters();
        void Add(IDataImporter importer);
        void Add(IEnumerable<IDataImporter> importers);
    }

    public class ImporterCollection : IImporterCollection
    {

        readonly IList<IDataImporter> _importers = new List<IDataImporter>();

        public ImporterCollection()
        {
            
        }

        public ImporterCollection(IEnumerable<IDataImporter> importers)
        {
            _importers.AddRange(importers);
        }

        #region IImporterCollection Members

        public IList<IDataImporter> GetImporters()
        {
            return _importers;
        }

        public void Add(IDataImporter importer)
        {
            _importers.Add(importer);
        }

        public void Add(IEnumerable<IDataImporter> importers)
        {
            _importers.AddRange(importers);
        }

        #endregion
    }

}
