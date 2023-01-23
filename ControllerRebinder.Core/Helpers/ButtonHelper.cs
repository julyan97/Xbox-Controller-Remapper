using ControllerRebinder.Core.Caches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            foreach(var ranges in QuadrantCache.Quadrants.Values)
            {
                foreach(var range in ranges)
                {
                    foreach(var button in range.Buttons)
                    {
                        await Task.Run(() => _inputSimulator.Keyboard.KeyUp(button));
                    }
                }
            }
        }

        public static async Task PressButtons(List<VirtualKeyCode> buttons)
        {
            foreach(var button in buttons)
            {
                await Task.Run(() => _inputSimulator.Keyboard.KeyDown(button));
            }
        }

        public static async Task ReleaseButtons(List<VirtualKeyCode> buttons)
        {
            foreach(var button in buttons)
            {
                await Task.Run(() => _inputSimulator.Keyboard.KeyUp(button));
            }
        }
    }
}
