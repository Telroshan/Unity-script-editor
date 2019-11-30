using System;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

namespace ScriptEditor.Editor
{
    [CustomEditor(typeof(MonoScript))]
    public class ScriptEditor : UnityEditor.Editor
    {
        private string _scriptContent;
        private string _infoMessage;
        private MonoScript _monoScript;
        private Vector2 _scrollPos;

        private string _feedbackMessage;
        private Color _feedbackColor;
        private Color DefaultLabelColor => EditorStyles.label.normal.textColor;

        private GUIStyle _buttonStyle;

        private void OnEnable()
        {
            _monoScript = (MonoScript) target;
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
            _buttonStyle = new GUIStyle(EditorStyles.miniButton)
            {
                fixedHeight = 50f,
                alignment = TextAnchor.MiddleCenter,
                fontSize = 20,
            };

            DisplayCodeArea();
            DisplayFeedbackMessage();

            EditorGUILayout.BeginHorizontal();
            DisplaySaveButton();
            DisplayDiscardButton();
            EditorGUILayout.EndHorizontal();
        }

        private void DisplayCodeArea()
        {
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos,
                GUILayout.Width(EditorGUIUtility.currentViewWidth - 25),
                GUILayout.MaxHeight(500));
            try
            {
                _scriptContent = EditorGUILayout.TextArea(_scriptContent, GUILayout.ExpandHeight(true));
            }
            catch (Exception)
            {
                // TODO : For some reason, I get a NullReferenceException sometimes in the TextArea, even though everything is set
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
                string scriptPath = AssetDatabase.GetAssetPath(target);

                // Write new content into the script file
                File.WriteAllBytes(scriptPath, Encoding.UTF8.GetBytes(_scriptContent));

                SetFeedbackMessage("Saved script !", new Color(0.09f, 0.34f, 0.09f));

                // Refresh to force recompile
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
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
    }
}