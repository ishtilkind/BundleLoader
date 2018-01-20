using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Loader : MonoBehaviour
{
    public bool debug = false;
    public Bundle[] bundles;

    private Dictionary<AssetBundle, List<GameObject> > loadedObjects;

    void Start () {
        StartCoroutine(LoadAssetBundlesAsync() );
    }

    public void UnloadBundles()
    {
        if (null == loadedObjects) return;

        //AssetBundle.UnloadAllAssetBundles(true);

        var enumerator =  loadedObjects.GetEnumerator();

        while (enumerator.MoveNext())
        {
            var golist = enumerator.Current.Value;

            var ab = enumerator.Current.Key;
            ab.Unload(true);

            foreach (var go in golist)
            {
                Destroy(go);
            }
        }

        loadedObjects.Clear();

        //foreach (var bundle in loadedObjects)
        //{
        //    bundle.
        //}
    }

    public IEnumerator LoadAssetBundlesAsync()
    {
        if (null == bundles || bundles.Length == 0)
            yield break;
        
        foreach (var bundle in bundles)
        {
            if (null == loadedObjects)
                loadedObjects = new Dictionary<AssetBundle, List<GameObject>>();

            yield return StartCoroutine(InstantiateObject(bundle.bundleName, bundle.assetName));
        }
    }



    public void LoadAssetBundles()
    {
        StartCoroutine(LoadAssetBundlesAsync());
    }
    public void LoadAssetBundle(string name)
    {

    }

    IEnumerator InstantiateObject(string assetBundleName, string assetName=null)
    {
        if(!string.IsNullOrEmpty(assetBundleName))
        {
            yield return null ;
        }
#if UNITY_STANDALONE_WIN
        string uri = "file:///" + Application.dataPath + "/../AssetBundles/StandaloneWindows/" + assetBundleName;
#endif

#if UNITY_WEBGL
        string uri = Application.dataPath + "/../AssetBundles/WebGL/" + assetBundleName;
#endif

        if (debug) Debug.Log(uri);
        UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequest.GetAssetBundle(uri, 0);
        yield return request.SendWebRequest();
        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
        if (null == bundle)
            yield return null;

        var list = new List<GameObject>();


        if (!string.IsNullOrEmpty(assetName))
        {
            GameObject[] gos = bundle.LoadAssetWithSubAssets<GameObject>(assetName);
            foreach (var go in gos)
            {
                var o = Instantiate(go);
                list.Add(o);

            }
        }

        loadedObjects.Add(bundle, list );
       


    }
}
