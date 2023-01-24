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

        public void ReleaseAll(InputSimulator _inputSimulator)
        {
            _inputSimulator.Keyboard.KeyUp(Up);
            _inputSimulator.Keyboard.KeyUp(Down);
            _inputSimulator.Keyboard.KeyUp(Left);
            _inputSimulator.Keyboard.KeyUp(Right);
        }
    }
}
