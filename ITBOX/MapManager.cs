using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITBOX
{
    public static class MapManager
    {
        private static Shader _shader = new Shader("shader.vert", "shader.frag");
        private static Map _currentMap = null;
        private static List<Map> _maps = new List<Map>();
      
        public static Shader GetShader()
        {
            return _shader;
        }
        public static void LoadMap(string name)
        {
            _currentMap = _maps.Find((t) => t.Name == name);
        }
        public static void RenderingMap()
        {
            if (_currentMap != null)
                _currentMap.Rendering();
            else
                Console.WriteLine("Пустая карта");
        }
        public static void AddMap(Map map)
        {
            _maps.Add(map);
        }
    }
}

