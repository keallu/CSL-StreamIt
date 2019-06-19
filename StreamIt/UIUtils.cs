using ColossalFramework.UI;
using UnityEngine;

namespace StreamIt
{
    public class UIUtils
    {
        public static UIFont GetUIFont(string name)
        {
            UIFont[] fonts = Resources.FindObjectsOfTypeAll<UIFont>();

            foreach (UIFont font in fonts)
            {
                if (font.name.CompareTo(name) == 0)
                {
                    return font;
                }
            }

            return null;
        }

        public static UIPanel CreatePanel(string name)
        {
            UIPanel panel = UIView.GetAView().AddUIComponent(typeof(UIPanel)) as UIPanel;
            panel.name = name;

            return panel;
        }

        public static UIPanel CreatePanel(UIComponent parent, string name)
        {
            UIPanel panel = parent.AddUIComponent<UIPanel>();
            panel.name = name;

            return panel;
        }

        public static UIScrollablePanel CreateScrollablePanel(string name)
        {
            UIScrollablePanel scrollablePanel = UIView.GetAView().AddUIComponent(typeof(UIScrollablePanel)) as UIScrollablePanel;
            scrollablePanel.name = name;

            return scrollablePanel;
        }

        public static UIScrollablePanel CreateScrollablePanel(UIComponent parent, string name)
        {
            UIScrollablePanel scrollablePanel = parent.AddUIComponent<UIScrollablePanel>();
            scrollablePanel.name = name;

            return scrollablePanel;
        }



        public static UIDragHandle CreateDragHandle(UIComponent parent, string name)
        {
            UIDragHandle dragHandle = parent.AddUIComponent<UIDragHandle>();
            dragHandle.name = name;
            dragHandle.target = parent;

            return dragHandle;
        }

        public static UIDragHandle CreateDragHandle(UIComponent parent, string name, UIComponent target)
        {
            UIDragHandle dragHandle = parent.AddUIComponent<UIDragHandle>();
            dragHandle.name = name;
            dragHandle.target = target;

            return dragHandle;
        }

        public static UILabel CreateLabel(UIComponent parent, string name, string text)
        {
            UILabel label = parent.AddUIComponent<UILabel>();
            label.name = name;
            label.text = text;

            return label;
        }
    }
}
