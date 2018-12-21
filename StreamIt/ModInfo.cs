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
        }
    }
}
