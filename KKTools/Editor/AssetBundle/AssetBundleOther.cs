/**********************************************************
// Author   : K.(k79k06k02k)
// FileName : AssetBundleOther.cs
**********************************************************/
using UnityEngine;
using UnityEditor;
using System.Text;

public class AssetBundleOther : AssetPostprocessor
{
    public static void ShowAllAssetBundleNames()
    {
        StringBuilder sb = new StringBuilder();
        string[] assetBundleNames = AssetDatabase.GetAllAssetBundleNames();

        foreach (string str in assetBundleNames)
            sb.Append(str + "\n");

        Debug.Log("AssetBundleNames：\n" + sb.ToString());
    }

    void OnPostprocessAssetbundleNameChanged(string path, string previous, string next)
    {
        Debug.Log("AB: " + path + "\told: " + previous + "\tnew: " + next);
    }
}
