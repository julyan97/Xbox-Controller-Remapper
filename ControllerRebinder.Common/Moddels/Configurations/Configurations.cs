using ControllerRebinder.Common.Moddels.Configurations.SubModelsOfConfigurations;

namespace ControllerRebinder.Common.Moddels.Configurations
{
    public class Configurations
    {
        public int RefreshRate { get; set; }
        public BaseJoyStick LeftJoyStick { get; set; }
        public BaseJoyStick RightJoyStick { get; set; }
        public Buttons Buttons { get; set; }
    }
}
