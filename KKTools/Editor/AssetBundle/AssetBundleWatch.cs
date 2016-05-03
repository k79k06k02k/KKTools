/**********************************************************
// Author   : K.(k79k06k02k)
// FileName : AssetBundleWatch.cs
**********************************************************/
using UnityEngine;
using UnityEditor;

public class AssetBundleWatch : EditorWindow
{
    string[] Types = new string[] { "Select One", "Show All" };
    int tabIndex = 0;

    private string[] assetBundleNames;
    public int index = 0;

    string _LevelStr = "    ";

    Vector2 scrollPos;
    GUIStyle BtnStyle;
    GUIStyle LableStyle;
    GUIStyle BoxStyle;
    bool isShowAll = false;

    public static void ShowWindow()
    {
        EditorWindow script = EditorWindow.GetWindow(typeof(AssetBundleWatch));
        script.position = new Rect(script.position.xMin + 100f, script.position.yMin + 100f, 700, 700);
        script.autoRepaintOnSceneChange = true;
        script.Show();
        script.titleContent = new GUIContent("AB Watch");
    }

    void OnGUI()
    {
        BtnStyle = new GUIStyle(GUI.skin.button);
        BtnStyle.fontSize = 16;
        BtnStyle.alignment = TextAnchor.MiddleLeft;

        LableStyle = new GUIStyle(GUI.skin.label);
        LableStyle.fontSize = 16;
        LableStyle.alignment = TextAnchor.MiddleLeft;

        BoxStyle = new GUIStyle(GUI.skin.box);

        assetBundleNames = AssetDatabase.GetAllAssetBundleNames();


        if (assetBundleNames.Length == 0)
        {
            EditorGUILayout.HelpBox("No AssetBundle Data", MessageType.Warning);
            return;
        }

        tabIndex = UtilityEditor.Tabs(Types, tabIndex);
        GUILayout.Space(10);

        string[] assetName = null;

        switch (tabIndex)
        {
            case 0:
                EditorGUILayout.LabelField("Select AssetBundle Names", EditorStyles.boldLabel);

                index = EditorGUILayout.Popup(index, assetBundleNames);
                GUILayout.Space(10f);

                assetName = AssetDatabase.FindAssets("b:" + assetBundleNames[index]);

                EditorGUILayout.BeginVertical();
                this.scrollPos = EditorGUILayout.BeginScrollView(this.scrollPos, false, false);
                EditorGUILayout.Space();


                foreach (string item in assetName)
                {
                    string path = AssetDatabase.GUIDToAssetPath(item);
                    string[] level = path.Split('/');

                    if (GUILayout.Button(GetLevelString(level.Length - 1) + path, BtnStyle))
                        EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<Object>(path));
                }


                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();

                GUILayout.Space(25.0f);
                break;


            case 1:
                EditorGUILayout.LabelField("AssetBundle", EditorStyles.boldLabel);

                EditorGUILayout.BeginVertical();
                this.scrollPos = EditorGUILayout.BeginScrollView(this.scrollPos, false, false);

                int count = 1;
                foreach (string name in assetBundleNames)
                {
                    assetName = AssetDatabase.FindAssets("b:" + name);

                    EditorGUILayout.BeginVertical(BoxStyle);
                    GUILayout.Label(count.ToString("000") + ".  " + name + "：", LableStyle);
                    foreach (string item in assetName)
                    {
                        string path = AssetDatabase.GUIDToAssetPath(item);
                        string[] level = path.Split('/');

                        if (GUILayout.Button(GetLevelString(level.Length - 1) + path, BtnStyle))
                            EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<Object>(path));
                    }
                    EditorGUILayout.EndVertical();

                    count++;
                    GUILayout.Space(25.0f);
                }

                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();

                GUILayout.Space(25.0f);
                break;
        }
    }


    string GetLevelString(int count)
    {
        string temp = string.Empty;

        for (int i = 0; i < count; i++)
            temp += _LevelStr;

        return temp;
    }
}
