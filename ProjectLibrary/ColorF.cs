using System;

namespace ProjectLibrary
{
    [Serializable]
    public struct ColorF
    {
        public float R;
        public float G;
        public float B;
        public float A;
        public ColorF(float r, float g, float b, float a)
        {
            if (r > 1 || g > 1 || b > 1 || a > 1)
                throw new Exception("Max value 1");
            R = r;
            G = g;
            B = b;
            A = a;
        }
    }
}
