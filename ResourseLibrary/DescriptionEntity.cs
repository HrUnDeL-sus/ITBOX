using OpenTK;
using OpenTK.Graphics;
using System;

namespace ResourseLibrary
{
    [Serializable]
    public struct DescriptionEntity
    {
        public readonly Color4 MyColor;
        public readonly string Name;
        public readonly DescriptionAnimator MyAnimator;

        public DescriptionEntity(Color4 color, string name,DescriptionAnimator get)
        {
            MyColor = color;
            Name = name;
            MyAnimator = get;
        }
    }
}