using System;
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

        private GUIStyle _buttonStyle;
        private GUIStyle _editButtonStyle;

        private void OnEnable()
        {
            // Retrieve script's content
            MonoBehaviour script = (MonoBehaviour) target;
            _monoScript = MonoScript.FromMonoBehaviour(script);
            _scriptContent = _monoScript.text;
        }

        private GUIContent GetButtonGuiContent(string iconName, string text, string tooltip)
        {
            GUIContent guiContent = new GUIContent(EditorGUIUtility.IconContent(iconName))
            {
                text = text,
                tooltip = tooltip,
            };
            return guiContent;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SetupStyles();

            DisplayEditButton();

            if (!_displayed) return;

            DisplayCodeArea();
            DisplayFeedbackMessage();

            EditorGUILayout.BeginHorizontal();
            DisplaySaveButton();
            DisplayDiscardButton();
            DisplayHighlightButton();
            EditorGUILayout.EndHorizontal();
        }

        private void SetupStyles()
        {
            _editButtonStyle = new GUIStyle(EditorStyles.miniButton)
            {
                fixedHeight = 50f,
                alignment = TextAnchor.MiddleCenter,
                fontSize = 20,
            };
            _buttonStyle = new GUIStyle(EditorStyles.miniButton)
            {
                fixedHeight = 30f,
                fontSize = 11,
            };
        }

        private void DisplayEditButton()
        {
            if (!_displayed &&
                GUILayout.Button(
                    GetButtonGuiContent("d_editicon.sml", "Edit",
                        "Click here to be able to edit your script directly in this inspector"), _editButtonStyle))
            {
                SetFeedbackMessage(null, DefaultLabelColor);
                _displayed = true;
            }
        }

        private void DisplayCodeArea()
        {
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos,
                GUILayout.Width(EditorGUIUtility.currentViewWidth - 25),
                GUILayout.MaxHeight(300));
            try
            {
                _scriptContent = EditorGUILayout.TextArea(_scriptContent, GUILayout.ExpandHeight(true));
            }
            catch (Exception e)
            {
                // TODO : For some reason, I get a NullReferenceException sometimes in the TextArea, even though all is set
                // ignored
            }

            EditorGUILayout.EndScrollView();
        }

        private void SetFeedbackMessage(string value, Color color)
        {
            _feedbackMessage = value;
            _feedbackColor = color;
        }

        private void DisplayFeedbackMessage()
        {
            EditorGUILayout.LabelField(_feedbackMessage,
                new GUIStyle(EditorStyles.label) {wordWrap = true, normal = {textColor = _feedbackColor}});
        }

        private void DisplaySaveButton()
        {
            if (GUILayout.Button(GetButtonGuiContent("SaveActive", "Save", "Apply your changes to the script"),
                _buttonStyle))
            {
                string scriptGuid = AssetDatabase.FindAssets(target.GetType().Name).FirstOrDefault();
                string scriptPath = AssetDatabase.GUIDToAssetPath(scriptGuid);

                // Write new content into the script file
                File.WriteAllBytes(scriptPath, Encoding.UTF8.GetBytes(_scriptContent));

                // Refresh to force recompile
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

                // Intercept logs to display a feedback
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
        }

        private void DisplayDiscardButton()
        {
            if (GUILayout.Button(GetButtonGuiContent("d_TreeEditor.Trash", "Discard",
                "Reverts your changes back to the original script"), _buttonStyle))
            {
                // Get back to the original content
                _scriptContent = _monoScript.text;

                // Focus out the text area
                EditorGUI.FocusTextInControl(null);

                SetFeedbackMessage("Changes discarded", DefaultLabelColor);
            }
        }

        private void DisplayHighlightButton()
        {
            if (GUILayout.Button(GetButtonGuiContent("ViewToolZoom", "Select",
                "Highlight the script in the project window"), _buttonStyle))
            {
                EditorGUIUtility.PingObject(_monoScript);

                SetFeedbackMessage("Script Highlighted", DefaultLabelColor);
            }
        }
    }
}