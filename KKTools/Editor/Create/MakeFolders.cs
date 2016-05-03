/**********************************************************
// Author   : K.(k79k06k02k)
// FileName : MakeFolders.cs
// Reference: http://wiki.unity3d.com/index.php/Create_project_directories
**********************************************************/
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MakeFolders : EditorWindow
{
    private Vector2 scrollPos;

    private bool isToggleAll = false;
    private bool isToggleAllLast = false;

    private bool[] isCreateFolder;
    private List<Folder> folderList = new List<Folder>();

    public class Folder
    {
        public Folder(string name, bool isDefaultCreate = false)
        {
            this.name = name;
            this.isDefaultCreate = isDefaultCreate;
        }

        public string name;
        public bool isDefaultCreate;
    }

    public static void ShowWindow()
    {
        EditorWindow editorWindow = EditorWindow.GetWindow(typeof(MakeFolders));
        editorWindow.position = new Rect(editorWindow.position.xMin + 100f, editorWindow.position.yMin + 100f, 400f, 400f);
        editorWindow.autoRepaintOnSceneChange = true;
        editorWindow.Show();
        editorWindow.titleContent = new GUIContent("Make Folders");
    }

    void OnEnable()
    {
        folderList = new List<Folder>();
        folderList.Add(new Folder("Scenes", true));
        folderList.Add(new Folder("Scripts", true));
        folderList.Add(new Folder("Plugins", true));
        folderList.Add(new Folder("Resources", true));
        folderList.Add(new Folder("Resources/Materials"));
        folderList.Add(new Folder("Resources/Models"));
        folderList.Add(new Folder("Resources/Textures"));
        folderList.Add(new Folder("Resources/Shaders"));
        folderList.Add(new Folder("Resources/Prefabs"));
        folderList.Add(new Folder("Resources/Sprites"));
        folderList.Add(new Folder("Resources/Physics"));
        folderList.Add(new Folder("Resources/Fonts"));
        folderList.Add(new Folder("Resources/Music"));


        isCreateFolder = new bool[folderList.Count];

        for (int i = 0; i < folderList.Count; i++)
            isCreateFolder[i] = folderList[i].isDefaultCreate;

    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("Choose Folders", EditorStyles.boldLabel);

        EditorGUILayout.BeginVertical();
        this.scrollPos = EditorGUILayout.BeginScrollView(this.scrollPos, false, false);
        EditorGUILayout.Space();

        isToggleAll = EditorGUILayout.ToggleLeft("  Toggle All", isToggleAll);

        if (isToggleAll != isToggleAllLast)
            ToggleAll(isToggleAll);

        EditorGUILayout.Space();

        for (int i = 0; i < folderList.Count; i++)
        {
            CreateToogle(i, folderList[i]);
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();

        if (UtilityEditor.GetCommonButton("Make MakeFolders"))
        {

            if (Check() == false)
            {
                if (EditorUtility.DisplayDialog("Alert", "Please Choose Folder!!", "OK"))
                    return;
            }

            if (EditorUtility.DisplayDialog("",
           "Are you sure?",
           "Yes",
           "No"))
            {
                for (int i = 0; i < isCreateFolder.Length; i++)
                {
                    if (isCreateFolder[i] == true)
                        CreateFolders(folderList[i].name);
                }


                this.Close();
            }
        }

        EditorGUILayout.Space();

    }

    void CreateToogle(int index, Folder folder)
    {
        isCreateFolder[index] = EditorGUILayout.ToggleLeft("  " + folder.name, isCreateFolder[index]);
    }

    void CreateFolders(string name)
    {
        bool isCreate = false;

        string[] splitName = name.Split('/');

        string prefixFolderName = "";
        string pathValid = "";
        for (int i = 0; i < splitName.Length; i++)
        {
            pathValid += "/" + splitName[i];

            if (AssetDatabase.IsValidFolder("Assets" + pathValid) == false)
            {
                AssetDatabase.CreateFolder("Assets" + prefixFolderName, splitName[i]);
                isCreate = true;
            }

            prefixFolderName += "/" + splitName[i];
        }


        if (isCreate)
            Debug.Log("Folder [ Assets/" + name + " ] is Create!!");
        else
            Debug.LogWarning("Folder [ Assets/" + name + " ] is Exist!!");
    }

    bool Check()
    {
        for (int i = 0; i < isCreateFolder.Length; i++)
            if (isCreateFolder[i] == true)
                return true;

        return false;
    }

    void ToggleAll(bool isToggle)
    {
        isToggleAllLast = isToggle;

        for (int i = 0; i < isCreateFolder.Length; i++)
            isCreateFolder[i] = isToggle;
    }
}