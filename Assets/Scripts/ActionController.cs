using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace sail
{
    public class ActionController : MonoBehaviour
    {

        public event EventHandler OnActionEnded = delegate { };

        private ActionBase RunningAction = null;

        void Start()
        {
        
        }

        void Update()
        {
        
        }

        public void requestAction(ActionBase action, List<ActionBase> secondaryActions)
        {
            RunningAction = action;
            RunningAction.start(secondaryActions);
            
            StartCoroutine(runAction());
        }

        IEnumerator runAction()
        {
            yield return RunningAction.run();
            OnActionEnded(RunningAction, EventArgs.Empty);
            RunningAction = null;
        }

        
    }
}