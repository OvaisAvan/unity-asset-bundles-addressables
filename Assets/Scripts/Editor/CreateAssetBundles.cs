using System;
using UnityEditor;
using UnityEngine;

public class CreateAssetBundles 
{
    [MenuItem("BlackSpider Studios/Create/AssetBundles")]
    private static void BuildAssetBundles()
    {
        Debug.Log("Build AssetBundles");
        string assetBundleDirectoryPath = Application.dataPath + "/AssetBundles";
        try
        {
            BuildPipeline.BuildAssetBundles(assetBundleDirectoryPath, BuildAssetBundleOptions.None,
                EditorUserBuildSettings.activeBuildTarget);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
        }
    }
}
