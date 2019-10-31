using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class TagWizard : ScriptableWizard
    {
        private string _tag = "Your tag here...";

        [MenuItem("Tools/Select All Of Tag Wizard")]
        public static void SelectAllOfTagWizard()
        {
            TagWizard tagWizard = DisplayWizard<TagWizard>("Tag Wizard");
            tagWizard.minSize = new Vector2(300f, 200f);
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Search tag");
            _tag = EditorGUILayout.TextField(_tag);
            _tag = EditorGUILayout.TagField(_tag);
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