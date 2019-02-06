using ColossalFramework;
using ColossalFramework.Plugins;
using ColossalFramework.UI;
using ICities;
using System;
using System.Text;
using UnityEngine;

namespace StreamIt
{
    class Streamer : MonoBehaviour
    {
        private bool _initialized;

        private UIComponent _chirperPanel;
        private bool _wasChirperPanelEnabled;

        private bool _isScrolling;
        private UIScrollablePanel _marqueeScrollablePanel;
        private UIDragHandle _marqueeDragHandle;
        private UILabel _marqueeLabel;

        private void Awake()
        {
            try
            {
                _chirperPanel = GameObject.Find("ChirperPanel").GetComponent<UIComponent>();

                _wasChirperPanelEnabled = _chirperPanel != null ? _chirperPanel.isEnabled : false;

                CreateMarquee();
            }
            catch (Exception e)
            {
                Debug.Log("[Stream It!] StreamPanel:Awake -> Exception: " + e.Message);
            }
        }

        private void OnEnable()
        {
            try
            {

            }
            catch (Exception e)
            {
                Debug.Log("[Stream It!] Streamer:OnEnable -> Exception: " + e.Message);
            }
        }

        private void Start()
        {
            try
            {

            }
            catch (Exception e)
            {
                Debug.Log("[Stream It!] Streamer:Start -> Exception: " + e.Message);
            }
        }

        private void Update()
        {
            try
            {
                if (!_initialized || ModConfig.Instance.ConfigUpdated)
                {
                    if (_isScrolling)
                    {
                        StopScrolling();
                    }

                    if (ModConfig.Instance.Enabled)
                    {
                        _marqueeScrollablePanel.enabled = true;

                        UpdateMarquee();

                        _marqueeLabel.text = GenerateMarqueeText();

                        StartScrollingIfNeeded();
                    }
                    else
                    {
                        _marqueeScrollablePanel.enabled = false;
                    }

                    _initialized = true;
                    ModConfig.Instance.ConfigUpdated = false;
                }

                if (_chirperPanel.isEnabled != _wasChirperPanelEnabled)
                {
                    UpdateMarquee();

                    _wasChirperPanelEnabled = !_wasChirperPanelEnabled;
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Stream It!] Streamer:Update -> Exception: " + e.Message);
            }
        }

        private void OnDisable()
        {
            try
            {

            }
            catch (Exception e)
            {
                Debug.Log("[Stream It!] Streamer:OnDisable -> Exception: " + e.Message);
            }
        }

        private void OnDestroy()
        {
            try
            {

            }
            catch (Exception e)
            {
                Debug.Log("[Stream It!] Streamer:OnDestroy -> Exception: " + e.Message);
            }
        }

        private string GenerateMarqueeText()
        {
            string marquee = "No mods found.";

            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(ModConfig.Instance.Prefix);
                sb.Append("     ");

                foreach (PluginManager.PluginInfo pluginInfo in Singleton<PluginManager>.instance.GetPluginsInfo())
                {
                    if (pluginInfo.isEnabled)
                    {
                        IUserMod[] instances = pluginInfo.GetInstances<IUserMod>();

                        if (instances.Length == 1)
                        {
                            sb.Append(instances[0].Name);
                            sb.Append("  |  ");
                        }
                    }
                }

                sb.Append(ModConfig.Instance.Suffix);

                return sb.ToString();
            }
            catch (Exception e)
            {
                Debug.Log("[Stream It!] Streamer:GenerateMarqueeText -> Exception: " + e.Message);
            }

            return marquee;
        }

        private void CreateMarquee()
        {
            try
            {
                UIComponent infoPanel = GameObject.Find("InfoPanel").GetComponent<UIComponent>();

                if (infoPanel != null)
                {
                    _marqueeScrollablePanel = UIUtils.CreateScrollablePanel(infoPanel, "StreamItMarqueeScrollablePanel");
                    _marqueeScrollablePanel.isInteractive = false;
                    _marqueeScrollablePanel.clipChildren = true;

                    _marqueeDragHandle = UIUtils.CreateDragHandle(infoPanel, "StreamItMarqueeDragHandle", _marqueeScrollablePanel);
                    _marqueeDragHandle.anchor = UIAnchorStyle.Top | UIAnchorStyle.Left;
                    _marqueeDragHandle.eventDragStart += (component, eventParam) =>
                    {
                        if (_isScrolling)
                        {
                            StopScrolling();
                        }
                    };
                    _marqueeDragHandle.eventMouseUp += (component, eventParam) =>
                    {
                        ModConfig.Instance.PositionX = _marqueeScrollablePanel.relativePosition.x;
                        ModConfig.Instance.PositionY = _marqueeScrollablePanel.relativePosition.y;
                        ModConfig.Instance.Save();

                        if (!_isScrolling)
                        {
                            UpdateMarquee();
                            StartScrollingIfNeeded();
                        }
                    };

                    _marqueeLabel = UIUtils.CreateLabel(_marqueeScrollablePanel, "StreamItMarqueeLabel", "No mods found.");
                    _marqueeLabel.isInteractive = false;
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Stream It!] Streamer:CreateMarquee -> Exception: " + e.Message);
            }
        }

        private void UpdateMarquee()
        {
            try
            {
                float positionX = ModConfig.Instance.PositionX;
                float positionY = ModConfig.Instance.PositionY;

                if (positionX == 0.0f && positionY == 0.0f)
                {
                    float topOffset;

                    if (_chirperPanel != null && _chirperPanel.isEnabled)
                    {
                        topOffset = 134;
                    }
                    else
                    {
                        topOffset = 64;
                    }

                    positionX = UIView.GetAView().GetScreenResolution().x / 2f - _marqueeScrollablePanel.width / 2f;
                    positionY = 0 - UIView.GetAView().GetScreenResolution().y + topOffset;
                }

                _marqueeScrollablePanel.size = new Vector3(ModConfig.Instance.Width, 44);
                _marqueeScrollablePanel.relativePosition = new Vector3(positionX, positionY);

                _marqueeDragHandle.size = _marqueeScrollablePanel.size;
                _marqueeDragHandle.relativePosition = _marqueeScrollablePanel.relativePosition;

                _marqueeLabel.textScale = ModConfig.Instance.TextScale;
            }
            catch (Exception e)
            {
                Debug.Log("[Stream It!] Streamer:UpdateMarquee -> Exception: " + e.Message);
            }
        }

        private void StartScrollingIfNeeded()
        {
            try
            {
                if (!_isScrolling)
                {
                    _marqueeScrollablePanel.scrollPosition = Vector2.zero;

                    if (_marqueeLabel.width > _marqueeScrollablePanel.width)
                    {
                        WaitAndStartScrolling();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Stream It!] Streamer:StartScrollingIfNeeded -> Exception: " + e.Message);
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
                Debug.Log("[Stream It!] Streamer:WaitAndStartScrolling -> Exception: " + e.Message);
            }
        }

        private void StartScrolling()
        {
            try
            {
                _isScrolling = true;

                float overrun = _marqueeLabel.width - _marqueeScrollablePanel.width;

                ValueAnimator.Animate("StreamItScrolling", delegate (float val)
                {
                    _marqueeScrollablePanel.scrollPosition = new Vector2(val, 0f);
                }, new AnimatedFloat(0f, overrun, overrun / ModConfig.Instance.Speed), delegate
                {
                    OnCompletedScrolling();
                });
            }
            catch (Exception e)
            {
                Debug.Log("[Stream It!] Streamer:StartScrolling -> Exception: " + e.Message);
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
                Debug.Log("[Stream It!] Streamer:OnCompletedScrolling -> Exception: " + e.Message);
            }
        }

        private void StopScrolling()
        {
            try
            {
                _isScrolling = false;
                ValueAnimator.Cancel("StreamItScrolling");
                _marqueeScrollablePanel.scrollPosition = Vector2.zero;
            }
            catch (Exception e)
            {
                Debug.Log("[Stream It!] Streamer:StopScrolling -> Exception: " + e.Message);
            }
        }
    }
}
