using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Gymnastika.Data.Configuration
{
    public class AutomappingConfigurationElementCollection : ConfigurationElementCollection
    {
        public AutomappingConfigurationElementCollection()
        { }

        public AutomappingConfigurationElementCollection(AutomappingConfigurationElement[] automappings)
        { 
            if(automappings == null) throw new ArgumentNullException("automappings");

            foreach(var automapping in automappings)
            {
                BaseAdd(automapping);
            }
        }

        public void Add(AutomappingConfigurationElement mapping)
        {
            BaseAdd(mapping);
        }

        public AutomappingConfigurationElement this[int index]
        {
            get { return (AutomappingConfigurationElement)BaseGet(index); }
        }

        public IList<AutomappingConfigurationElement> FindAll(Predicate<AutomappingConfigurationElement> match)
        {
            if (match == null) throw new System.ArgumentNullException("match");

            IList<AutomappingConfigurationElement> found = new List<AutomappingConfigurationElement>();
            foreach (AutomappingConfigurationElement AutomappingElement in this)
            {
                if (match(AutomappingElement))
                {
                    found.Add(AutomappingElement);
                }
            }
            return found;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override string ElementName
        {
            get { return "assembly"; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new AutomappingConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AutomappingConfigurationElement)element).AssemblyName;
        }
    }
}
