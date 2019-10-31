using System;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;
using Object = UnityEngine.Object;

namespace ScriptEditor.Editor
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class ScriptEditor : UnityEditor.Editor
    {
        private string _scriptContent;
        private string _infoMessage;
        private MonoScript _monoScript;
        private bool _displayed;

        private void OnEnable()
        {
            MonoBehaviour script = (MonoBehaviour) target;
            _monoScript = MonoScript.FromMonoBehaviour(script);
            _scriptContent = _monoScript.text;
        }

        private GUIContent GetButtonGuiContent(string iconName, string text)
        {
            GUIContent guiContent = new GUIContent(EditorGUIUtility.IconContent(iconName)) {text = text};
            return guiContent;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUIStyle buttonStyle = new GUIStyle(EditorStyles.miniButton)
            {
                fixedHeight = 50f,
                alignment = TextAnchor.MiddleCenter,
                fontSize = 20,
            };
            if (!_displayed && GUILayout.Button(GetButtonGuiContent("d_editicon.sml", "Edit"), buttonStyle))
            {
                _displayed = true;
            }

            if (!_displayed) return;

            _scriptContent = EditorGUILayout.TextArea(_scriptContent);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(GetButtonGuiContent("SaveActive", "Save")))
            {
                string guid = AssetDatabase.FindAssets(target.GetType().Name).FirstOrDefault();
                string path = AssetDatabase.GUIDToAssetPath(guid);
                File.WriteAllBytes(path, Encoding.UTF8.GetBytes(_scriptContent));
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            }

            if (GUILayout.Button(GetButtonGuiContent("d_TreeEditor.Trash", "Discard")))
            {
                _scriptContent = _monoScript.text;
                EditorGUI.FocusTextInControl(null);
            }

            if (GUILayout.Button(GetButtonGuiContent("ViewToolZoom", "Select")))
            {
//                Selection.objects = new[] {(Object) _monoScript};
                EditorGUIUtility.PingObject(_monoScript);
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}