using System;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class CharacterCreator : ScriptableWizard
    {
        public string gameObjectName = "GameObject name...";
        public Texture2D portraitTexture;
        public Color color = Color.white;
        public string nickname = "Default nickname";

        [MenuItem("Tools/Create Character")]
        public static void SelectAllOfTagWizard()
        {
            DisplayWizard<CharacterCreator>("Create Character", "Create new", "Update selected");
        }

        private void OnWizardUpdate()
        {
            helpString = "Enter Character details";
        }

        private void OnWizardCreate()
        {
            GameObject gameObject = new GameObject(gameObjectName);
            Character character = gameObject.AddComponent<Character>();
            character.portrait = portraitTexture;
            character.color = color;
            character.nickname = nickname;
        }

        private void OnWizardOtherButton()
        {
            GameObject gameObject = Selection.activeGameObject;
            if (!gameObject)
            {
                helpString = "Update Fail : Please Select GameObject !";
                return;
            }
            Character character = gameObject.GetComponent<Character>();
            if (!character)
            {
                helpString = "Update Fail : Please Select GameObject !";
                return;
            }
            character.gameObject.name = gameObjectName;
            character.color = color;
            character.portrait = portraitTexture;
            character.nickname = nickname;
            helpString = "Update successful";
        }
    }
}
