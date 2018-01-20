using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LoaderDynamic : MonoBehaviour
{
    private AssetBundleCreateRequest bundleRequest;
    private UnityWebRequest request;

    private void Start()
    {
        request = UnityWebRequest.GetAssetBundle("https://unity3dcollegedownloads.blob.core.windows.net/assetbundles/StreamingAssets/Windows/level1");
        request.SendWebRequest();
    }

    private void Update()
    {
        if (request.isDone)
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
            SceneManager.LoadScene("Level1");
        }
    }
}
