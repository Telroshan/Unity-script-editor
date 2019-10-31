using UnityEditor;

namespace Editor
{
    [CustomEditor(typeof(Sample1))]
    public class Sample1Editor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
//            base.OnInspectorGUI();
            Sample1 sample1 = (Sample1) target;
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(Sample1.useFriction)));
            if (sample1.useFriction)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(Sample1.isFrictionMultiplied)));
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(Sample1.frictionFactor)));
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(Sample1.rangeFriction)));
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}