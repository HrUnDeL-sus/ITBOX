using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
namespace ITBOX
{
  public static   class MapManager
    {
        private static List<Map> _loadingMaps = new List<Map>();
        private static Map _activeMap;
        private static List<ShaderProgram> _shader=new List<ShaderProgram>();
       
        private static Vector3 GenerateVector3FromString(string get,char split='/')
        {
            return new Vector3(
                            float.Parse(get.Split(split)[0].Replace('.',',')),
                            float.Parse(get.Split(split)[1].Replace('.', ',')),
                            float.Parse(get.Split(split)[2].Replace('.', ',')));
        }
        public static void SetCurrentMap<T>(T get)
        {
            if( get is string)
            {
                _activeMap = _loadingMaps.Find((q) => q.Name == (get as string));
            }else if(get is int?)
            {                                                       
                _activeMap = _loadingMaps.ElementAt((int)(get as int?));
            }
        }
       public static Matrix4 GetOrtoInActiveScene()
        {
            return _activeMap.ortho;   
        }
        public static void RenderingMap()
        {
            _activeMap.Rendering();
        }
        private static void InitializationShaders()
        {
            _shader.Add(new ShaderProgram("vs_tex.glsl", "fs_tex.glsl"));
        }
        public static ShaderProgram GetShader(int id)
        {
            return  _shader[id];
        }
        private static List<T> ReturnTheDesiredList<T>(string subLine) where T : class
        {
            List<T> list = new List<T>();
            if (subLine.Split('/').Length > 0)
            {
                foreach (var item in subLine.Split('/'))
                {
                    if ((Activator.CreateInstance(Type.GetType("ITBOX." + item)) as T) is not null)
                        list.Add((Activator.CreateInstance(Type.GetType("ITBOX." + item)) as T));
                }
            }
            else
            {
                if ((Activator.CreateInstance(Type.GetType("ITBOX." + subLine)) as T) is not null)
                    list.Add((Activator.CreateInstance(Type.GetType("ITBOX." + subLine[1])) as T));
            }

            return list;
        }
        private static List<T> GetLisFromEntity<T>(string name) where T:class
        {
            string[] lines = File.ReadAllLines(Directory.GetCurrentDirectory()+"/Entities/"+name+".entity");
            List<T> list = new List<T>();
            List<Component> components = new List<Component>();
            foreach (var line in lines)
            {
                string[] subLine = line.Split(' ');

                
                switch (subLine[0])
                {
                    case "Scripts":
                        if(list.Count == 0)
                        {
                            list = (ReturnTheDesiredList<Script>(subLine[1]) as List<T>);
                            list = list is null ? new List<T>() : list;
                        }
                        break;
                    case "Components":
                        if (list.Count == 0)
                        {
                            list = (ReturnTheDesiredList<Component>(subLine[1]) as List<T>);
                            list = list is null ? new List<T>() : list;
                        }
                            
                        break;
                    case "Animator":
                        if (list is null||list.Find((t) => t.GetType().Name == "Animator") is null)
                            break;
                        Animator animator = (list.Find((t) => t.GetType().Name == "Animator") as Animator);
                        if (subLine[1].Split('@').Length == 0)
                        {
                            animator.AddAnimation(subLine[1]);
                            continue;
                        }
                        foreach (var subSubLine in subLine[1].Split('@'))
                        {
                            animator.AddAnimation(subSubLine);
                        }
                        break;
                    case "BoxCollider":
                        if (list is null || list.Find((t) => t.GetType().Name == "Animator") is null)
                            break;
                        BoxCollider boxCollider = (list.Find((t) => t.GetType().Name == "BoxCollider") as BoxCollider);
                        if (subLine[0].Split('/').Length > 0)
                        {
                            Vector2 size = new Vector2(
                                float.Parse(subLine[1].Split('/')[0].Replace('.', ',')),
                                float.Parse(subLine[1].Split('/')[1].Replace('.', ','))
                                );
                            //size.X = size.X / (1.2f + size.X);
                            //size.Y = size.Y / (1.2f + size.Y);
                            boxCollider.ChangeSize(size);
                        }
                       
                        break;
                    default:
                        break;

                }
            }
            return list;
        }
        private static void LoadingPrefabsForAllMaps()
        {
           
            foreach (var path_for_entity in Directory.GetFiles(Directory.GetCurrentDirectory() + ConfigurationManager.AppSettings.Get("PathMaps")))
            {
                if (new FileInfo(path_for_entity).Extension != ".map")
                    continue;
                List<PrefabEntity> sendPrefabEntities = new List<PrefabEntity>();
                int _size = default;
                int _size2 = default;
                string nameMap = new FileInfo(path_for_entity).Name.Replace(new FileInfo(path_for_entity).Extension, "");
                string[] lines = File.ReadAllLines(path_for_entity);
              
                foreach (var line in lines)
                {
                    List<Script> scripts = new List<Script>();
                    if (line.StartsWith("pref "))
                    {
                        string[] subLineArray = line.Substring(5).Split(' ');
                        Entity entity = EntityManager.FindEntity(subLineArray[0]);
                        List<Component> sendComponents=new List<Component>();
                        List<Script> sendScripts = new List<Script>();
                        
                       
                        PrefabEntity prefab = new PrefabEntity(
                                GenerateVector3FromString(subLineArray[1]),
                                GenerateVector3FromString(subLineArray[3]),
                                GenerateVector3FromString(subLineArray[2]),
                                entity,
                              GetLisFromEntity<Component>(subLineArray[0]),
                              GetLisFromEntity<Script>(subLineArray[0])

                            );
                        sendPrefabEntities.Add(prefab);
                    }
                    else if (line.StartsWith("s "))
                    {
                        _size = int.Parse(line.Substring(2).Split(' ')[0]);
                        _size2 = int.Parse(line.Substring(2).Split(' ')[1]);
                    }
                   
                }
                _loadingMaps.Add(new Map(sendPrefabEntities, nameMap, _size, _size2));
            }
        }
        public static void LoadMapsFromDirectory()
        {
            InitializationShaders();
            LoadingPrefabsForAllMaps();
        }
        private class Map
        {
            private List<PrefabEntity> _entitiesOnScene = new List<PrefabEntity>();
            public readonly string Name;
            public readonly Matrix4 ortho;
            public Map(List<PrefabEntity> prefabEntities, string name,int _size,int _size2)
            {
                _entitiesOnScene = prefabEntities;
                Name = name;
                ortho = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), 1, 0.1f, 100.0f);
            }
            public void Rendering()
            {
                foreach (var prefabEntity in _entitiesOnScene)
                {
                    prefabEntity.UpdateBoxCollider();
                    prefabEntity.Rendering();
                   
                }
            }
            
        }
    }
}
