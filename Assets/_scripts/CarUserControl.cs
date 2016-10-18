using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;
using UnityStandardAssets.Vehicles.Car;

public class CarUserControl : MonoBehaviour
{
    private CarController m_Car;

    // Use this for initialization
    void Start()
    {
        m_Car = GetComponent<CarController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        // pass the input to the car!
        float steer  = CrossPlatformInputManager.GetAxis("Horizontal");
        float accel  = CrossPlatformInputManager.GetAxis("Vertical");
        float jump   = CrossPlatformInputManager.GetAxis("Jump");
        float boost  = CrossPlatformInputManager.GetAxis("Boost");

        m_Car.Move(steer, accel, jump, boost);
    }
}
