using System;
using WindowsInput.Native;

namespace ControllerRebinder.Common.Enumerations
{
    public static class ControllsHelper
    {
        public static VirtualKeyCode StringToKey(string key)
        {
            VirtualKeyCode result = (VirtualKeyCode)Enum.Parse(typeof(VirtualKeyCode), key);
            return result;
        }
    }
}
