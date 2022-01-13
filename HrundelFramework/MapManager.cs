using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrundelFramework
{
    public static class MapManager
    {
        private static Shader _shader = new Shader("shader.vert", "shader.frag");
        private static Map _currentMap = null;
        private static List<Map> _maps = new List<Map>();
      public static Map GetLoadingMap()
        {
                return _currentMap;
        }
        public static Shader GetShader()
        {
            return _shader;
        }
        public static void LoadMap(string name)
        {
            _currentMap = _maps.Find((t) => t.Name == name);
            foreach (var item in _currentMap.GetEntities())
            {
                item.Load();
            }
        }
        public static void RenderingMap()
        {
            if (_currentMap != null)
                _currentMap.Rendering();
            else
                Console.WriteLine("Пустая карта");
        }
        public static void ClearMap()
        {
            if (_currentMap != null)
                _currentMap.Clear();
            else
                Console.WriteLine("Пустая карта");
        }
        public static void AddMap(Map map)
        {
            _maps.Add(map);
        }
    }
}

