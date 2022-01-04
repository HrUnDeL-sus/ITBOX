using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ITBOX
{
  public sealed class ItBoxWindow:GameWindow
    {
      public  ItBoxWindow ():base(500,500)
        {
            UpdateFrame += ItBoxWindow_UpdateFrame;
            Load += ItBoxWindow_Load;
        }

        private void ItBoxWindow_Load(object sender, EventArgs e)
        {
            Map map = new Map("map1",new Camera(10));
            map.AddEntity(new TestEntity());
            map.AddEntity(new TestEntity());
            map.AddEntity(new TestEntity());
            map.AddEntity(new TestEntity());
            MapManager.LoadMap("map1");
        }

        private void ItBoxWindow_UpdateFrame(object sender, FrameEventArgs e)
        {
           MapManager.RenderingMap();
            SwapBuffers();
        }
    }
}
