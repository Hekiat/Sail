using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class ActionController : MonoBehaviour
    {
        void Start()
        {
        
        }

        void Update()
        {
        
        }

        public void requestAction(ActionBase action, List<ActionBase> secondaryActions)
        {
            action.start(secondaryActions);

            StartCoroutine(action.run());
        }

        
    }
}