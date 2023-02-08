using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerRebinder.Core.Helpers
{
    public static class ConsoleHelper
    {
        public static void ClearConsole()
        {
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = false;
            for(int y = 0; y < Console.WindowHeight; y++)
                Console.Write(new String(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = true;
        }
    }
}
