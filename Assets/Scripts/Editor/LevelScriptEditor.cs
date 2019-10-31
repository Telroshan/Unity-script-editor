using UnityEditor;

namespace Editor
{
    [CustomEditor(typeof(LevelScript))]
    public class LevelScriptEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            LevelScript level = (LevelScript) target;
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(LevelScript.experience)));
            EditorGUILayout.LabelField("Level", level.Level.ToString());
            serializedObject.ApplyModifiedProperties();
        }
    }
}
