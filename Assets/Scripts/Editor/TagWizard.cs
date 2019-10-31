using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class TagWizard : ScriptableWizard
    {
        private string _tag;

        [MenuItem("Tools/Select All Of Tag Wizard")]
        public static void SelectAllOfTagWizard()
        {
            DisplayWizard<TagWizard>("Tag Wizard");
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Search tag");
            _tag = EditorGUILayout.TextField(_tag);
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Make selection"))
            {
                if (string.IsNullOrEmpty(_tag))
                {
                    Debug.LogWarning("Null tag");
                }
                else
                {
                    Selection.objects = FindObjectsOfType<GameObject>()
                        .Where(x => x.CompareTag(_tag))
                        .Select(x => (Object) x).ToArray();
                }
            }
        }
    }
}