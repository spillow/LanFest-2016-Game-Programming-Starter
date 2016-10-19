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
        // TODO

        // Uncomment after defining variables
        //m_Car.Move(steer, accel, jump, boost, roll);
    }
}
