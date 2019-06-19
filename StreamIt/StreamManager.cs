using ColossalFramework;
using ColossalFramework.Plugins;
using ColossalFramework.UI;
using ICities;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace StreamIt
{
    public class StreamManager : MonoBehaviour
    {
        private bool _initialized;

        private UIComponent _chirperPanel;
        private UIComponent _optionsPanel;
        private OptionsGraphicsPanel _optionsGraphicsPanel;
        private bool _wasChirperPanelEnabled;

        private UIPanel _modsPanel;
        private UIDragHandle _modsDragHandle;
        private UIScrollablePanel _modsScrollablePanel;
        private UILabel _modsLabel;
        private bool _isScrolling;

        private UIPanel _graphicsPanel;
        private UIDragHandle _graphicsDragHandle;
        private UIPanel _graphicsInnerPanel;
        private UILabel[] _graphicsTagLabels;
        private UILabel[] _graphicsValueLabels;

        public void Awake()
        {
            try
            {
                _chirperPanel = GameObject.Find("ChirperPanel").GetComponent<UIComponent>();
                _optionsPanel = GameObject.Find("(Library) OptionsPanel").GetComponent<UIComponent>();
                _optionsGraphicsPanel = _optionsPanel.Find("Graphics").GetComponent<OptionsGraphicsPanel>();
            }
            catch (Exception e)
            {
                Debug.Log("[Stream It!] StreamManager:Awake -> Exception: " + e.Message);
            }
        }

        public void Start()
        {
            try
            {
                _wasChirperPanelEnabled = _chirperPanel != null ? _chirperPanel.isEnabled : false;

                _graphicsTagLabels = new UILabel[5];
                _graphicsValueLabels = new UILabel[5];

                CreateUI();
            }
            catch (Exception e)
            {
                Debug.Log("[Stream It!] StreamManager:Start -> Exception: " + e.Message);
            }
        }

        public void Update()
        {
            try
            {
                if (!_initialized || ModConfig.Instance.ConfigUpdated)
                {
                    UpdateUI();

                    if (ModConfig.Instance.ModsPanelEnabled)
                    {
                        _modsPanel.enabled = true;

                        _modsLabel.text = GenerateMarqueeText();

                        StartScrollingIfNeeded();
                    }
                    else
                    {
                        _modsPanel.enabled = false;
                    }

                    if (ModConfig.Instance.GraphicsPanelEnabled)
                    {
                        _graphicsPanel.enabled = true;
                    }
                    else
                    {
                        _graphicsPanel.enabled = false;
                    }

                    _initialized = true;
                    ModConfig.Instance.ConfigUpdated = false;
                }

                if (_chirperPanel.isEnabled != _wasChirperPanelEnabled)
                {
                    UpdateUI();

                    _wasChirperPanelEnabled = !_wasChirperPanelEnabled;
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Stream It!] StreamManager:Update -> Exception: " + e.Message);
            }
        }

        public void OnDestroy()
        {
            try
            {
                foreach (UILabel label in _graphicsValueLabels)
                {
                    Destroy(label);
                }
                foreach (UILabel label in _graphicsTagLabels)
                {
                    Destroy(label);
                }
                if (_graphicsInnerPanel != null)
                {
                    Destroy(_graphicsInnerPanel);
                }
                if (_graphicsDragHandle != null)
                {
                    Destroy(_graphicsDragHandle);
                }
                if (_graphicsPanel != null)
                {
                    Destroy(_graphicsPanel);
                }
                if (_modsLabel != null)
                {
                    Destroy(_modsLabel);
                }
                if (_modsDragHandle != null)
                {
                    Destroy(_modsDragHandle);
                }
                if (_modsScrollablePanel != null)
                {
                    Destroy(_modsScrollablePanel);
                }
                if (_modsPanel != null)
                {
                    Destroy(_modsPanel);
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Stream It!] StreamManager:OnDestroy -> Exception: " + e.Message);
            }
        }

        private void CreateUI()
        {
            try
            {
                _modsPanel = UIUtils.CreatePanel("StreamItModsPanel");
                _modsPanel.zOrder = 0;
                _modsPanel.isInteractive = false;

                _modsDragHandle = UIUtils.CreateDragHandle(_modsPanel, "ModsDragHandle");
                _modsDragHandle.eventMouseUp += (component, eventParam) =>
                {
                    ModConfig.Instance.ModsPanelPositionX = _modsPanel.absolutePosition.x;
                    ModConfig.Instance.ModsPanelPositionY = _modsPanel.absolutePosition.y;
                    ModConfig.Instance.Save();
                };

                _modsScrollablePanel = UIUtils.CreateScrollablePanel(_modsPanel, "MarqueeScrollablePanel");
                _modsScrollablePanel.zOrder = 0;
                _modsScrollablePanel.isInteractive = false;
                _modsScrollablePanel.clipChildren = true;

                _modsLabel = UIUtils.CreateLabel(_modsScrollablePanel, "MarqueeLabel", "No mods found.");
                _modsLabel.zOrder = 0;
                _modsLabel.isInteractive = false;

                _graphicsPanel = UIUtils.CreatePanel("StreamItGraphicsPanel");
                _graphicsPanel.zOrder = 0;
                _graphicsPanel.isInteractive = false;

                _graphicsDragHandle = UIUtils.CreateDragHandle(_graphicsPanel, "GraphicsDragHandle");
                _graphicsDragHandle.zOrder = 0;
                _graphicsDragHandle.eventMouseUp += (component, eventParam) =>
                {
                    ModConfig.Instance.GraphicsPanelPositionX = _graphicsPanel.relativePosition.x;
                    ModConfig.Instance.GraphicsPanelPositionY = _graphicsPanel.relativePosition.y;
                    ModConfig.Instance.Save();
                };

                _graphicsInnerPanel = UIUtils.CreatePanel(_graphicsPanel, "GraphicsInnerPanel");
                _graphicsInnerPanel.zOrder = 0;
                _graphicsInnerPanel.autoLayout = true;
                _graphicsInnerPanel.autoLayoutDirection = LayoutDirection.Vertical;
                _graphicsInnerPanel.autoLayoutStart = LayoutStart.TopLeft;
                _graphicsInnerPanel.isInteractive = false;

                for (int i = 0; i < 5; i++)
                {
                    _graphicsTagLabels[i] = UIUtils.CreateLabel(_graphicsInnerPanel, "GraphicsTagLabel" + i, "");
                    _graphicsTagLabels[i].isInteractive = false;

                    _graphicsValueLabels[i] = UIUtils.CreateLabel(_graphicsInnerPanel, "GraphicsValueLabel" + i, "");
                    _graphicsValueLabels[i].font = UIUtils.GetUIFont("OpenSans-Regular");
                    _graphicsValueLabels[i].padding = new RectOffset(0, 0, 0, 10);
                    _graphicsValueLabels[i].isInteractive = false;
                }

                _optionsPanel.eventVisibilityChanged += (UIComponent component, bool value) =>
                {
                    UpdateGraphics();
                };
            }
            catch (Exception e)
            {
                Debug.Log("[Stream It!] StreamManager:CreateUI -> Exception: " + e.Message);
            }
        }

        private void UpdateUI()
        {
            try
            {
                if (ModConfig.Instance.ModsPanelPositionX == 0f && ModConfig.Instance.ModsPanelPositionY == 0f)
                {
                    float topOffset;

                    if (_chirperPanel != null && _chirperPanel.isEnabled)
                    {
                        topOffset = 114f;
                    }
                    else
                    {
                        topOffset = 44f;
                    }

                    ModConfig.Instance.ModsPanelPositionX = UIView.GetAView().GetScreenResolution().x / 2f - _modsScrollablePanel.width / 2f;
                    ModConfig.Instance.ModsPanelPositionY = 0f + topOffset;
                }

                if (ModConfig.Instance.GraphicsPanelPositionX == 0f && ModConfig.Instance.GraphicsPanelPositionY == 0f)
                {
                    ModConfig.Instance.GraphicsPanelPositionX = 20f;
                    ModConfig.Instance.GraphicsPanelPositionY = 80f;
                }

                _modsPanel.size = new Vector3(ModConfig.Instance.ModsPanelWidth, 44f);
                _modsPanel.absolutePosition = new Vector3(ModConfig.Instance.ModsPanelPositionX, ModConfig.Instance.ModsPanelPositionY);

                _modsDragHandle.size = _modsPanel.size;
                _modsDragHandle.relativePosition = new Vector3(0f, 0f);

                _modsScrollablePanel.size = _modsPanel.size;
                _modsScrollablePanel.relativePosition = new Vector3(0f, 0f);

                _modsLabel.textScale = ModConfig.Instance.ModsPanelTextScale;

                _graphicsPanel.size = new Vector2(200f, 200f);
                _graphicsPanel.absolutePosition = new Vector3(ModConfig.Instance.GraphicsPanelPositionX, ModConfig.Instance.GraphicsPanelPositionY);

                _graphicsDragHandle.size = _graphicsPanel.size;
                _graphicsDragHandle.relativePosition = new Vector3(0f, 0f);

                _graphicsInnerPanel.size = _graphicsPanel.size;
                _graphicsInnerPanel.relativePosition = new Vector3(0f, 0f);

                for (int i = 0; i < 5; i++)
                {
                    _graphicsTagLabels[i].textScale = ModConfig.Instance.GraphicsPanelTextScale;
                    _graphicsValueLabels[i].textScale = ModConfig.Instance.GraphicsPanelTextScale;

                    switch (i)
                    {
                        case 0:
                            _graphicsTagLabels[i].text = "Aspect Ratio";
                            break;
                        case 1:
                            _graphicsTagLabels[i].text = "Resolution";
                            break;
                        case 2:
                            _graphicsTagLabels[i].text = "V-Sync";
                            break;
                        case 3:
                            _graphicsTagLabels[i].text = "Color Correction";
                            break;
                        case 4:
                            _graphicsTagLabels[i].text = "Depth Of Field";
                            break;
                        default:
                            break;
                    }
                }

                UpdateGraphics();
            }
            catch (Exception e)
            {
                Debug.Log("[Stream It!] StreamManager:UpdateUI -> Exception: " + e.Message);
            }
        }

        private string GenerateMarqueeText()
        {
            try
            {
                StringBuilder marqueeText = new StringBuilder();
                List<string> modNames = new List<string>();

                marqueeText.Append(ModConfig.Instance.ModsPanelPrefix);
                marqueeText.Append("     ");

                foreach (PluginManager.PluginInfo pluginInfo in Singleton<PluginManager>.instance.GetPluginsInfo())
                {
                    if (pluginInfo.isEnabled)
                    {
                        IUserMod[] instances = pluginInfo.GetInstances<IUserMod>();

                        if (instances.Length == 1)
                        {
                            modNames.Add(instances[0].Name);
                        }
                    }
                }

                modNames.Sort();

                foreach (string modName in modNames)
                {
                    marqueeText.Append(modName);
                    marqueeText.Append("  |  ");
                }

                marqueeText.Append(ModConfig.Instance.ModsPanelSuffix);

                return marqueeText.ToString();
            }
            catch (Exception e)
            {
                Debug.Log("[Stream It!] StreamManager:GenerateMarqueeText -> Exception: " + e.Message);
                return string.Empty;
            }
        }

        private void StartScrollingIfNeeded()
        {
            try
            {
                if (!_isScrolling)
                {
                    _modsScrollablePanel.scrollPosition = Vector2.zero;

                    if (_modsLabel.width > _modsScrollablePanel.width)
                    {
                        WaitAndStartScrolling();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Stream It!] StreamManager:StartScrollingIfNeeded -> Exception: " + e.Message);
            }
        }

        private void WaitAndStartScrolling()
        {
            try
            {
                Invoke("StartScrolling", 0.8f);
            }
            catch (Exception e)
            {
                Debug.Log("[Stream It!] StreamManager:WaitAndStartScrolling -> Exception: " + e.Message);
            }
        }

        private void StartScrolling()
        {
            try
            {
                _isScrolling = true;

                float overrun = _modsLabel.width - _modsScrollablePanel.width;

                ValueAnimator.Animate("Scrolling", delegate (float val)
                {
                    _modsScrollablePanel.scrollPosition = new Vector2(val, 0f);
                }, new AnimatedFloat(0f, overrun, overrun / ModConfig.Instance.ModsPanelSpeed), delegate
                {
                    OnCompletedScrolling();
                });
            }
            catch (Exception e)
            {
                Debug.Log("[Stream It!] StreamManager:StartScrolling -> Exception: " + e.Message);
            }
        }

        private void OnCompletedScrolling()
        {
            try
            {
                _isScrolling = false;
                Invoke("StartScrollingIfNeeded", 2.5f);
            }
            catch (Exception e)
            {
                Debug.Log("[Stream It!] StreamManager:OnCompletedScrolling -> Exception: " + e.Message);
            }
        }

        private void StopScrolling()
        {
            try
            {
                _isScrolling = false;
                ValueAnimator.Cancel("Scrolling");
                _modsScrollablePanel.scrollPosition = Vector2.zero;
            }
            catch (Exception e)
            {
                Debug.Log("[Stream It!] StreamManager:StopScrolling -> Exception: " + e.Message);
            }
        }

        private void UpdateGraphics()
        {
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    switch (i)
                    {
                        case 0:
                            UIDropDown aspectRatioDropdown = _optionsGraphicsPanel.GetType().GetField("m_AspectRatioDropdown", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_optionsGraphicsPanel) as UIDropDown;
                            _graphicsValueLabels[i].text = aspectRatioDropdown.selectedValue;
                            break;
                        case 1:
                            UIDropDown resolutionDropdown = _optionsGraphicsPanel.GetType().GetField("m_ResolutionDropdown", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_optionsGraphicsPanel) as UIDropDown;
                            _graphicsValueLabels[i].text = resolutionDropdown.selectedValue;

                            break;
                        case 2:
                            UIDropDown vSyncDropdown = _optionsGraphicsPanel.GetType().GetField("m_VSyncDropdown", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_optionsGraphicsPanel) as UIDropDown;
                            _graphicsValueLabels[i].text = vSyncDropdown.selectedValue;

                            break;
                        case 3:
                            UIDropDown colorCorrectionDropdown = _optionsGraphicsPanel.GetType().GetField("m_ColorCorrectionDropdown", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_optionsGraphicsPanel) as UIDropDown;
                            _graphicsValueLabels[i].text = colorCorrectionDropdown.selectedValue;

                            break;
                        case 4:
                            UIDropDown dofTypeDropdown = _optionsGraphicsPanel.GetType().GetField("m_DOFTypeDropdown", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_optionsGraphicsPanel) as UIDropDown;
                            _graphicsValueLabels[i].text = dofTypeDropdown.selectedValue;

                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Stream It!] StreamManager:UpdateGraphics -> Exception: " + e.Message);
            }
        }
    }
}
