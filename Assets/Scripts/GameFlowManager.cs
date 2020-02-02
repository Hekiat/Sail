using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{

    public static GameFlowManager instance = null;

    public GameObject HUDPrefab = null;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if(HUDPrefab)
        {
            Instantiate(HUDPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }

    
    void Update()
    {
        
    }
}
