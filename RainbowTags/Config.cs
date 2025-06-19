using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Interfaces;

namespace RainbowTags
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        public List<string> GroupWithRainbowTag { get; set; } = new List<string> { "rbpref" };
        public float Delay { get; set; } = 0.25f;
        public string[] Colors { get; set; } = 
        {
            "red",
            "orange",
            "yellow",
            "green",
            "blue_green",
            "magenta",
            "silver",
            "crimson"
        };
    }
}
