using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameSettings))]
public class GameSettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Add a button for applying the changes
        GUILayout.Space(10);
        if (GUILayout.Button("Apply", GUILayout.Height(50)))
        {
            GameSettings settings = (GameSettings)target;

            settings.NotifySettingsChanged();
        }
        GUILayout.Space(10);

        DrawDefaultInspector();
    }
}