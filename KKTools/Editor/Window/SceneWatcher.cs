/**********************************************************
// Author   : K.(k79k06k02k)
// FileName : SceneWatcher.cs
// Reference: http://wiki.unity3d.com/index.php/SceneViewWindow
**********************************************************/

using System.IO;
using UnityEditor;
using UnityEngine;

public class SceneWatcher : EditorWindow
{
    
    private Vector2 scrollPos;

  
    public static void Init()
    {
        var window = (SceneWatcher)GetWindow(typeof(SceneWatcher), false, "Scene Watcher");
        window.position = new Rect(window.position.xMin + 100f, window.position.yMin + 100f, 400f, 400f);
    }

   
    void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        this.scrollPos = EditorGUILayout.BeginScrollView(this.scrollPos, false, false);

        GUILayout.Label("Scenes In Build", EditorStyles.boldLabel);
        for (var i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            var scene = EditorBuildSettings.scenes[i];
            if (scene.enabled)
            {
                var sceneName = Path.GetFileNameWithoutExtension(scene.path);
                var pressed = GUILayout.Button(i + ": " + sceneName, new GUIStyle(GUI.skin.GetStyle("Button")) { alignment = TextAnchor.MiddleLeft });
                if (pressed)
                {
                    if (EditorApplication.SaveCurrentSceneIfUserWantsTo())
                    {
                        EditorApplication.OpenScene(scene.path);
                    }
                }
            }
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }
}