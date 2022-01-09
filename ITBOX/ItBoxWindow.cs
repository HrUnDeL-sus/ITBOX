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
            RenderFrame += ItBoxWindow_RenderFrame;
            Load += ItBoxWindow_Load;
            Resize += ItBoxWindow_Resize;
        }

        private void ItBoxWindow_RenderFrame(object sender, FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            MapManager.RenderingMap();
            SwapBuffers();
           
        }

        private void ItBoxWindow_Resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
        }

        private void ItBoxWindow_Load(object sender, EventArgs e)
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            Map map = new Map("map1",new Camera(80));
            for (int i = 0; i < 20; i++)
            {
                map.AddEntity(new TestEntity());
            }
            map.AddEntity(new PlayerEntity());
         
       
            MapManager.LoadMap("map1");
        }

        private void ItBoxWindow_UpdateFrame(object sender, FrameEventArgs e)
        {
            MapManager.ClearMap();
        }
    }
}
