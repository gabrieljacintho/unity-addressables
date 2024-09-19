using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    [Serializable]
    public class AssetReferenceScene : AssetReference
    {
        public AssetReferenceScene(string guid) : base(guid) { }

        public override bool ValidateAsset(string path)
        {
            return path.EndsWith(".unity");
        }
    }

    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private AssetReferenceScene _sceneReference;
        [SerializeField] private LoadSceneMode _mode;
        [SerializeField] private bool _unloadPrevious;
        [SerializeField] private bool _loadOnStart;

        private SceneInstance _currentSceneInstance;


        private void Start()
        {
            if (_loadOnStart)
            {
                LoadScene();
            }
        }

        public void LoadScene()
        {
            LoadScene(_sceneReference);
        }

        public void LoadScene(AssetReference sceneReference)
        {
            LoadScene(sceneReference.AssetGUID);
        }

        public void LoadScene(string addressableKey)
        {
            if (_unloadPrevious)
            {
                UnloadPreviousScene();
            }

            Addressables.LoadSceneAsync(addressableKey, _mode).Completed += asyncHandle =>
            {
                _currentSceneInstance = asyncHandle.Result;
                Debug.Log("Scene loaded.");
            };
        }

        public void UnloadPreviousScene()
        {
            if (!_currentSceneInstance.Scene.IsValid())
            {
                return;
            }

            Addressables.UnloadSceneAsync(_currentSceneInstance).Completed += asyncHandle =>
            {
                _currentSceneInstance = default;
                Debug.Log("Scene unloaded.");
            };
        }
    }
}