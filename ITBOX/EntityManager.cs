using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
namespace ITBOX
{
  static  class EntityManager
    {
        private static List<Entity> _loadingEntities = new List<Entity>();
        
       
        public static void LoadEntitiesFromDirectory()
        {
            foreach (var path_for_entity in Directory.GetFiles(Directory.GetCurrentDirectory() + ConfigurationManager.AppSettings.Get("PathEntities")))
            {
                string name = new FileInfo(path_for_entity).Name.Replace(new FileInfo(path_for_entity).Extension, "");
                List<Script> scripts = new List<Script>();
                List<Component> components = new List<Component>();
                if (new FileInfo(path_for_entity).Extension != ".entity")
                    continue;
                
                _loadingEntities.Add(new Entity(
                    name
                    ));
            }
        }
        public static Entity FindEntity(string name)
        {
            return _loadingEntities.Find((q) => q.Name == name);
        }
    }
}
