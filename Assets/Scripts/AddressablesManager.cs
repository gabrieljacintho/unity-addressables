using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets.ResourceLocators;

[Serializable]
public class AssetReferenceAudioClip : AssetReferenceT<AudioClip>
{
    public AssetReferenceAudioClip(string guid) : base(guid) { }
}

public class AddressablesManager : MonoBehaviour
{
    [SerializeField] private AssetReference _playerAssetReference;
    [SerializeField] private AssetReferenceAudioClip _musicAssetReference;
    [SerializeField] private AssetReferenceTexture2D _logoAssetReference;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private RawImage _logoRawImage;

    private GameObject _playerInstance;


    private void Start()
    {
        Addressables.InitializeAsync().Completed += AddressablesManager_Completed;
        Debug.Log("Addressables initializing...");
    }

    private void AddressablesManager_Completed(AsyncOperationHandle<IResourceLocator> obj)
    {
        Debug.Log("Addressables initialized.");

        _playerAssetReference.InstantiateAsync().Completed += (instance) =>
        {
            _playerInstance = instance.Result;
            _virtualCamera.Follow = _playerInstance.transform.Find("PlayerCameraRoot");
            Debug.Log("Player instantiated.");
        };

        _musicAssetReference.LoadAssetAsync<AudioClip>().Completed += (audioClip) =>
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = audioClip.Result;
            audioSource.playOnAwake = false;
            audioSource.loop = true;
            audioSource.Play();
            Debug.Log("AudioClip loaded.");
        };

        _logoAssetReference.LoadAssetAsync<Texture2D>();
    }

    private void Update()
    {
        if (_logoAssetReference.Asset != null && _logoRawImage.texture == null)
        {
            _logoRawImage.texture = _logoAssetReference.Asset as Texture2D;
            Color currentColor = _logoRawImage.color;
            currentColor.a = 1f;
            _logoRawImage.color = currentColor;
            Debug.Log("Logo loaded.");
        }
    }

    private void OnDestroy()
    {
        _playerAssetReference.ReleaseInstance(_playerInstance);
        _logoAssetReference.ReleaseAsset();
    }
}
