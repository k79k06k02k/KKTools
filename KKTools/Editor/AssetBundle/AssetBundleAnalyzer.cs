/**********************************************************
// Author   : K.(k79k06k02k)
// FileName : AssetBundleAnalyzer.cs
// Reference: http://forum.unity3d.com/threads/asset-bundle-analyzer.182413/
**********************************************************/
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AssetBundleAnalyzer : EditorWindow
{
    private struct StatData
    {
        public string typeName;
        public string objName;
        public int size;
    }

    private class StatDataComparer : IComparer<StatData>
    {
        Dictionary<string, int> _typeSizeMap;

        public StatDataComparer(Dictionary<string, int> typeSizeMap)
        {
            _typeSizeMap = typeSizeMap;
        }

        public int Compare(StatData stat1, StatData stat2)
        {
            int typeSize1 = _typeSizeMap[stat1.typeName];
            int typeSize2 = _typeSizeMap[stat2.typeName];

            int stringCompare = stat1.typeName.CompareTo(stat2.typeName);

            if (typeSize1 > typeSize2)
            {
                return -1;
            }
            else if (typeSize1 < typeSize2)
            {
                return +1;
            }
            else if (stringCompare != 0)
            {
                return stringCompare;
            }
            else if (stat1.size > stat2.size)
            {
                return -1;
            }
            else if (stat1.size < stat2.size)
            {
                return +1;
            }
            else
            {
                return 0;
            }
        }
    }

    private Vector2 scrollPosition = Vector2.zero;
    private bool compressed = false;
    private BuildTarget buildTarget = EditorUserBuildSettings.activeBuildTarget;
    private string outputPath = "";
    private string selectedPath = "";
    private Object analyzedObject = null;
    private List<StatData> statistics = new List<StatData>();
    private Dictionary<string, int> typeSizeMap = new Dictionary<string, int>();
    private Dictionary<string, bool> typeStatusMap = new Dictionary<string, bool>();

    public static void ShowWindow()
    {
        EditorWindow script = EditorWindow.GetWindow(typeof(AssetBundleAnalyzer));
        script.autoRepaintOnSceneChange = true;
        script.Show();
        script.titleContent = new GUIContent("AB Analyzer");
    }

    void OnGUI()
    {
        Object currentObject = null;
        string assetPath = "";

        if (Selection.activeObject != null && AssetDatabase.Contains(Selection.activeObject))
        {
            assetPath = Path.GetFullPath(AssetDatabase.GetAssetPath(Selection.activeObject));
            if (assetPath.ToLower().Contains(".abbin"))
            {
                currentObject = Selection.activeObject;
            }
        }

        GUILayout.Label("Asset bundle to analyze", EditorStyles.boldLabel);
        if (currentObject != null)
        {
            FileInfo file = new FileInfo(assetPath);

            GUILayout.Label("   file: " + assetPath);
            GUILayout.Label("   size: " + file.Length / 1024 + " Kb");
        }
        else
        {
            GUILayout.Label("   file: None (select in project)");
            GUILayout.Label("   size: 0 Kb");
        }

        GUILayout.Label("Settings", EditorStyles.boldLabel);

        compressed = EditorGUILayout.Toggle("   Compress", compressed);
        buildTarget = (BuildTarget)EditorGUILayout.EnumPopup("   Build Target", buildTarget);

        EditorGUILayout.BeginHorizontal();
        {
            selectedPath = EditorGUILayout.TextField("   Output path: ", selectedPath);
            if (GUILayout.Button("select", GUILayout.Width(50)))
            {
                selectedPath = EditorUtility.SaveFolderPanel("Select output directory", "", selectedPath);
            }

            outputPath = selectedPath + "/.analyze";
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        if (GUILayout.Button("analyze", GUILayout.Width(100)) && currentObject != null)
        {
            if (outputPath.Length == 0)
            {
                Debug.LogError("Please select valid output path");
                return;
            }

            try
            {
                if (Directory.Exists(outputPath))
                {
                    Directory.Delete(outputPath, true);
                }
            }
            finally
            {
                Directory.CreateDirectory(outputPath);
            }

            if (!Directory.Exists(outputPath))
            {
                Debug.LogError("Please select valid output path");
                return;
            }

            analyzeAssetBundle(currentObject, buildTarget);
        }

        if (analyzedObject != currentObject)
        {
            statistics.Clear();
            analyzedObject = null;
        }

        if (analyzedObject != null)
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            {
                string curType = "";

                foreach (StatData data in statistics)
                {
                    if (curType != data.typeName)
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            typeStatusMap[data.typeName] = EditorGUILayout.Foldout(typeStatusMap[data.typeName], data.typeName);

                            GUI.skin.label.alignment = TextAnchor.MiddleRight;
                            GUI.skin.label.fontStyle = FontStyle.Bold;

                            GUILayout.Label(typeSizeMap[data.typeName] / 1024 + " Kb");
                            GUILayout.Space(400);
                        }
                        EditorGUILayout.EndHorizontal();

                        curType = data.typeName;
                    }

                    if (typeStatusMap[data.typeName])
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            GUI.skin.label.alignment = TextAnchor.MiddleLeft;
                            GUI.skin.label.fontStyle = FontStyle.Normal;

                            GUILayout.Label("    " + data.objName);

                            GUI.skin.label.alignment = TextAnchor.MiddleRight;
                            GUILayout.Label(data.size / 1024 + " Kb");

                            GUILayout.Space(400);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
            EditorGUILayout.EndScrollView();
        }
    }

    private void analyzeAssetBundle(Object obj, BuildTarget buildTarget)
    {
        typeStatusMap.Clear();
        typeSizeMap.Clear();
        statistics.Clear();
        analyzedObject = obj;

        string assetPath = Path.GetFullPath(AssetDatabase.GetAssetPath(obj));
        WWW www = new WWW("file:///" + assetPath);
        Object[] loadedObjects = www.assetBundle.LoadAllAssets();

        foreach (Object loadedObj in loadedObjects)
        {
            string directory = outputPath + "/" + loadedObj.GetType().FullName + "/";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string bundlePath = directory + loadedObj.name.Replace("/", ".") + "." + loadedObj.GetInstanceID() + ".bytes";
            BuildPipeline.BuildAssetBundle(loadedObj, null, bundlePath,
                                           compressed ? 0 : BuildAssetBundleOptions.UncompressedAssetBundle,
                                           buildTarget);

            if (File.Exists(bundlePath))
            {
                StatData stat = new StatData();
                stat.objName = loadedObj.name;
                stat.typeName = loadedObj.GetType().FullName;

                FileInfo fileInfo = new FileInfo(bundlePath);
                stat.size = (int)fileInfo.Length;

                statistics.Add(stat);
            }
        }

        www.assetBundle.Unload(true);
        www.Dispose();

        foreach (StatData data in statistics)
        {
            if (typeSizeMap.ContainsKey(data.typeName))
            {
                typeSizeMap[data.typeName] += data.size;
            }
            else
            {
                typeSizeMap.Add(data.typeName, data.size);
            }
        }

        foreach (string typeName in typeSizeMap.Keys)
        {
            typeStatusMap.Add(typeName, false);
        }

        statistics.Sort(new StatDataComparer(typeSizeMap));
    }
}