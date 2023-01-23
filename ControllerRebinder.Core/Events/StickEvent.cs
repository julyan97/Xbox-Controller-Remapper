﻿using ControllerRebinder.Core.Caches;
using ControllerRebinder.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using WindowsInput.Native;

namespace ControllerRebinder.Core.Events
{
    class ProgressEventArgs : EventArgs
    {
        public ProgressEventArgs(List<VirtualKeyCode> buttons)
        {
            Buttons = buttons;
        }

        public List<VirtualKeyCode> Buttons { get; private set; }
    }

    class FinishedEventArgs : EventArgs
    {
        public FinishedEventArgs(List<VirtualKeyCode> buttons)
        {
            Buttons = buttons;
        }

        public List<VirtualKeyCode> Buttons { get; private set; }
    }


    public class StickEvent
    {
        public static Quadrant _quadrant;
        public static double _currentXArea;
        public event EventHandler Progress;
        public event EventHandler Finished;


        protected virtual void OnFinished(List<VirtualKeyCode> buttons)
        {
            var handler = Finished;
            if(handler != null)
                handler(this, new FinishedEventArgs(buttons));
        }

        protected virtual void OnProgress(List<VirtualKeyCode> buttons)
        {
            var handler = Progress;
            if(handler != null)
                handler(this, new ProgressEventArgs(buttons));
        }

        public void Start()
        {
            var controller = new XboxControllerBinder();
            controller.BindStickToWASD();
            // Task.Run((Action)ActivateOrDeactivatedEevnt);
        }

    }

}
