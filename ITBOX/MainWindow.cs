using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Configuration;
using System.Collections.Specialized;
using System.Diagnostics;

namespace ITBOX
{
    class MainWindow:GameWindow
    {
       
     
       
        public MainWindow() : base(500, 500)
        {
            Load += LoadWindow;
          
            RenderFrame += RenderFrameActiveScene;
            UpdateFrame += UpdateFrameActiveScene;
            KeyUp += MainWindow_KeyUp;
            Run(60, 60);
        }

        private void MainWindow_KeyUp(object sender, KeyboardKeyEventArgs e)
        {
            
        }

       

        private void LoadWindow(object sender, EventArgs e)
        {
            
         //   entities.Add(new Mob());
            EntityManager.LoadEntitiesFromDirectory();
            MapManager.LoadMapsFromDirectory();
            MapManager.SetCurrentMap(0);
            GL.ClearColor(OpenTK.Color.BlueViolet);

        }

        private void UpdateFrameActiveScene(object sender, FrameEventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            
          
        }
        private void RenderFrameActiveScene(object sender, FrameEventArgs e)
        {

            
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            MapManager.RenderingMap();
             
            SwapBuffers();
           




        }
    }
}
