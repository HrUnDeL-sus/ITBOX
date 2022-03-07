using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;

namespace ResourseLibrary
{
    [Serializable]
    public struct DescriptionAnimation
    {
        public readonly List<DescriptionResourseSprite> DescriptionSprites;
        public DescriptionAnimation(List<DescriptionResourseSprite> get)
        {
            DescriptionSprites = get;
        }
    }
}