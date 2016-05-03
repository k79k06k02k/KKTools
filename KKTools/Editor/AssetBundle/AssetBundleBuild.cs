/**********************************************************
// Author   : K.(k79k06k02k)
// FileName : AssetBundleBuild.cs
**********************************************************/
using UnityEngine;
using UnityEditor;

public class AssetBundleBuild : EditorWindow
{
    bool[] isTargetToggle;

    static string[] BUILD_TARGET_NAMES = { "Android", "IOS", "PC" };

    static string folder = "AssetBundles";


    public static void ShowWindow()
    {
        EditorWindow script = EditorWindow.GetWindow(typeof(AssetBundleBuild));
        script.position = new Rect(script.position.xMin + 100f, script.position.yMin + 100f, 600, 120);
        script.autoRepaintOnSceneChange = true;
        script.Show();
        script.titleContent = new GUIContent("AB Build");
    }
    public static void BuildAllPlatform()
    {
        if (EditorUtility.DisplayDialog("",
          "Are you sure?",
          "Yes",
          "No"))
        {
            for (int i = 0; i < BUILD_TARGET_NAMES.Length; i++)
            {
                UtilityEditor.CreateFolder(folder + "/" + BUILD_TARGET_NAMES[i]);

                BuildPipeline.BuildAssetBundles(Application.dataPath + "/" + folder + "/" + BUILD_TARGET_NAMES[i], BuildAssetBundleOptions.CollectDependencies, GetTarget(BUILD_TARGET_NAMES[i]));

            }
        }
    }

    private void OnEnable()
    {
        isTargetToggle = new bool[BUILD_TARGET_NAMES.Length];


        switch (EditorUserBuildSettings.activeBuildTarget)
        {
            case BuildTarget.Android:
                isTargetToggle[0] = true;
                break;

            case BuildTarget.iOS:
                isTargetToggle[1] = true;
                break;

            case BuildTarget.StandaloneWindows:
                isTargetToggle[2] = true;
                break;

            default:
                isTargetToggle[2] = true;
                break;
        }
    }


    void OnGUI()
    {
        EditorGUILayout.LabelField("Choose Build Target", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(25.0f);

        for (int i = 0; i < BUILD_TARGET_NAMES.Length; i++)
        {
            CreateToogle(i, BUILD_TARGET_NAMES[i], isTargetToggle[i]);
        }

        EditorGUILayout.EndHorizontal();
        GUILayout.Space(25.0f);

      
        if (UtilityEditor.GetCommonButton("Create"))
        {
            if (EditorUtility.DisplayDialog("",
           "Are you sure?",
           "Yes",
           "No"))
            {
                for (int i = 0; i < isTargetToggle.Length; i++)
                {
                    if (isTargetToggle[i])
                    {
                        UtilityEditor.CreateFolder(folder + "/" + BUILD_TARGET_NAMES[i]);

                        BuildPipeline.BuildAssetBundles(Application.dataPath + "/" + folder + "/" + BUILD_TARGET_NAMES[i], BuildAssetBundleOptions.CollectDependencies, GetTarget(BUILD_TARGET_NAMES[i]));
                    }
                }

                this.Close();
            }
        }
    }

    void CreateToogle(int index, string name, bool value)
    {
        isTargetToggle[index] = EditorGUILayout.ToggleLeft("  " + name, value);
    }

    static BuildTarget GetTarget(string name)
    {
        switch (name)
        {
            case "Android":
                return BuildTarget.Android;

            case "iOS":
                return BuildTarget.iOS;

            case "PC":
                return BuildTarget.StandaloneWindows;

            default:
                return BuildTarget.StandaloneWindows;
        }

    }
}
