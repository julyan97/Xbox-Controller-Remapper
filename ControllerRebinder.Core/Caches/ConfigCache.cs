using ControllerRebinder.Common.Moddels.Configurations;
using Newtonsoft.Json;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ControllerRebinder.Core.Caches
{
    public static class ConfigCache
    {
        private const string PATH = "Configurations.json";
        public static Configurations Configurations { get; set; }

        public static void Init()
        {
            var configurations = File.ReadAllText(PATH);
            Configurations = JsonConvert.DeserializeObject<Configurations>(configurations);

            // var test = JsonConvert.SerializeObject(Configurations, Formatting.Indented);
            // File.WriteAllText("./test", test);
        }

        public static async Task Refresh()
        {
            var configurations = File.ReadAllText(PATH);
            var tempConf = JsonConvert.DeserializeObject<Configurations>(configurations);

            Configurations.LeftJoyStick = tempConf.LeftJoyStick;
            Configurations.RightJoyStick = tempConf.RightJoyStick;
            Configurations.Buttons = tempConf.Buttons;  
        }


    }
}
