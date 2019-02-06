using ColossalFramework.UI;
using UnityEngine;

namespace StreamIt
{
    public class UIUtils
    {
        public static UILabel CreateLabel(UIComponent parent, string name, string text)
        {
            UILabel label = parent.AddUIComponent<UILabel>();
            label.name = name;
            label.text = text;

            return label;
        }

        public static UIScrollablePanel CreateScrollablePanel(UIComponent parent, string name)
        {
            UIScrollablePanel scrollablePanel = parent.AddUIComponent<UIScrollablePanel>();
            scrollablePanel.name = name;

            return scrollablePanel;
        }

        public static UIDragHandle CreateDragHandle(UIComponent parent, string name, UIComponent target)
        {
            UIDragHandle dragHandle = parent.AddUIComponent<UIDragHandle>();
            dragHandle.name = name;
            dragHandle.target = target;

            return dragHandle;
        }
    }
}
