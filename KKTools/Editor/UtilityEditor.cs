/**********************************************************
// Author   : K.(k79k06k02k)
// FileName : UtilityEditor.cs
**********************************************************/
using UnityEngine;
using UnityEditor;

public class UtilityEditor
{
    public static bool GetCommonButton(string btnName)
    {
        GUIStyle BtnStyle = new GUIStyle(GUI.skin.button);
        BtnStyle.fontSize = 25;
        BtnStyle.fixedHeight = 50;

        return GUILayout.Button(btnName, BtnStyle);
    }


    /// <summary>
    /// Create Folder(path not include "Assets")
    /// EX: GameResources/Prefabs/Sprites/Enemy
    /// </summary>
    public static void CreateFolder(string name)
    {
        string[] splitName = name.Split('/');

        string prefixFolderName = "";
        string pathValid = "";
        for (int i = 0; i < splitName.Length; i++)
        {
            pathValid += "/" + splitName[i];

            if (AssetDatabase.IsValidFolder("Assets" + pathValid) == false)
                AssetDatabase.CreateFolder("Assets" + prefixFolderName, splitName[i]);

            prefixFolderName += "/" + splitName[i];
        }
    }

    public static int Tabs(string[] options, int selected)
    {
        const float DarkGray = 0.6f;
        const float LightGray = 0.9f;
        const float StartSpace = 10;

        GUILayout.Space(StartSpace);
        Color storeColor = GUI.backgroundColor;
        Color highlightCol = new Color(LightGray, LightGray, LightGray);
        Color bgCol = new Color(DarkGray, DarkGray, DarkGray);

        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.padding.bottom = 8;

        GUILayout.BeginHorizontal();
        {   
            for (int i = 0; i < options.Length; ++i)
            {
                GUI.backgroundColor = i == selected ? highlightCol : bgCol;
                if (GUILayout.Button(options[i], buttonStyle))
                {
                    selected = i; 
                }
            }
        }
        GUILayout.EndHorizontal();

        GUI.backgroundColor = storeColor;

        var texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, highlightCol);
        texture.Apply();
        GUI.DrawTexture(new Rect(0, buttonStyle.lineHeight + buttonStyle.border.top + buttonStyle.margin.top + StartSpace, Screen.width, 4), texture);

        return selected;
    }
}
