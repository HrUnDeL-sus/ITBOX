using OpenTK;
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
        public Camera GetCamera()
        {
            return _mainCamera;
        }
        public List<Entity> GetEntities()
        {
            return _entities;
        }
       public List<T> FindEntityTypeInRadius<T>(Vector2 position,float radius) where T:class
        {
            List<T> typeEntities = new List<T>();
            foreach (var entity in _entities)
            {
                if(entity is T && Math.Abs(position.LengthFast - entity.Position.LengthFast) < radius)
                {
                    typeEntities.Add(entity as T);
                }
            }
            return typeEntities;
        }
        public void AddEntity(Entity entity)
        {
            _entities.Add(entity);
        }
        public void Clear()
        {
            foreach (var entity in _entities)
            {
                entity.Clear();

            }
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

