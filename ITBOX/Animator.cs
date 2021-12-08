using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace ITBOX
{
   public sealed class Animator:Component
    {
        private List<Animation> _animations = new List<Animation>();
        private int _id=0;
        public Animator()
        {
        }
        public void SetActiveIDAnimation(int id)
        {
            _id = id;
        }
        public void AddAnimation(string path)
        {
            _animations.Add(new Animation(Directory.GetFiles(Directory.GetCurrentDirectory()+path)));
        }
        public void RunAnimation()
        {
            if(_animations.Count> _id)
            _animations[_id].Start();
        }
        private sealed class Animation
        {
            private List<Texture> textures = new List<Texture>();
            private int index = 0;
            public Animation(string[] path_textures)
            {
                foreach (var item in path_textures)
                {
                    textures.Add(new Texture(item));
                }
            }
            public void Start()
            {
                textures[index].ActivateTexture();
                index = index + 1 >= textures.Count ? 0 : index + 1;
            }
            private sealed class Texture
            {
                private string _pathTextures;
                private readonly int _texID;
                public Texture(string path)
                {
                    _pathTextures = path;
                    _texID = GL.GenTexture();
                    LoadTexture();
                }
                public void ActivateTexture()
                {
                    GL.ActiveTexture(TextureUnit.Texture0);

                    GL.BindTexture(TextureTarget.Texture2D, _texID);
                
                    GL.AlphaFunc(AlphaFunction.Gequal, 0.1f);
                }
                private void LoadTexture()
                {
                    Bitmap image = new Bitmap(_pathTextures);
                    GL.ActiveTexture(TextureUnit.Texture0);
                    GL.BindTexture(TextureTarget.Texture2D, _texID);
                    BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                        ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                    GL.TexImage2D(TextureTarget.Texture2D,
                            0,
                            PixelInternalFormat.Rgba,
                            image.Width,
                            image.Height,
                            0,
                           OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                            PixelType.UnsignedByte,
                            data.Scan0);

                    image.UnlockBits(data);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                    GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
                }
            }
        }
       
    }
}
