using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public float level;
    void OnTriggerEnter(){
        Destroy(GameObject.Find("TimerUI"));
        //TODO: Stop actual game time
        //TODO: Play dance animation
        Time.timeScale = 0;
        switch(level){

            case 1:
                GameObject dm = GameObject.Find("NextLevelMenuContainer").transform.GetChild(0).gameObject;
                dm.SetActive(true);
            break;
            case 2:
                GameObject dm2 = GameObject.Find("NextLevelMenuContainer").transform.GetChild(0).gameObject;
                dm2.SetActive(true);
            break;
            case 3:
                GameObject dm3 = GameObject.Find("YouWonMenuContainer").transform.GetChild(0).gameObject;
                dm3.SetActive(true);
            break;
            
        }

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
