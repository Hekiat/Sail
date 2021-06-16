using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace sail
{
    [System.Serializable]
    public abstract class EventActionBase
    {
        public event System.Action ActionEnd;

        protected void onActionEnd()
        {
            ActionEnd?.Invoke();
        }

        public virtual void run() { }
    }

    public class DialogueEventAction : EventActionBase
    {
        [SerializeField]
        public CharacterID CharacterID = CharacterID.Invalid;

        // Add Mood / Portrait

        public List<LocalizedString> Dialogues = new List<LocalizedString>();

        public override void run()
        {
            GlobalManagers.dialogueManager.request(CharacterID, Dialogues);
            Debug.Log(Dialogues[0].GetLocalizedString());
            onActionEnd();
        }
    }

    public class ItemDropEventAction : EventActionBase
    {
        public string ItemName = string.Empty;
    }
}