using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
namespace ProjectLibrary
{
    [Serializable]
    public class ResourceSprite
    {
        public readonly byte[] ArraySprite;
        public readonly int Height;
        public readonly int Width;
        public readonly string Path;
        public readonly string Name;
        public ResourceSprite(string path,byte[] arraySprite,string name,int height,int width)
        {
            Path = path;
            ArraySprite = arraySprite;
            Name = name;
            Height = height;
            Width = width;
        }
    }
}
