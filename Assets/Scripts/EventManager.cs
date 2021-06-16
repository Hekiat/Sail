using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class EventManager : MonoBehaviour
    {
        public EventData Data = null;

        public bool Test = false;

        int CurrentEventActionIndex = -1;

        void Awake()
        {
            GlobalManagers.eventManager = this;
        }

        void Update()
        {
            if(Test)
            {
                startEvent(Data);
                Test = false;
            }

            if(Data == null || CurrentEventActionIndex == -1)
            {
                return;
            }

            Data.Actions[CurrentEventActionIndex].run();
        }

        public void startEvent(EventData data)
        {
            Data = data;
            CurrentEventActionIndex = -1;
            startNextAction();
        }

        void startNextAction()
        {
            CurrentEventActionIndex++;

            if(CurrentEventActionIndex >= Data.Actions.Count)
            {
                //Data = null;
                CurrentEventActionIndex = -1;
                return;
            }

            Data.Actions[CurrentEventActionIndex].ActionEnd += onActionEnded;
        }

        void onActionEnded()
        {
            Data.Actions[CurrentEventActionIndex].ActionEnd -= onActionEnded;
            startNextAction();
        }
    }
}