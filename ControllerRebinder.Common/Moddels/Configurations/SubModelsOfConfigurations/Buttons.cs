using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput.Native;

namespace ControllerRebinder.Common.Moddels.Configurations.SubModelsOfConfigurations
{
    public class Buttons
    {
        public bool On { get; set; }
        public bool Log { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public VirtualKeyCode X { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public VirtualKeyCode Y { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public VirtualKeyCode A { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public VirtualKeyCode B { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public VirtualKeyCode DPadUp { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public VirtualKeyCode DPadDown { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public VirtualKeyCode DPadLeft { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public VirtualKeyCode DPadRight { get; set; 
        }
    }
}
