using ColossalFramework.UI;
using ICities;

namespace StreamIt
{
    public class ModInfo : IUserMod
    {
        public string Name => "Stream It!";
        public string Description => "Stream the mods currently enabled in-game.";

        public void OnSettingsUI(UIHelperBase helper)
        {
            UIHelperBase group;
            bool selected;
            string selectedText;
            float selectedValue;
            float result;

            group = helper.AddGroup(Name);

            selected = ModConfig.Instance.ModsPanelEnabled;
            group.AddCheckbox("Enabled", selected, sel =>
            {
                ModConfig.Instance.ModsPanelEnabled = sel;
                ModConfig.Instance.Save();
            });

            selectedText = ModConfig.Instance.ModsPanelPrefix;
            group.AddTextfield("Prefix", selectedText, sel =>
            {
                ModConfig.Instance.ModsPanelPrefix = sel;
                ModConfig.Instance.Save();
            });

            selectedText = ModConfig.Instance.ModsPanelSuffix;
            group.AddTextfield("Suffix", selectedText, sel =>
            {
                ModConfig.Instance.ModsPanelSuffix = sel;
                ModConfig.Instance.Save();
            });

            selectedValue = ModConfig.Instance.ModsPanelWidth;
            group.AddTextfield("Width (in pixels)", selectedValue.ToString(), sel =>
            {
                float.TryParse(sel, out result);
                ModConfig.Instance.ModsPanelWidth = result;
                ModConfig.Instance.Save();
            });

            selectedValue = ModConfig.Instance.ModsPanelTextScale;
            group.AddSlider("Text Scale", 0.5f, 2f, 0.1f, selectedValue, sel =>
            {
                ModConfig.Instance.ModsPanelTextScale = sel;
                ModConfig.Instance.Save();
            });

            selectedValue = ModConfig.Instance.ModsPanelSpeed;
            group.AddSlider("Speed", 5f, 50f, 0.5f, selectedValue, sel =>
            {
                ModConfig.Instance.ModsPanelSpeed = sel;
                ModConfig.Instance.Save();
            });

            group.AddButton("Reset position", () =>
            {
                ModConfig.Instance.ModsPanelPositionX = 0.0f;
                ModConfig.Instance.ModsPanelPositionY = 0.0f;
                ModConfig.Instance.Save();
            });

            group = helper.AddGroup("Graphics");

            selected = ModConfig.Instance.GraphicsPanelEnabled;
            group.AddCheckbox("Enabled", selected, sel =>
            {
                ModConfig.Instance.GraphicsPanelEnabled = sel;
                ModConfig.Instance.Save();
            });

            selectedValue = ModConfig.Instance.GraphicsPanelTextScale;
            group.AddSlider("Text Scale", 0.5f, 2f, 0.1f, selectedValue, sel =>
            {
                ModConfig.Instance.GraphicsPanelTextScale = sel;
                ModConfig.Instance.Save();
            });

            group.AddButton("Reset position", () =>
            {
                ModConfig.Instance.GraphicsPanelPositionX = 0.0f;
                ModConfig.Instance.GraphicsPanelPositionY = 0.0f;
                ModConfig.Instance.Save();
            });

            group = helper.AddGroup("Export to file");

            selectedText = ModConfig.Instance.FileName;
            group.AddTextfield("File name", selectedText, sel =>
            {
                ModConfig.Instance.FileName = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.IncludeTimestampInFileName;
            group.AddCheckbox("Include timestamp in file name", selected, sel =>
            {
                ModConfig.Instance.IncludeTimestampInFileName = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.IncludeDisabledMods;
            group.AddCheckbox("Include disabled mods", selected, sel =>
            {
                ModConfig.Instance.IncludeDisabledMods = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.IncludeBuiltinMods;
            group.AddCheckbox("Include builtin mods", selected, sel =>
            {
                ModConfig.Instance.IncludeBuiltinMods = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.IncludeLocaleMods;
            group.AddCheckbox("Include locale mods", selected, sel =>
            {
                ModConfig.Instance.IncludeLocaleMods = sel;
                ModConfig.Instance.Save();
            });

            group.AddButton("Export", () =>
            {
                StreamUtils.ExportToFile(ModConfig.Instance.FileName, ModConfig.Instance.IncludeTimestampInFileName, ModConfig.Instance.IncludeDisabledMods, ModConfig.Instance.IncludeBuiltinMods, ModConfig.Instance.IncludeLocaleMods);
            });
        }
    }
}
