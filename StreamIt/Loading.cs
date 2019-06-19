using ICities;
using System;
using UnityEngine;

namespace StreamIt
{

    public class Loading : LoadingExtensionBase
    {
        private GameObject _streamManagerGameObject;

        public override void OnLevelLoaded(LoadMode mode)
        {
            try
            {
                _streamManagerGameObject = new GameObject("StreamItStreamManager");
                _streamManagerGameObject.AddComponent<StreamManager>();
            }
            catch (Exception e)
            {
                Debug.Log("[Stream It!] Loading:OnLevelLoaded -> Exception: " + e.Message);
            }
        }

        public override void OnLevelUnloading()
        {
            try
            {
                if (_streamManagerGameObject != null)
                {
                    UnityEngine.Object.Destroy(_streamManagerGameObject);
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Stream It!] Loading:OnLevelUnloading -> Exception: " + e.Message);
            }
        }
    }
}