using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DropActions))]
public class DropActionsEditor: Editor
{
    private static GUIContent
        addButton = new GUIContent("+", "add");
    private static GUILayoutOption miniButtonWidth = GUILayout.Width(20f);
    public void OnEnable()
    {

    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button(addButton, EditorStyles.miniButtonRight, miniButtonWidth))
        {

        }
    }
}

