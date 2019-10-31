using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class Colorizer : ScriptableWizard
    {
        private Color _color = Color.white;
        
        [MenuItem("Tools/Colorizer")]
        public static void OpenColorizer()
        {
            DisplayWizard<Colorizer>("Colorizer");
        }
        
        public void OnGUI()
        {
            _color = EditorGUILayout.ColorField(_color);
            if (GUILayout.Button("Colorize"))
            {
                GameObject activeGameObject = Selection.activeGameObject;
                if (activeGameObject)
                {
                    MeshRenderer renderer = activeGameObject.GetComponent<MeshRenderer>();
                    renderer.sharedMaterial.color = _color;
                }
            }
        }
    }
}
