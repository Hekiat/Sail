using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;

namespace sail
{
    [CreateAssetMenu(fileName = "Event0000", menuName = "Custom/EventData", order = 1)]
    public class EventData : ScriptableObject
    {
        public EventID ID = EventID.Invalid;

        [SerializeReference]
        public List<EventActionBase> Actions = new List<EventActionBase>() { new DialogueEventAction() };
    }

    [CustomEditor(typeof(EventData))]
    public class EventDataEditor : UnityEditor.Editor
    {
        private System.Type[] ActionTypes = null;
        private int SelectedIndex = 0;

        private void OnEnable()
        {
            System.Type[] types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            ActionTypes = (from System.Type type in types where type.IsSubclassOf(typeof(EventActionBase)) select type).ToArray();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            //serializedObject.Update();

            string[] names = ActionTypes.Select(o => o.Name).ToArray();

            GUILayout.BeginVertical("Add Action:", "window");

            SelectedIndex = EditorGUILayout.Popup("Action Type:", SelectedIndex, names);

            if(GUILayout.Button("Add Action"))
            {
                var eventAction = System.Activator.CreateInstance(ActionTypes[SelectedIndex]) as EventActionBase;
                var inst = target as EventData;
                inst.Actions.Add(eventAction);
            }

            
            GUILayout.EndVertical();
        }
    }
}