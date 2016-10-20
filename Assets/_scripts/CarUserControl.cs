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

    void FixedUpdate()
    {
        // pass the input to the car!
        float steer = CrossPlatformInputManager.GetAxis("Horizontal");
        float accel = CrossPlatformInputManager.GetAxis("Vertical");
        float jump = CrossPlatformInputManager.GetAxis("Jump");
        float boost = CrossPlatformInputManager.GetAxis("Boost");
        float roll = CrossPlatformInputManager.GetAxis("AirRollBrake");

        m_Car.Move(steer, accel, jump, boost, roll);
    }
}
