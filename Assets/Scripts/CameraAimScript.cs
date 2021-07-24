using UnityEngine;

public class CameraAimScript : MonoBehaviour
{

    public float turnSpeed;

    public Transform cameraLookAt;
    public Cinemachine.AxisState xAxis;
    public Cinemachine.AxisState yAxis;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }


    // Update is called once per frame
    void LateUpdate()
    {
        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);

        cameraLookAt.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0.0f);

        //float yawCamera = mainCamera.transform.rotation.eulerAngles.y;
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), turnSpeed * Time.fixedDeltaTime);
    }
}
