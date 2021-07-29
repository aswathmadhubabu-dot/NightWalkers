using UnityEngine;

public class CameraAimScript : MonoBehaviour
{
    public float rotationSpeed = 15;
    public Transform cameraLookAt;
    public Cinemachine.AxisState xAxis;
    public Cinemachine.AxisState yAxis;

    private Camera mainCamera;
    private PlayerControlScript playerControlScript;
    private float mousePrevX = 0 ;
    private float mousePrevY = 0;
    
    void Start()
    {
        mainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerControlScript = GetComponent<PlayerControlScript>();
    }


    // Update is called once per frame
    void Update()
    {
        /*
        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);

        cameraLookAt.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0.0f);
        */
        float mouseX = Input.mousePosition.x;
		float mouseY = Input.mousePosition.y;
        
        float deltaX = mouseX - mousePrevX;
        float deltaY = mouseY - mousePrevY;
         
        if (playerControlScript.isAiming){
            /*Debug.Log(" MOUS " + mouseX + " " + mouseY);
            Vector3 target = Camera.main.ScreenToWorldPoint(new Vector3(mouseY, mouseX, 5.0f));
            Debug.Log(" TARGET " + target );
            cameraLookAt.eulerAngles = target;
            */

            Vector2 v2 = Input.mouseScrollDelta;
            //Debug.Log(" MOUS " + deltaX + " ");
            //cameraLookAt.eulerAngles = Quaternion.Euler(0.0f, deltaX, 0.0f) * cameraLookAt.eulerAngles; 
            if(deltaX != 0){
                cameraLookAt.Rotate(new Vector3(0.0f,  deltaX * 0.05f, 0.0f));
                transform.Rotate(new Vector3(0.0f,  deltaX * 0.08f, 0.0f));
            }
        } else
        {
            cameraLookAt.rotation = transform.rotation;
        }
        mousePrevX = mouseX;
        mousePrevY = mouseY;

    }

    void FixedUpdate()
    {
        /*
        if (playerControlScript.isAiming)
        {
            float yaw = mainCamera.transform.rotation.eulerAngles.y;
            Quaternion rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yaw, 0), rotationSpeed * Time.fixedDeltaTime);
            rotation.x = 0;
            rotation.z = 0;

            transform.Rotate(new Vector3(0.0f,  deltaX * 0.01f, 0.0f));
        }
        */
    }
}
