using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CancelHandler : MonoBehaviour, ICancelHandler {
    public void OnCancel(BaseEventData eventData)
    {
        Debug.Log("CANCEL");
        //GameObject obj = this.gameObject.transform.parent.gameObject.transform.parent.gameObject;
        PlayerSetupMenuController psm = this.GetComponentInParent<PlayerSetupMenuController>();
        psm.DestroyPlayer();
    }

}
