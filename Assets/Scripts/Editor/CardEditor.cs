using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(Card))]
class CardEditor : Editor
{
    private SerializedProperty cardTags;
    private SerializedProperty neededCheckpointsToSpawn;

    private SerializedProperty person;
    private SerializedProperty cardEmotion;
    private SerializedProperty decisionQuestion;
    private SerializedProperty answers;
    private SerializedProperty actionsOnCardSpawn;

    private SerializedProperty leftSideActions;
    private SerializedProperty rightSideActions;

    private SerializedProperty isTutorial;

    private SerializedProperty cardInfoColor;

    public void OnEnable()
    {
        cardTags = serializedObject.FindProperty("cardTags");
        neededCheckpointsToSpawn = serializedObject.FindProperty("neededCheckpointsToSpawn");

        person = serializedObject.FindProperty("person");
        cardEmotion = serializedObject.FindProperty("cardEmotion");
        decisionQuestion = serializedObject.FindProperty("decisionQuestion");
        answers = serializedObject.FindProperty("answers");
        actionsOnCardSpawn = serializedObject.FindProperty("actionsOnCardSpawn");

        leftSideActions = serializedObject.FindProperty("leftSideActions");
        rightSideActions = serializedObject.FindProperty("rightSideActions");

        isTutorial = serializedObject.FindProperty("isTutorial");

        cardInfoColor = serializedObject.FindProperty("cardInfoColor");
    }

    public override void OnInspectorGUI()
    {
        // Load the real class values into the serialized copy
        serializedObject.Update();

        DisplayList("Tags", cardTags, (item) => {
            item.objectReferenceValue =
                    (CardTag)EditorGUILayout.ObjectField("", item.objectReferenceValue, typeof(CardTag), false,
                    GUILayout.Height(20), GUILayout.ExpandWidth(true));
        });

        EditorGUILayout.PropertyField(isTutorial);

        EditorGUILayout.PropertyField(person);
        EditorGUILayout.PropertyField(cardEmotion);
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Decision Question");
        EditorGUILayout.PropertyField(decisionQuestion, new GUIContent(""));
        EditorGUILayout.EndVertical();
        EditorGUILayout.PropertyField(answers);
        EditorGUILayout.PropertyField(actionsOnCardSpawn);

        DisplayList("Checkpoints needed to spawn", neededCheckpointsToSpawn, (item) => {
            item.objectReferenceValue =
                    (Checkpoint)EditorGUILayout.ObjectField("", item.objectReferenceValue, typeof(Checkpoint), false,
                    GUILayout.Height(20), GUILayout.ExpandWidth(true));
        });
        
     

        string leftAnswer = answers.GetArrayElementAtIndex(0).stringValue;
        string rightAnswer = answers.GetArrayElementAtIndex(1).stringValue;

        GUI.color = Color.red;
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Left - " + leftAnswer);
        EditorGUILayout.EndVertical();
        GUI.color = Color.white;
        ViewSection(leftSideActions);
        GUI.color = Color.green;
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Right - " + rightAnswer);
        EditorGUILayout.EndVertical();
        GUI.color = Color.white;
        ViewSection(rightSideActions);

        EditorGUILayout.PropertyField(cardInfoColor);

        // Write back changed values and evtl mark as dirty and handle undo/redo
        serializedObject.ApplyModifiedProperties();
    }

    private bool foldout = true;

    private void ViewSection(SerializedProperty dropActions)
    {
        SerializedProperty dayAmount = dropActions.FindPropertyRelative("dayAmount");
        EditorGUILayout.PropertyField(dayAmount);

        DisplayList("Add checkpoints", dropActions.FindPropertyRelative("passCheckpoints"), (item) => {
            EditorGUILayout.PropertyField(item, new GUIContent(""), GUILayout.ExpandWidth(true), GUILayout.Height(20));
        });

        DisplayList("Static actions", dropActions.FindPropertyRelative("staticActions"), (item) => {
            item.objectReferenceValue =
                    (CardAction)EditorGUILayout.ObjectField("", item.objectReferenceValue, typeof(CardAction), false,
                    GUILayout.Height(20), GUILayout.ExpandWidth(true));
        });

        DisplayList("Shuffle Actions", dropActions.FindPropertyRelative("shuffleActions"), (item) => {
            var card = item.FindPropertyRelative("cardToShuffle");
            EditorGUILayout.PropertyField(card, new GUIContent(""), GUILayout.ExpandWidth(true), GUILayout.Height(20));
            var shufflePlace = item.FindPropertyRelative("shufflePlace");
            EditorGUILayout.PropertyField(shufflePlace, new GUIContent(""), GUILayout.Width(90), GUILayout.Height(20));
        });

        DisplayList("Game State Actions", dropActions.FindPropertyRelative("gameStateChangeActions"), (item) => {
            var changerType = item.FindPropertyRelative("changerType");
            EditorGUILayout.PropertyField(changerType, new GUIContent(""), GUILayout.Width(80), GUILayout.Height(20));
            var parameterToChange = item.FindPropertyRelative("parameterToChange");
            EditorGUILayout.PropertyField(parameterToChange, new GUIContent(""), GUILayout.ExpandWidth(true), GUILayout.Height(20));
            var value = item.FindPropertyRelative("value");
            EditorGUILayout.PropertyField(value, new GUIContent(""), GUILayout.Width(80), GUILayout.Height(20));
        });

        DisplayList("Remove Cards with tags", dropActions.FindPropertyRelative("removeCardsWithTags"), (item) => {
            item.objectReferenceValue =
                    (CardTag)EditorGUILayout.ObjectField("", item.objectReferenceValue, typeof(CardTag), false,
                    GUILayout.Height(20), GUILayout.ExpandWidth(true));
        });
    }

    private delegate void ListItemRepresentation(SerializedProperty item);
    private void DisplayList( string header, SerializedProperty list, ListItemRepresentation representation)
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
