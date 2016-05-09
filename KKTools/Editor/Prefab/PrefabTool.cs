/**********************************************************
// Author   : K.(k79k06k02k)
// FileName : PrefabTool.cs
**********************************************************/
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PrefabTool : EditorWindow
{
    string path = "Assets/";
    string extension = ".prefab";
    string[] Types = new string[] { "Create", "Apply" };
    int tabIndex = 0;

    Vector2 scrollPos;
    GUIStyle BtnStyle;
    List<GameObject> selectObj = new List<GameObject>();

    public static void ShowView()
    {
        EditorWindow script = GetWindow(typeof(PrefabTool));
        script.position = new Rect(script.position.xMin + 100f, script.position.yMin + 100f, 500, 700);
        script.autoRepaintOnSceneChange = true;
        script.Show();
        script.titleContent = new GUIContent("Prefab Tool");
    }

    void OnHierarchyChange()
    {
        Repaint();
    }
    void OnSelectionChange()
    {
        Repaint();
    }

    void OnGUI()
    {
        tabIndex = UtilityEditor.Tabs(Types, tabIndex);
        GUILayout.Space(10);

        BtnStyle = new GUIStyle(GUI.skin.button);
        BtnStyle.fontSize = 16;
        BtnStyle.alignment = TextAnchor.MiddleLeft;

        selectObj.Clear();
        selectObj = Selection.gameObjects.ToList();

        for (int i = selectObj.Count - 1; i >= 0; i--)
        {
            if (GetFiltered(selectObj[i]))
                selectObj.Remove(selectObj[i]);
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Choose GameObjects", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Count : " + selectObj.Count, EditorStyles.boldLabel);
        EditorGUILayout.EndHorizontal();

        if (selectObj.Count == 0)
        {
            EditorGUILayout.HelpBox(GetHelpString(), MessageType.Warning);
            return;
        }

        selectObj.Sort(delegate (GameObject a, GameObject b)
        {
            return a.name.CompareTo(b.name);
        });

        EditorGUILayout.BeginVertical();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);
        EditorGUILayout.Space();


        foreach (GameObject obj in selectObj)
        {
            if (GUILayout.Button(obj.name, BtnStyle))
                EditorGUIUtility.PingObject(obj);
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();


        if (UtilityEditor.GetCommonButton(Types[tabIndex]))
            Execute();
    }

    string GetHelpString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("No Source." + "\n\n");
        sb.Append("Please Check Select GameObject：" + "\n");

        switch (tabIndex)
        {
            case 0:
                sb.Append("1.GameObject Is In Scene" + "\n");
                sb.Append("2.GameObject Is not a Prefab" + "\n");
                sb.Append("3.GameObject Is Active");
                break;

            case 1:
                sb.Append("1.GameObject Is In Scene" + "\n");
                sb.Append("2.GameObject Is a Prefab");
                break;
        }

        return sb.ToString();
    }


    bool GetFiltered(GameObject obj)
    {
        switch (tabIndex)
        {
            case 0:
                return PrefabUtility.GetPrefabType(obj) == PrefabType.PrefabInstance || PrefabUtility.GetPrefabType(obj) == PrefabType.Prefab || obj.activeSelf == false;

            case 1:
                return PrefabUtility.GetPrefabType(obj) != PrefabType.PrefabInstance;

            default:
                return true;
        }
    }

    void Execute()
    {
        switch (tabIndex)
        {
            case 0:
                PrefabCreate();
                break;

            case 1:
                PrefabApply();
                break;
        }
    }

    void PrefabCreate()
    {
        if (EditorUtility.DisplayDialog("",
            "Are you sure?",
            "Yes",
            "No"))
        {

            int count = 1;
            foreach (GameObject obj in selectObj)
            {
                EditorUtility.DisplayProgressBar("Create Prefab", string.Format("({0} / {1})　Path:[{2}]；Name:[{3}]", count, selectObj.Count, path, obj.name), count / (float)selectObj.Count);

                Object emptyPrefab = PrefabUtility.CreateEmptyPrefab(path + obj.name + extension);

                if (emptyPrefab != null)
                {
                    GameObject prefab = PrefabUtility.ReplacePrefab(obj, emptyPrefab);

                    if (prefab != null)
                        Debug.Log(string.Format("Create Prefab　　Path:[{0}]；Name:[{1}]　Success!!", path, obj.name + extension));
                }

                AssetDatabase.Refresh();
                count++;
            }

            EditorUtility.ClearProgressBar();
        }
    }

    void PrefabApply()
    {
        if (EditorUtility.DisplayDialog("",
            "Are you sure?",
            "Yes",
            "No"))
        {

            for (int i = 0; i < selectObj.Count; i++)
            {
                Object parentObject = PrefabUtility.GetPrefabParent(selectObj[i]);
                string prefabPath = AssetDatabase.GetAssetPath(parentObject);
                prefabPath = prefabPath.Substring(0, prefabPath.Length - (selectObj[i].name + extension).Length);


                EditorUtility.DisplayProgressBar("Apply Prefab", string.Format("({0} / {1})　Path:[{2}]；Name:[{3}]", i + 1, selectObj.Count, prefabPath, selectObj[i].name), (i + 1) / (float)selectObj.Count);
                GameObject prefab = PrefabUtility.ReplacePrefab(selectObj[i], PrefabUtility.GetPrefabParent(selectObj[i]), ReplacePrefabOptions.ConnectToPrefab);

                if (prefab != null)
                    Debug.Log(string.Format("Apply Prefab　　Path:[{0}]；Name:[{1}]　Success!!", prefabPath, selectObj[i].name + extension));
            }

            EditorUtility.ClearProgressBar();
        }
    }
}
