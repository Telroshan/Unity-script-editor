using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

namespace ScriptEditor.Editor
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class ScriptEditor : UnityEditor.Editor
    {
        private string _scriptContent;
        private string _infoMessage;
        private MonoScript _monoScript;
        private bool _displayed;
        private Vector2 _scrollPos;

        private string _feedbackMessage;
        private Color _feedbackColor;
        private Color DefaultLabelColor => EditorStyles.label.normal.textColor;

        private void OnEnable()
        {
            MonoBehaviour script = (MonoBehaviour) target;
            _monoScript = MonoScript.FromMonoBehaviour(script);
            _scriptContent = _monoScript.text;
        }

        private GUIContent GetButtonGuiContent(string iconName, string text, string tooltip)
        {
            GUIContent guiContent = new GUIContent(EditorGUIUtility.IconContent(iconName))
                {text = text, tooltip = tooltip};
            return guiContent;
        }

        private void SetFeedbackMessage(string value, Color color)
        {
            _feedbackMessage = value;
            _feedbackColor = color;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUIStyle editButtonStyle = new GUIStyle(EditorStyles.miniButton)
            {
                fixedHeight = 50f,
                alignment = TextAnchor.MiddleCenter,
                fontSize = 20,
            };
            if (!_displayed &&
                GUILayout.Button(
                    GetButtonGuiContent("d_editicon.sml", "Edit",
                        "Click here to be able to edit your script directly in this inspector"), editButtonStyle))
            {
                SetFeedbackMessage(null, DefaultLabelColor);
                _displayed = true;
            }

            if (!_displayed) return;

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos,
                GUILayout.Width(EditorGUIUtility.currentViewWidth - 25),
                GUILayout.MaxHeight(300));
            _scriptContent = EditorGUILayout.TextArea(_scriptContent, GUILayout.ExpandHeight(true));
            EditorGUILayout.EndScrollView();

            EditorGUILayout.LabelField(_feedbackMessage,
                new GUIStyle(EditorStyles.label) {wordWrap = true, normal = {textColor = _feedbackColor}});

            GUIStyle buttonStyle = new GUIStyle(EditorStyles.miniButton) {fixedHeight = 30f, fontSize = 11};

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(GetButtonGuiContent("SaveActive", "Save", "Apply your changes to the script"),
                buttonStyle))
            {
                string guid = AssetDatabase.FindAssets(target.GetType().Name).FirstOrDefault();
                string path = AssetDatabase.GUIDToAssetPath(guid);
                File.WriteAllBytes(path, Encoding.UTF8.GetBytes(_scriptContent));
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                Application.logMessageReceived += (condition, trace, type) =>
                {
                    switch (type)
                    {
                        case LogType.Error:
                            SetFeedbackMessage("There are some compile errors ! (" + condition + ")",
                                new Color(0.45f, 0.04f, 0.05f));
                            break;
                        default:
                            SetFeedbackMessage("Saved script !", new Color(0.11f, 0.45f, 0.11f));
                            break;
                    }
                };
            }

            if (GUILayout.Button(GetButtonGuiContent("d_TreeEditor.Trash", "Discard",
                "Reverts your changes back to the original script"), buttonStyle))
            {
                _scriptContent = _monoScript.text;
                EditorGUI.FocusTextInControl(null);
                SetFeedbackMessage("Changes discarded", DefaultLabelColor);
            }

            if (GUILayout.Button(GetButtonGuiContent("ViewToolZoom", "Select",
                "Highlight the script in the project window"), buttonStyle))
            {
                EditorGUIUtility.PingObject(_monoScript);
                SetFeedbackMessage("Script Highlighted", DefaultLabelColor);
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}