using UnityEngine;

public class CameraAimScript : MonoBehaviour
{
    public float rotationSpeed = 15;
    public Transform cameraLookAt;
    public Cinemachine.AxisState xAxis;
    public Cinemachine.AxisState yAxis;

    private Camera mainCamera;
    private PlayerControlScript playerControlScript;

    void Start()
    {
        mainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerControlScript = GetComponent<PlayerControlScript>();
    }


    // Update is called once per frame
    void LateUpdate()
    {
        if (playerControlScript.isAiming)
        {
            xAxis.Update(Time.deltaTime);
            yAxis.Update(Time.deltaTime);

            cameraLookAt.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0.0f);
        }
    }

    void FixedUpdate()
    {
        if (playerControlScript.isAiming)
        {
            float yaw = mainCamera.transform.rotation.eulerAngles.y;
            Quaternion rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yaw, 0), rotationSpeed * Time.fixedDeltaTime);
            rotation.x = 0;
            rotation.z = 0;
            transform.rotation = rotation;
        }

        // Reset y because sometimes player falls through the floor
        if (transform.position.y < 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
    }
}
