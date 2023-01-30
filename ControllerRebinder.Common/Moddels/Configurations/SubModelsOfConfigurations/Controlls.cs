using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;

namespace ControllerRebinder.Common.Moddels.Configurations.SubModelsOfConfigurations
{
    public class Controlls
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public VirtualKeyCode Up { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public VirtualKeyCode Down { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public VirtualKeyCode Left { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public VirtualKeyCode Right { get; set; }

        public async Task ReleaseAll(InputSimulator _inputSimulator)
        {
           await Task.Run(() =>
            {
                _inputSimulator.Keyboard.KeyUp(Up);
                _inputSimulator.Keyboard.KeyUp(Down);
                _inputSimulator.Keyboard.KeyUp(Left);
                _inputSimulator.Keyboard.KeyUp(Right);
            });

        }
    }
}
