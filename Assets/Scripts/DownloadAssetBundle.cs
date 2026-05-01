using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class DownloadAssetBundle : MonoBehaviour
{
    private enum TypeWanted
    {
        Unknown,
        GameObject,
        Texture2D,
        AudioClip,
        Sprite,
        ScriptableObject,
    }
    
    private IEnumerator DownloadAssetBundleFromServer(CallBack<dynamic, 
        TypeWanted> callbackFunction, string assetBundleName="")
    {
        dynamic assetBundleLoad = null;
        TypeWanted typeReceived = TypeWanted.Unknown;
        string url = "https://blackspiderstudios.com/assets/cube_prefab";
        using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(url))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError ||
                www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogWarning("Error on the get request at: " + url + " " + www.error);
            }
            else
            {
                AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
                assetBundleLoad = bundle.LoadAsset(bundle.GetAllAssetNames()[0]);
                bundle.Unload(false);
                yield return new WaitForEndOfFrame();
            }
            www.Dispose();
        }
        Debug.Log("The asset bundle unloaded is a type of: " + assetBundleLoad);
    }
}
