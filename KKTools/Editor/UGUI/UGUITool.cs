/**********************************************************
// Author   : K.(k79k06k02k)
// FileName : UGUITool.cs
**********************************************************/
using UnityEngine;
using UnityEditor;
using System.Text;

public class UGUITool : EditorWindow
{
    private string[] types = new string[]
    {
        "Anchors to Corners",
        "Corners to Anchors",
        "Mirror Horizontally Around Anchors",
        "Mirror Horizontally Around Parent Center",
        "Mirror Vertically Around Anchors",
        "Mirror Vertically Around Parent Center",
    };

    private Vector2 scrollPos;
    private GUIStyle BtnStyle;

    public static void ShowWindow()
    {
        EditorWindow editorWindow = GetWindow(typeof(UGUITool));
        editorWindow.position = new Rect(editorWindow.position.xMin + 100f, editorWindow.position.yMin + 100f, 400f, 190f);
        editorWindow.autoRepaintOnSceneChange = true;
        editorWindow.Show();
        editorWindow.titleContent = new GUIContent("UGUI Tool");
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
        if (Selection.activeGameObject == null || Selection.activeGameObject.GetComponent<RectTransform>() == null)
        {
            EditorGUILayout.HelpBox(GetHelpString(), MessageType.Warning);
            return;
        }


        BtnStyle = new GUIStyle(GUI.skin.button);
        BtnStyle.fontSize = 16;
        BtnStyle.alignment = TextAnchor.MiddleCenter;

        EditorGUILayout.LabelField("Features：", EditorStyles.boldLabel);

        EditorGUILayout.BeginVertical();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);

        for (int i = 0; i < types.Length; i++)
            if (GUILayout.Button(types[i], BtnStyle))
                OnBtnClick(i);


        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    string GetHelpString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("No Source." + "\n\n");
        sb.Append("Please Check Select GameObject：" + "\n");
        sb.Append("1.GameObject is not Null" + "\n");
        sb.Append("2.GameObject is UI (Have RectTransform Component)");

        return sb.ToString();
    }

    void OnBtnClick(int index)
    {
        switch (index)
        {
            case 0:
                AnchorsToCorners();
                break;

            case 1:
                CornersToAnchors();
                break;

            case 2:
                MirrorHorizontallyAnchors();
                break;

            case 3:
                MirrorHorizontallyParent();
                break;

            case 4:
                MirrorVerticallyAnchors();
                break;

            case 5:
                MirrorVerticallyParent();
                break;
        }
    }


    void AnchorsToCorners()
    {
        foreach (Transform transform in Selection.transforms)
        {
            RectTransform t = transform as RectTransform;
            RectTransform pt = Selection.activeTransform.parent as RectTransform;

            if (t == null || pt == null) return;

            Vector2 newAnchorsMin = new Vector2(t.anchorMin.x + t.offsetMin.x / pt.rect.width,
                                                t.anchorMin.y + t.offsetMin.y / pt.rect.height);
            Vector2 newAnchorsMax = new Vector2(t.anchorMax.x + t.offsetMax.x / pt.rect.width,
                                                t.anchorMax.y + t.offsetMax.y / pt.rect.height);

            t.anchorMin = newAnchorsMin;
            t.anchorMax = newAnchorsMax;
            t.offsetMin = t.offsetMax = new Vector2(0, 0);
        }
    }

    void CornersToAnchors()
    {
        foreach (Transform transform in Selection.transforms)
        {
            RectTransform t = transform as RectTransform;

            if (t == null) return;

            t.offsetMin = t.offsetMax = new Vector2(0, 0);
        }
    }

    void MirrorHorizontallyAnchors()
    {
        MirrorHorizontally(false);
    }

    void MirrorHorizontallyParent()
    {
        MirrorHorizontally(true);
    }

    void MirrorHorizontally(bool mirrorAnchors)
    {
        foreach (Transform transform in Selection.transforms)
        {
            RectTransform t = transform as RectTransform;
            RectTransform pt = Selection.activeTransform.parent as RectTransform;

            if (t == null || pt == null) return;

            if (mirrorAnchors)
            {
                Vector2 oldAnchorMin = t.anchorMin;
                t.anchorMin = new Vector2(1 - t.anchorMax.x, t.anchorMin.y);
                t.anchorMax = new Vector2(1 - oldAnchorMin.x, t.anchorMax.y);
            }

            Vector2 oldOffsetMin = t.offsetMin;
            t.offsetMin = new Vector2(-t.offsetMax.x, t.offsetMin.y);
            t.offsetMax = new Vector2(-oldOffsetMin.x, t.offsetMax.y);

            t.localScale = new Vector3(-t.localScale.x, t.localScale.y, t.localScale.z);
        }
    }

    void MirrorVerticallyAnchors()
    {
        MirrorVertically(false);
    }

    void MirrorVerticallyParent()
    {
        MirrorVertically(true);
    }

    void MirrorVertically(bool mirrorAnchors)
    {
        foreach (Transform transform in Selection.transforms)
        {
            RectTransform t = transform as RectTransform;
            RectTransform pt = Selection.activeTransform.parent as RectTransform;

            if (t == null || pt == null) return;

            if (mirrorAnchors)
            {
                Vector2 oldAnchorMin = t.anchorMin;
                t.anchorMin = new Vector2(t.anchorMin.x, 1 - t.anchorMax.y);
                t.anchorMax = new Vector2(t.anchorMax.x, 1 - oldAnchorMin.y);
            }

            Vector2 oldOffsetMin = t.offsetMin;
            t.offsetMin = new Vector2(t.offsetMin.x, -t.offsetMax.y);
            t.offsetMax = new Vector2(t.offsetMax.x, -oldOffsetMin.y);

            t.localScale = new Vector3(t.localScale.x, -t.localScale.y, t.localScale.z);
        }
    }
}
