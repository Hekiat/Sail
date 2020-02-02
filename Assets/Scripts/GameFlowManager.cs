using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFlowManager : MonoBehaviour
{

    public static GameFlowManager instance = null;

    public GameObject HUDPrefab = null;
    public GameObject ActionButtonPrefab = null;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if(HUDPrefab)
        {
            var hud = Instantiate(HUDPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            var actionListContent = hud.transform.Find("Canvas/ActionList/ScrollList/Viewport/Content");          

            // Remove all children from content widget
            //for (int i=0; i< actionListContent.transform.childCount; ++i)
            //{
            //    var child = actionListContent.transform.GetChild(i);
            //    Destroy(child.gameObject);
            //}

            // Add some button for testing
            if (ActionButtonPrefab)
            {
                for (int j = 0; j < 10; j++)
                {
                    GameObject newButton = Instantiate(ActionButtonPrefab) as GameObject;
                    newButton.transform.SetParent(actionListContent.transform, false);
                    newButton.GetComponent<Button>().onClick.AddListener(() => Debug.Log("TEst"));
                }
            }
        }
    }

    
    void Update()
    {
        
    }
}
