using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(Scaler))]
    public class ScalerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            Scaler scaler = (Scaler) target;
            EditorGUILayout.LabelField("Oscillates around a base size");
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(Scaler.size)));
            scaler.transform.localScale = new Vector3(scaler.size, scaler.size, scaler.size);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
