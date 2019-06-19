namespace StreamIt
{
    [ConfigurationPath("StreamItConfig.xml")]
    public class ModConfig
    {
        public bool ConfigUpdated { get; set; }
        public bool ModsPanelEnabled { get; set; } = true;
        public float ModsPanelPositionX { get; set; } = 0.0f;
        public float ModsPanelPositionY { get; set; } = 0.0f;
        public string ModsPanelPrefix { get; set; } = string.Empty;
        public string ModsPanelSuffix { get; set; } = string.Empty;
        public float ModsPanelWidth { get; set; } = 512.0f;
        public float ModsPanelTextScale { get; set; } = 1.0f;
        public float ModsPanelSpeed { get; set; } = 20.0f;
        public bool GraphicsPanelEnabled { get; set; } = true;
        public float GraphicsPanelPositionX { get; set; } = 0.0f;
        public float GraphicsPanelPositionY { get; set; } = 0.0f;
        public float GraphicsPanelTextScale { get; set; } = 1.0f;
        public string FileName { get; set; } = "Mods";
        public bool IncludeTimestampInFileName { get; set; }
        public bool IncludeDisabledMods { get; set; }
        public bool IncludeBuiltinMods { get; set; }
        public bool IncludeLocaleMods { get; set; }

        private static ModConfig instance;

        public static ModConfig Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Configuration<ModConfig>.Load();
                }

                return instance;
            }
        }

        public void Save()
        {
            Configuration<ModConfig>.Save();
            ConfigUpdated = true;
        }
    }
}