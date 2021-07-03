using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //public GameObject player;
    public Vector3 offset;
    public List<Transform> targets;
    private Vector3 velocity;
    private float smoothTime = .3f;
    public float totalFieldSize;
    public float maxZoom;
    public float minZoom;

    public float maxOffsetY;

    public float minOffsetY;
    public float minOffsetZ;
    public float maxOffsetZ;

    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        //offset = transform.position - player.transform.position;    
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Move();
        Zoom();
    }

    float GetGreatestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return getMaxElement(bounds.size);
    }

    float getMaxElement(Vector3 v3)
    {
        return Mathf.Max(Mathf.Max(v3.x, v3.y), v3.z);
    }

    void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / totalFieldSize);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 realOffset = offset;
        //realOffset.y = Mathf.Clamp(realOffset.y, minOffsetY, maxOffsetY);
        //realOffset.z = Mathf.Clamp(realOffset.z, minOffsetZ, maxOffsetZ);
        transform.position = Vector3.SmoothDamp(transform.position, centerPoint + realOffset, ref velocity, smoothTime);
    }

    private Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].position;
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    }
}