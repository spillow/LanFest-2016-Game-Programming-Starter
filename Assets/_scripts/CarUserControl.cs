using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;

public class CarUserControl : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        // pass the input to the car!
        float h         = CrossPlatformInputManager.GetAxis("Horizontal");
        float v         = CrossPlatformInputManager.GetAxis("Vertical");
        float jump      = CrossPlatformInputManager.GetAxis("Jump");
        float boost     = CrossPlatformInputManager.GetAxis("Boost");

        //m_Car.Move(h, accel, accel, v, jump, boost, rollbrake);
    }
}
