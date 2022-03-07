using OpenTK;
using OpenTK.Graphics;
using System;

namespace ResourseLibrary
{
    [Serializable]
    public struct DescriptionResourseSprite
    {
        public readonly byte[] ArraySprite;
        public readonly int Height;
        public readonly int Width;
        public DescriptionResourseSprite(byte[] arraySprite, int height, int width)
        {
            ArraySprite = arraySprite;
            Height = height;
            Width = width;
        }
    }
}