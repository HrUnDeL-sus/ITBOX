using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;

namespace ResourseLibrary
{
    [Serializable]
    public struct DescriptionAnimator
    {
        public readonly Dictionary<string, DescriptionAnimation> Animations;
      
        public DescriptionAnimator(Dictionary<string,DescriptionAnimation> get)
        {
            Animations = get;
        }
    }
}