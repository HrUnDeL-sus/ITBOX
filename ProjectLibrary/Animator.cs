using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectLibrary
{
    [Serializable]
    public class Animator
    {
        public string SelectedAnimation;
        public Dictionary<string, Animation> Animations = new Dictionary<string, Animation>();
        internal float[] GetVerticesTexture()
        {
            return new float[] {
                   1,0,
                   1,1,
                   0,1,
                   0,0
            };
        }

    }
}
