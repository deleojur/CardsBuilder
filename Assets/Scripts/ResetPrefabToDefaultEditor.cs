using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ResetPrefabToDefault))]
public class ResetPrefabToDefaultEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ResetPrefabToDefault resetScript = (ResetPrefabToDefault)target;

        if (GUILayout.Button("Reset to default"))
        {
            Debug.Log("Reset");
            PrefabUtility.RevertObjectOverride(resetScript.gameObject, InteractionMode.UserAction);
        }

        base.OnInspectorGUI();
    }
}
