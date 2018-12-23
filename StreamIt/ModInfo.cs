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

            selected = ModConfig.Instance.Enabled;
            group.AddCheckbox("Stream enabled", selected, sel =>
            {
                ModConfig.Instance.Enabled = sel;
                ModConfig.Instance.Save();
            });

            selectedText = ModConfig.Instance.Prefix ?? "";
            group.AddTextfield("Prefix", selectedText, sel =>
            {
                ModConfig.Instance.Prefix = sel;
                ModConfig.Instance.Save();
            });

            selectedText = ModConfig.Instance.Suffix ?? "";
            group.AddTextfield("Suffix", selectedText, sel =>
            {
                ModConfig.Instance.Suffix = sel;
                ModConfig.Instance.Save();
            });

            selectedValue = ModConfig.Instance.Width != 0f ? ModConfig.Instance.Width : UIView.GetAView().GetScreenResolution().x / 2f;

            group.AddTextfield("Width (in pixels)", selectedValue.ToString(), sel =>
            {
                float.TryParse(sel, out result);
                ModConfig.Instance.Width = result;
                ModConfig.Instance.Save();
            });

            selectedValue = ModConfig.Instance.TextScale != 0f ? ModConfig.Instance.TextScale : 1f;

            group.AddSlider("Text Scale", 0.5f, 2f, 0.1f, selectedValue, sel =>
            {
                ModConfig.Instance.TextScale = sel;
                ModConfig.Instance.Save();
            });

            selectedValue = ModConfig.Instance.Speed != 0f ? ModConfig.Instance.Speed : 20f;

            group.AddSlider("Speed", 5f, 50f, 0.5f, selectedValue, sel =>
            {
                ModConfig.Instance.Speed = sel;
                ModConfig.Instance.Save();
            });

            group = helper.AddGroup("Export to file");

            selectedText = ModConfig.Instance.FileName ?? "";
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
