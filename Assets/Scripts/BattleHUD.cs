using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BattleHUD : MonoBehaviour
{
    // Prefab list
    public GameObject ActionButtonPrefab = null;
    public GameObject ActionWidgetPrefab = null;

    // General purpose
    private GameObject RootCanvas = null;

    // inner widgets
    private GameObject ActionListContent = null;
    private ActionWidget ActionWidget = null;

    private void Awake()
    {
        
    }

    void Start()
    {
        // Remove all children from content widget
        //for (int i=0; i< actionListContent.transform.childCount; ++i)
        //{
        //    var child = actionListContent.transform.GetChild(i);
        //    Destroy(child.gameObject);
        //}

        ActionListContent = transform.Find("Canvas/ActionList/ScrollList/Viewport/Content").gameObject;

        RootCanvas = transform.GetChild(0).gameObject;

        var actionWidgetGO = Instantiate(ActionWidgetPrefab) as GameObject;

        ActionWidget = actionWidgetGO.GetComponent<ActionWidget>();
        actionWidgetGO.transform.SetParent(RootCanvas.transform, false);

        // Add some button for testing
        if (ActionButtonPrefab)
        {
            foreach (var action in GlobalManagers.actionManager.Actions)
            {
                GameObject prefabInst = Instantiate(ActionButtonPrefab) as GameObject;
                prefabInst.transform.SetParent(ActionListContent.transform, false);
                prefabInst.GetComponent<Button>().onClick.AddListener(() => { showAction(action); });
                prefabInst.GetComponentInChildren<Text>().text = action.Name;
            }
        }

        GlobalManagers.hud = this;
    }

    void Update()
    {
        
    }

    void showAction(ActionBase action)
    {
        ActionWidget.gameObject.SetActive(true);
        ActionWidget.setAction(action);
    }
}
