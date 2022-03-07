using System;
using System.Collections.Generic;
using System.Text;

namespace HrundelFramework
{
    public class Animation
    {
        private readonly Sprite[] _sprites;
        private int _id;
        private int _time;
        internal Animation(Sprite[] sprites) { _sprites = sprites; }
        internal void ResetTimer()
        {
            _time = 0;
        }
        internal void Rendering(int fps)
        {
            _time += 1;
            if (_time >= fps)
            {
                _time = 0;
                _id = _id + 1 >= _sprites.Length ? 0 : _id + 1;
            }
            _sprites[_id].Rendering();
        }
    }
}
