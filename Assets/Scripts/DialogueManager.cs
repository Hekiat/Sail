using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEditor;


namespace sail
{
    public class DialogueManager : MonoBehaviour
    {
        private DialogueWidget _Widget = null;

        private void Awake()
        {
            GlobalManagers.dialogueManager = this;
        }

        void Start()
        {

        }

        void Update()
        {

        }

        public void registerWidget(DialogueWidget widget)
        {
            _Widget = widget;
        }

        public void request(CharacterID name, List<LocalizedString> text)
        {
            _Widget.request(name, text);
        }
    }

    [CustomEditor(typeof(DialogueManager))]
    public class DialogueManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DialogueManager myScript = (DialogueManager)target;

            if (GUILayout.Button("Test"))
            {
                var text = new List<string>();
                text.Add("Test");
                text.Add("");
                //localization.Dialogues.CH_000_NAME
                myScript.request(CharacterID.ch000, new List<LocalizedString>() { localization.Dialogues.ID_0 });
            }
        }
    }
}