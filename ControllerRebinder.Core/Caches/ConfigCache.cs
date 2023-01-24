﻿using ControllerRebinder.Common.Moddels.Configurations;
using Newtonsoft.Json;
using System.IO;

namespace ControllerRebinder.Core.Caches
{
    public static class ConfigCache
    {
        public static Configurations Configurations { get; set; }

        public static void Init()
        {
            var configurations = File.ReadAllText("appsettings.json");
            Configurations = JsonConvert.DeserializeObject<Configurations>(configurations);
        }
    }
}
