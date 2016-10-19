using UnityEngine;
using System.Collections;
using UnityStandardAssets.Vehicles.Car;

public class CameraControl : MonoBehaviour
{
    public Transform Car;
    public CarController Controller;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Car.position;

        if (Controller.InContactWithSurface())
        {
            Vector3 forward = Car.forward;
            forward.y = 0f;
            Quaternion rotation = Quaternion.LookRotation(forward, Vector3.up);
            transform.rotation = rotation;
        }
    }
}
