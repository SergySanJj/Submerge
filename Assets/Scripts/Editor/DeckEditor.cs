using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(Deck))]
class DeckEditor: Editor
{

    public void OnEnable()
    {
        deckCards = serializedObject.FindProperty("deckCards");
    }

    public SerializedProperty deckCards;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DisplayList("Deck cards", deckCards, (item) => {
            var card = item.FindPropertyRelative("card");
            var deckCardType = item.FindPropertyRelative("deckCardType");

            EditorGUILayout.PropertyField(card, new GUIContent(""), GUILayout.Width(200), GUILayout.Height(20));
            bool? isTutorial = card.FindPropertyRelative("isTutorial")?.boolValue;
            if (isTutorial!=null && isTutorial.Value)
                EditorGUILayout.LabelField(new GUIContent("Tutorial"), GUILayout.Width(100), GUILayout.Height(20));
            else
                EditorGUILayout.PropertyField(deckCardType, new GUIContent(""), GUILayout.Width(100), GUILayout.Height(20));
        });

        // Write back changed values and evtl mark as dirty and handle undo/redo
        serializedObject.ApplyModifiedProperties();
    }


    private bool foldout = true;
    private delegate void ListItemRepresentation(SerializedProperty item);
    private void DisplayList(string header, SerializedProperty list, ListItemRepresentation representation)
    {
        foldout = EditorGUILayout.BeginFoldoutHeaderGroup(foldout, header);
        if (foldout)
        {
            for (int i = 0; i < list.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal("box");

                representation.Invoke(list.GetArrayElementAtIndex(i));

                if (i > 0)
                {
                    if (GUILayout.Button("^", GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        list.MoveArrayElement(i, i - 1);
                        break;
                    }
                }
                else { GUILayout.Button("", GUILayout.Width(20), GUILayout.Height(20)); }
                if (list.arraySize > 1 && i < list.arraySize - 1)
                {
                    if (GUILayout.Button("\\/", GUILayout.Width(20), GUILayout.Height(20)))
                    {
                        list.MoveArrayElement(i, i + 1);
                        break;
                    }
                }
                else { GUILayout.Button("", GUILayout.Width(20), GUILayout.Height(20)); }


                if (GUILayout.Button("X", GUILayout.Width(20), GUILayout.Height(20)))
                {
                    int oldSize = list.arraySize;
                    list.DeleteArrayElementAtIndex(i);
                    if (list.arraySize == oldSize)
                    {
                        list.DeleteArrayElementAtIndex(i);
                    }
                    break;
                }

                EditorGUILayout.EndHorizontal();
            }
            if (GUILayout.Button("+", GUILayout.Height(15), GUILayout.ExpandWidth(true)))
            {
                int n = list.arraySize;
                list.InsertArrayElementAtIndex(n);
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
    }
}
