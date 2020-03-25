using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sail
{
    public class ActionExecutionController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
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