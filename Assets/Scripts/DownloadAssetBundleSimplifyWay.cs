using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DownloadAssetBundleSimplifyWay : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(DownloadAssetBundleFromServer());
    }

    private IEnumerator DownloadAssetBundleFromServer()
    {
        GameObject go = null;
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
                go = bundle.LoadAsset(bundle.GetAllAssetNames()[0]) as GameObject;
                bundle.Unload(false);
                yield return new WaitForEndOfFrame();
            }
            www.Dispose();
        }
        InstantiateGameObject(go);
    }

    private void InstantiateGameObject(GameObject go)
    {
        if (go != null)
        {
            GameObject intantiateGo=Instantiate(go);
            intantiateGo.transform.position = Vector3.zero;
        }
        else
        {
            Debug.Log("your asset bundle GameObject is null");
        }
    }
}
