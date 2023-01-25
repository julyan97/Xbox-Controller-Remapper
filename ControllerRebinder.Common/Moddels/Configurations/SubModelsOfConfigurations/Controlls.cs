using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;

namespace ControllerRebinder.Common.Moddels.Configurations.SubModelsOfConfigurations
{
    public class Controlls
    {
        public VirtualKeyCode Up { get; set; }
        public VirtualKeyCode Down { get; set; }
        public VirtualKeyCode Left { get; set; }
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
