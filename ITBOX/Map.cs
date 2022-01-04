using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITBOX
{
    public sealed class Map
    {
        public readonly string Name;
        private Camera _mainCamera;
        private List<Entity> _entities = new List<Entity>();
        public Map(string name,Camera camera)
        {
            Name = name;
            _mainCamera = camera;
            MapManager.AddMap(this);
        }
        public void AddEntity(Entity entity)
        {
            _entities.Add(entity);
        }
        public void Rendering()
        {
            foreach (var entity in _entities)
            {
                entity.Rendering(_mainCamera.GetOrthoMatrix());
            }
        }
    }
}

