namespace ControllerRebinder.Common.Moddels.Configurations.SubModelsOfConfigurations
{
    public class BaseJoyStick
    {
        private const int _ThresholdMultiplier = 100;
        private const int _AreaMultiplier = 10_000_000;

        private double staticArea;
        private double forwardDown;
        private double leftRight;

        public bool On { get; set; }
        public bool Log { get; set; }

        public double StaticArea { get => staticArea; set => staticArea = value * _AreaMultiplier; }
        public double ForwardDown { get => forwardDown; set => forwardDown = value * _AreaMultiplier; }
        public double LeftRight { get => leftRight; set => leftRight = value * _AreaMultiplier; }

        public int DeadZone { get; set; }
        public int MaxValController { get; set; }
        public int ThreshHoldAreaCal { get; set; }

        public Controlls Controlls { get; set; }
    }
}
