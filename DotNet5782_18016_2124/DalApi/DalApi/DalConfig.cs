using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace DalApi
{
    class DalConfig
    {
        internal class DalImplementation
        {
            internal string Package;
            internal string NameSpace;
            internal string Class;
        }

        internal static string DalName;
        internal static Dictionary<string, DalImplementation> DalPackages;
        static DalConfig()
        {
            XElement dalConfig = XElement.Load(@"xml\dal-config.xml");
            DalName = dalConfig.Element("dal").Value;
            DalPackages = (from imp in dalConfig.Element("dal-packages").Elements()
                           let pkg = imp.Value
                           let ns = imp.Attribute("namespace")?.Value ?? pkg
                           let cls = imp.Attribute("class")?.Value ?? pkg
                           select new
                           {
                               imp.Name,
                               Value = new DalImplementation { Package = pkg, NameSpace = ns, Class = cls }
                           }
                          ).ToDictionary(p => "" + p.Name, p => p.Value);
        }
    }

    public class DalConfigException : Exception
    {
        public DalConfigException(string msg) : base(msg) { }
        public DalConfigException(string msg, Exception ex) : base(msg, ex) { }
    }
}
