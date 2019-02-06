namespace StreamIt
{
    [ConfigurationPath("StreamItConfig.xml")]
    public class ModConfig
    {
        public bool ConfigUpdated { get; set; }
        public float PositionX { get; set; } = 0.0f;
        public float PositionY { get; set; } = 0.0f;
        public bool Enabled { get; set; } = true;
        public string Prefix { get; set; } = string.Empty;
        public string Suffix { get; set; } = string.Empty;
        public float Width { get; set; } = 512.0f;
        public float TextScale { get; set; } = 1.0f;
        public float Speed { get; set; } = 20.0f;
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