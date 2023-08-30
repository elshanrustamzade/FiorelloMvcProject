using System;
using System.Collections.Generic;

namespace Fiorello.Models
{
    public class SliderInfo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Image { get; set; }

        public static implicit operator List<object>(SliderInfo v)
        {
            throw new NotImplementedException();
        }
    }
}
