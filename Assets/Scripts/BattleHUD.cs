using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public GameObject ActionButtonPrefab = null;
    public GameObject ActionWidgetPrefab = null;

    private GameObject ActionListContent = null;
    private GameObject ActionWidget = null;

    private GameObject RootCanvas = null;

    void Start()
    {
        ActionListContent = transform.Find("Canvas/ActionList/ScrollList/Viewport/Content").gameObject;

        // Remove all children from content widget
        //for (int i=0; i< actionListContent.transform.childCount; ++i)
        //{
        //    var child = actionListContent.transform.GetChild(i);
        //    Destroy(child.gameObject);
        //}

        // Add some button for testing
        if (ActionButtonPrefab)
        {
            for (int j = 0; j < 1; j++)
            {
                GameObject newButton = Instantiate(ActionButtonPrefab) as GameObject;
                newButton.transform.SetParent(ActionListContent.transform, false);
                //newButton.GetComponent<Button>().onClick.AddListener(() => Debug.Log("TEst"));
                newButton.GetComponent<Button>().onClick.AddListener(createAction);
            }
        }

        RootCanvas = transform.GetChild(0).gameObject;
        GlobalManagers.hud = this;
    }

    void createAction()
    {
        if (ActionWidget != null)
        {
            Destroy(ActionWidget);
        }

        ActionWidget = Instantiate(ActionWidgetPrefab) as GameObject;
        ActionWidget.transform.SetParent(RootCanvas.transform, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
