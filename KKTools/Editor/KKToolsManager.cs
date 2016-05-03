/**********************************************************
// Author   : K.(k79k06k02k)
// FileName : KKToolsManager.cs
**********************************************************/
using UnityEditor;

public class KKToolsManager
{
    //=======================================================================================
    //AssetBundle
    [MenuItem("KKTools/AssetBundle/AssetBundle Analyze")]
    static void AssetBundle_Analyze()
    {
        AssetBundleAnalyzer.ShowWindow();
    }

    [MenuItem("KKTools/AssetBundle/AssetBundle Watch")]
    static void AssetBundle_Watch()
    {
        AssetBundleWatch.ShowWindow();
    }

    [MenuItem("KKTools/AssetBundle/AssetBundle Build")]
    static void AssetBundle_Build()
    {
        AssetBundleBuild.ShowWindow();
    }

    [MenuItem("KKTools/AssetBundle/AssetBundle Build All Platform")]
    static void AssetBundle_BuildAllPlatform()
    {
        AssetBundleBuild.BuildAllPlatform();
    }

    [MenuItem("KKTools/AssetBundle/AssetBundle Show All Name")]
    static void AssetBundle_ShowAllNames()
    {
        AssetBundleOther.ShowAllAssetBundleNames();
    }


    //=======================================================================================
    //Create
    [MenuItem("KKTools/Create/Make Project Folders")]
    static void Create_MakeFolders()
    {
        MakeFolders.ShowWindow();
    }


    //=======================================================================================
    //Finder
    [MenuItem("KKTools/Finder/Finder")]
    static void Finder_GameObjectFinder()
    {
        GameObjectFinder.ShowWindow();
    }


    //=======================================================================================
    //Prefab
    [MenuItem("KKTools/Prefab/Prefab Tool")]
    static void Prefab_PrefabTool()
    {
        PrefabTool.ShowView();
    }


    //=======================================================================================
    //UGUI
    [MenuItem("KKTools/UGUI/UGUI Tool")]
    static void UGUI_UGUITool()
    {
        UGUITool.ShowWindow();
    }


    //=======================================================================================
    //Window
    [MenuItem("KKTools/Window/Scene Watcher")]
    static void Window_SceneWatcher()
    {
        SceneWatcher.Init();
    }
}
