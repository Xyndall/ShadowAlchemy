using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BasicMovingPlatform))]
public class BasicMovingPlatformEditor : Editor
{
    SerializedProperty localMovePoints;

    void OnEnable()
    {
        localMovePoints = serializedObject.FindProperty("localMovePoints");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawDefaultInspector();

        BasicMovingPlatform platform = (BasicMovingPlatform)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Local Move Points Tools", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Current Position"))
        {
            localMovePoints.arraySize++;
            localMovePoints.GetArrayElementAtIndex(localMovePoints.arraySize - 1).vector3Value = platform.transform.localPosition;
            serializedObject.ApplyModifiedProperties();
        }
        if (GUILayout.Button("Remove Last") && localMovePoints.arraySize > 0)
        {
            localMovePoints.arraySize--;
            serializedObject.ApplyModifiedProperties();
        }
        EditorGUILayout.EndHorizontal();
    }
}