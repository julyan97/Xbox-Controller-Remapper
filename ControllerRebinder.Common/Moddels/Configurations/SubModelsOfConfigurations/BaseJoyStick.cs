﻿namespace ControllerRebinder.Common.Moddels.Configurations.SubModelsOfConfigurations
{
    public class BaseJoyStick
    {
        public bool On { get; set; }
        public bool Log { get; set; }

        public double StaticArea { get; set; }

        public double ForwardDown { get; set; }
        public double LeftRight { get; set; }
        public int DeadZone { get; set; }

        public int MaxValController { get; set; }
        public int ThreshHoldAreaCal { get; set; }

        public Controlls Controlls { get; set; }
    }
}
