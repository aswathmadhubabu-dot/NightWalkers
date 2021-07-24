using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCameraBase killCam;
    // Start is called before the first frame update
    public void EnableKillCam(){
        killCam.Priority = 100;
    }
    
}
