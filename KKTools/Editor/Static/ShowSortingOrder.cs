/**********************************************************
// Author   : K.(k79k06k02k)
// FileName : ShowSortingOrder.cs
**********************************************************/
using UnityEngine;
using UnityEditor;

public class ShowSortingOrder
{
    private const int _LableWidth = 20;
    private const int _BtnWidth = 45;
    private const string _LablePlus = "+";
    private const string _LableLess = "-";


    [InitializeOnLoadMethod]
    private static void Init()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnGUI;
    }

    private static void OnGUI(int instanceID, Rect selectionRect)
    {
        var go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

        if (go == null)
        {
            return;
        }

        Component objSpriteRenderer = go.GetComponent<SpriteRenderer>();
        Component objCanvas = go.GetComponent<Canvas>();
        Component obj = (objCanvas == null) ? objSpriteRenderer : objCanvas;

        if (obj == null)
            return;


        if (obj.GetType() == typeof(SpriteRenderer))
        {
            Show((SpriteRenderer)obj, selectionRect);
        }
        else if (obj.GetType() == typeof(Canvas))
        {
            Show((Canvas)obj, selectionRect);
        }
    }

    static void Show(SpriteRenderer objSpriteRenderer, Rect selectionRect)
    {
        var pos = selectionRect;
        pos.x = pos.xMax - _LableWidth;
        pos.width = _LableWidth;

        if (GUI.Button(pos, _LablePlus))
        {
            objSpriteRenderer.sortingOrder++;
        }

        pos.x -= _BtnWidth;
        pos.width = _BtnWidth;


        var centeredStyle = GUI.skin.GetStyle("Label");
        centeredStyle.alignment = TextAnchor.UpperCenter;
        EditorGUI.LabelField(pos, objSpriteRenderer.sortingOrder.ToString(), centeredStyle);
        EditorUtility.SetDirty(objSpriteRenderer);

        pos.x -= _LableWidth;
        pos.width = _LableWidth;

        if (GUI.Button(pos, _LableLess))
        {
            objSpriteRenderer.sortingOrder--;
        }
    }

    static void Show(Canvas objCanvas, Rect selectionRect)
    {
        var pos = selectionRect;
        pos.x = pos.xMax - _LableWidth;
        pos.width = _LableWidth;

        if (GUI.Button(pos, _LablePlus))
        {
            objCanvas.sortingOrder++;
        }

        pos.x -= _BtnWidth;
        pos.width = _BtnWidth;

        var centeredStyle = GUI.skin.GetStyle("Label");
        centeredStyle.alignment = TextAnchor.UpperCenter;
        EditorGUI.LabelField(pos, objCanvas.sortingOrder.ToString(), centeredStyle);
        EditorUtility.SetDirty(objCanvas);

        pos.x -= _LableWidth;
        pos.width = _LableWidth;

        if (GUI.Button(pos, _LableLess))
        {
            objCanvas.sortingOrder--;
        }
    }
}
