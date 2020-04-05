﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace sail
{
    public class ActionController : MonoBehaviour
    {
        public event EventHandler OnActionEnded;

        private ActionBase Action { get; set; } = null;
        private List<ActionBase> SecondaryActions { get; set; } = new List<ActionBase>();

        void Start()
        {
        
        }

        void Update()
        {
        
        }

        public void setup(ActionBase action, List<ActionBase> secondaryActions)
        {
            Action = action;
            SecondaryActions = new List<ActionBase>(secondaryActions);

            Action.setup(SecondaryActions);
        }

        public void setupSelection()
        {
            Action.setupSelection();
        }

        public void request()
        {
            StartCoroutine(runAction());
        }

        IEnumerator runAction()
        {
            yield return Action.run();
            OnActionEnded(Action, EventArgs.Empty);
            clear();
        }

        void clear()
        {
            Action = null;
            SecondaryActions.Clear();
        }
    }
}