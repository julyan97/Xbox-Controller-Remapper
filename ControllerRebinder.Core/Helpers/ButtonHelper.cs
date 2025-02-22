using ControllerRebinder.Core.Caches;
using System.Collections.Generic;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;

namespace ControllerRebinder.Core.Helpers
{
    public static class ButtonHelper
    {
        public static InputSimulator _inputSimulator { get; set; } = new InputSimulator();


        public static async Task ReleaseKeysInQuadrantCache()
        {
            foreach (var ranges in QuadrantCache.Quadrants.Values)
            {
                foreach (var range in ranges)
                {
                    await Task.Run(() =>
                    {
                        foreach (var button in range.Buttons)
                        {
                            _inputSimulator.Keyboard.KeyUp(button);
                        }
                    });
                }
            }
        }

        public static async Task PressButtons(List<VirtualKeyCode> buttons)
        {
            await Task.Run(() =>
            {
                foreach (var button in buttons)
                {
                    _inputSimulator.Keyboard.KeyDown(button);
                }
            });
        }

        public static void ReleaseButtons(List<VirtualKeyCode> buttons)
        {
            foreach (var button in buttons)
            {
                _inputSimulator.Keyboard.KeyUp(button);
            }
        }
    }
}