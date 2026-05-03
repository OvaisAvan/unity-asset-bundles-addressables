using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DownloadAssetBundle : MonoBehaviour
{
    [SerializeField] private Image _myImageLoad;
    [SerializeField] private AudioSource _myAudioSourceLoad;
    
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
        typeReceived = (TypeWanted)CheckAssetBundleLoadType(assetBundleLoad);
        callbackFunction(assetBundleLoad, typeReceived);
    }

    private int CheckAssetBundleLoadType(dynamic assetBundleLoad)
    {
        TypeWanted typeWanted = TypeWanted.Unknown;
        if (assetBundleLoad is GameObject)
        {
            typeWanted = TypeWanted.GameObject;
        }
        else if (assetBundleLoad is Texture2D)
        {
            typeWanted = TypeWanted.Texture2D;
        }
        else if (assetBundleLoad is AudioClip)
        {
            typeWanted = TypeWanted.AudioClip;
        }
        else if (assetBundleLoad is Sprite)
        {
            typeWanted = TypeWanted.Sprite;
        }
        else if (assetBundleLoad is ScriptableObject)
        {
            typeWanted = TypeWanted.ScriptableObject;
        }
        return (int)typeWanted;
    }

    private void ApplyImportedSpriteFromAssetBundle(Sprite sprite)
    {
        _myImageLoad.sprite = sprite;
        _myImageLoad.type = Image.Type.Simple;
        _myImageLoad.preserveAspect = true;
    }

    private void PlayImportedAudioClipFromAssetBundle(AudioClip clip)
    {
        _myAudioSourceLoad.clip = clip;
        _myAudioSourceLoad.Play();
    }

    private void InstantiateGameObjectFromAssetBundle(GameObject gameObject)
    {
        GameObject InstantiateGo = Instantiate(gameObject);
        InstantiateGo.transform.position = Vector3.zero;
    }
}
