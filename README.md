# Unity Addressables

## Links
[Documentation](https://docs.unity3d.com/Packages/com.unity.addressables@2.6/manual/index.html)

[Webinar](https://www.youtube.com/watch?v=C5puKyuFrpM)

[Tutorials](https://www.youtube.com/playlist?list=PLQMQNmwN3FvwcDh-oo0lHYyqTo7F8V7t6)

[AWS (S3)](https://aws.amazon.com/)

## How to load a single asset?
[AddressablesManager.cs](https://github.com/gabrieljacintho/unity-addressables/blob/c6fd6d4a3253afce9ecff2bdfff7c5aa0617d26a/Assets/Scripts/AddressablesManager.cs)
```
private AssetReference _playerAssetReference;

private void Start()
{
    Addressables.InitializeAsync().Completed += AddressablesManager_Completed;
}

private void AddressablesManager_Completed(AsyncOperationHandle<IResourceLocator> obj)
{
    _playerAssetReference.LoadAssetAsync<GameObject>().Completed += prefab =>
    {
        _playerAssetReference.InstantiateAsync().Completed += (instance) =>
        {
            _playerInstance = instance.Result;
        };
    };
}
```

## How to release an asset?
```
private void OnDestroy()
{
    _playerAssetReference.ReleaseInstance(_playerInstance);
    _logoAssetReference.ReleaseAsset();
}
```

## How to load multiple assets?
```
 List<string> labels = new List<string>() { "characters", "animals" };
 Addressables.LoadAssetsAsync<GameObject>(labels, addressable =>
 {
     //Gets called for every loaded asset
     Instantiate(addressable);
 });
```

## How to load a scene?
[SceneLoader.cs](https://github.com/gabrieljacintho/unity-addressables/blob/c6fd6d4a3253afce9ecff2bdfff7c5aa0617d26a/Assets/Scripts/SceneLoader.cs)
```
public void LoadScene(AssetReference sceneReference)
{
    LoadScene(sceneReference.AssetGUID);
}

public void LoadScene(string addressableKey)
{
    Addressables.LoadSceneAsync(addressableKey, LoadSceneMode.Single);
}
```

## How to create a custom AssetReference?
```
[Serializable]
public class AssetReferenceAudioClip : AssetReferenceT<AudioClip>
{
    public AssetReferenceAudioClip(string guid) : base(guid) { }
}
```
```
[Serializable]
public class AssetReferenceScene : AssetReference
{
   public AssetReferenceScene(string guid) : base(guid) { }

   public override bool ValidateAsset(string path)
   {
       return path.EndsWith(".unity");
   }
}
```
