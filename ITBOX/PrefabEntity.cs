using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITBOX
{
  public  class PrefabEntity
    {
        public readonly Vector3 StartPosition;
        public readonly Vector3 StartRotation;
        public readonly Vector3 StartScale;
        private List<Component> _components = new List<Component>();
        private List<Script> _scripts = new List<Script>();
        public Entity _myEntity;
        private int _vertexBufferObject;
        private int _vertexArrayObject;
        private int _elementBufferObject;
        public void UpdateBoxCollider()
        {
            BoxCollider collider = GetComponent<BoxCollider>();
            Transform transform = GetComponent<Transform>();
            if (collider != null)
            {
                collider.UpdateBoxCollider(new Vector2(transform.Position.X, transform.Position.Y));
            }
        }
        public PrefabEntity(Vector3 _startPosition,
            Vector3 _startRotation,
            Vector3 _startScale,
            Entity entity, 
            List<Component> _startComponents,
            List<Script> _startScripts)
        {
            _components = _startComponents;
            _scripts = _startScripts;
            StartPosition = _startPosition;
            StartRotation = _startRotation;
            StartScale = _startScale;
            _myEntity = entity;
            _vertexBufferObject = GL.GenBuffer();
            _elementBufferObject = GL.GenBuffer();
            _vertexArrayObject = GL.GenVertexArray();
            ChangeTransformPosition();
            foreach (var item2 in _components)
            {
                item2.SetActiveMainPrefab(this);
            }
            foreach (var item in _scripts)
            {
                item.SetActiveMainPrefab(this);
                item.Start();
               
            }
        }
        public dynamic GetComponent<T>()
        {
            return _components.Find((t)=>(t is T));
        }
        public void AddScript(Script script)
        {
            _scripts.Add(script);   
        }
        public void RemoveScript(Script script)
        {
            _scripts.Remove(script);
        }
        private void ChangeTransformPosition()
        {
            (GetComponent<Transform>() as Transform).Position = StartPosition;
            (GetComponent<Transform>() as Transform).Rotate = StartRotation;
            (GetComponent<Transform>() as Transform).Scale = StartScale;
            if ((GetComponent<BoxCollider>() as BoxCollider) != null && (GetComponent<BoxCollider>() as BoxCollider).StateChangeSize == StateChangeSizeCollider.NotChange)
                (GetComponent<BoxCollider>() as BoxCollider).ChangeSize(new Vector2(StartScale.X, StartScale.Y));
        }
        public void AddComponent<T>() where T:new()
        {
            Component component =(new T() as Component);
            _components.Add(component);
            switch (component.GetType().Name)
            {
                case "Transform":
                    ChangeTransformPosition();
                    break;  
            }
        }
       private Matrix4 MathMatrixTransform()
        {
            Transform transform = (GetComponent<Transform>() as Transform);
            if (transform!=null)
            {
                return Matrix4.CreateTranslation(transform.Position) *
                Matrix4.CreateScale(transform.Scale);
            }
            return Matrix4.Zero;
        }
        private void ActivateMethodsInScripts()
        {
            foreach (var item in _scripts)
            {
                item.Update();
            }
        }
        public void Rendering()
        {
            ActivateMethodsInScripts();
           
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.AlphaTest);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(_myEntity.GetVertex().Length * Vector3.SizeInBytes), _myEntity.GetVertex(), BufferUsageHint.StaticDraw);
            GL.BindVertexArray(_vertexArrayObject);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(_myEntity.GetIndices().Length * sizeof(uint)), _myEntity.GetIndices(), BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, GL.GetAttribLocation(MapManager.GetShader(0).Handle, "texcoord"));
            GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr)(_myEntity.GetTexVertex().Length * Vector2.SizeInBytes), _myEntity.GetTexVertex(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(GL.GetAttribLocation(MapManager.GetShader(0).Handle, "texcoord"), 2, VertexAttribPointerType.Float, true, 0, 0);
            GL.EnableVertexAttribArray(GL.GetAttribLocation(MapManager.GetShader(0).Handle, "texcoord"));
            if(GetComponent<Animator>() is not null)
                (GetComponent<Animator>() as Animator).RunAnimation();

           Matrix4 result = MathMatrixTransform()*MapManager.GetOrtoInActiveScene();
            MapManager.GetShader(0).Use();

           
            GL.UniformMatrix4(GL.GetUniformLocation(MapManager.GetShader(0).Handle, "all"), false, ref result);
             
            GL.DrawElements(BeginMode.Triangles, _myEntity.GetIndices().Length, DrawElementsType.UnsignedInt, 0);
           GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(GL.GetAttribLocation(MapManager.GetShader(0).Handle, "texcoord"));
        }
    }
}
