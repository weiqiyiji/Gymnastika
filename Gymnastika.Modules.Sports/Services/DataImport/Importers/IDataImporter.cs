using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Data;
using Gymnastika.Modules.Sports.Models;

namespace Gymnastika.Modules.Sports.DataImport.Importers
{
    public interface IDataImporter
    {
        bool NeedImport();
        void ImportData();
    }
}
