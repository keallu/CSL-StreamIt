namespace StreamIt
{
    [ConfigurationPath("StreamItConfig.xml")]
    public class ModConfig
    {
        public bool ConfigUpdated { get; set; }
        public bool Enabled { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public float Width { get; set; }
        public float TextScale { get; set; }

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