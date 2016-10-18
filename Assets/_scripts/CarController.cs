using System;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Car
{
    public class CarController : MonoBehaviour
    {
        public WheelCollider[] m_WheelColliders = new WheelCollider[4];
        public GameObject[] m_WheelMeshes = new GameObject[4];

        public float MaxSteerAngle = 30.0f;
        public float DriveForce = 10000f;
        public float BoostForce = 100000f;

        private Rigidbody m_Rigidbody;

        void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }

        public float BrakeInput = 0.0f;
        public float CurrentSteerAngle = 0.0f;
        public float DownForce = 1000f;

        private void ControlWheels(float steer)
        {
            for (int i = 0; i < 4; i++)
            {
                Quaternion quat;
                Vector3 position;
                m_WheelColliders[i].GetWorldPose(out position, out quat);
                m_WheelMeshes[i].transform.position = position;
                m_WheelMeshes[i].transform.rotation = quat;
            }

            float angle = steer * MaxSteerAngle;
            m_WheelColliders[0].steerAngle = angle;
            m_WheelColliders[1].steerAngle = angle;
        }

        private void Accelerate(float accel, float multiplier)
        {
            m_Rigidbody.AddForce(
                    transform.forward * accel * multiplier * Time.deltaTime,
                    ForceMode.Impulse);
        }

        public void Move(float steer, float accel, float jump, float boost)
        {
            ControlWheels(steer);
            Accelerate(accel, DriveForce);
            Accelerate(boost, BoostForce);
        }
    }
}
