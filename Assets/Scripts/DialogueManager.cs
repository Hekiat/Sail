using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        public void request(string name, List<string> text)
        {
            var data = new DialogueData();
            data.CharacterName = name;
            data.Dialogues = new Queue<string>(text);
            _Widget.request(data);
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
                text.Add("Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo.");
                myScript.request("Name", text);
            }
        }
    }
}