using System;
using System.Reflection;
using static DalApi.DalConfig;

namespace DalApi
{
    public class DalFactory
    {
        public static IDal GetDal()
        {
            string dalType = DalConfig.DalName;
            DalImplementation dal = DalConfig.DalPackages[dalType];
            if (dal == null) throw new DalConfigException($"Package {dalType} is not found in packages list in dal-config.xml");

            try { Assembly.Load(dal.Package); }
            catch (Exception) { throw new DalConfigException("Failed to load the dal-config.xml file"); }

            Type type = Type.GetType($"{dal.NameSpace}.{dal.Class}, {dal.Package}");
            if (type == null) throw new DalConfigException($"Class {dal} was not found in the {dal}.dll");

            IDal dalInstance = (IDal)type.GetProperty("Instance",
                      BindingFlags.Public | BindingFlags.Static).GetValue(null);
            if (dal == null) throw new DalConfigException($"Class {dal} is not a singleton or wrong propertry name for Instance");

            return dalInstance;
        }
    }
}
