using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace sail
{
    public class ActionController : MonoBehaviour
    {
        public event EventHandler OnActionEnded;

        public ActionBase Action { get; private set; } = null;
        private List<ActionBase> SecondaryActions { get; set; } = new List<ActionBase>();

        public bool Running { get; private set; } = false;

        void Start()
        {
        
        }

        void Update()
        {
        
        }

        void LateUpdate()
        {
            if (Action == null || Running == false)
            {
                return;
            }
            
            Action.run();

            if (Action.ActionEnded == true)
            {
                OnActionEnded(Action, EventArgs.Empty);
                clear();
            }
        }

        public void setup(ActionBase action, List<ActionBase> secondaryActions)
        {
            Action = action;
            SecondaryActions = new List<ActionBase>(secondaryActions);

            if (Action != null)
            {
                Action.setup(SecondaryActions);
            }
        }

        public void request()
        {
            Running = true;
            Action.start();
            //StartCoroutine(runAction());
        }

        IEnumerator runAction()
        {
            Debug.Log("start action controller");
            //Action.start();
            while(Action.ActionEnded != true)
            {
                Action.run();
                yield return null;
            }
            OnActionEnded(Action, EventArgs.Empty);
            clear();
        }

        void clear()
        {
            if (Action != null)
            {
                Action.setup(new List<ActionBase>());
            }

            Action = null;
            SecondaryActions.Clear();
            Running = false;
        }
    }
}