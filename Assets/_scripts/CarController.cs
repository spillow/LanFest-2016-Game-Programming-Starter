using System;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Car
{
    public class CarController : MonoBehaviour
    {
        public WheelCollider[] m_WheelColliders = new WheelCollider[4];
        public GameObject[] m_WheelMeshes = new GameObject[4];

        public float MaxSteerAngle = 30.0f;
        public float DriveTorque = 10000f;
        public float BoostForce = 100000f;

        private float CurrTorque = 0f;
        public float MaxTorque = 10000f;

        private Rigidbody m_Rigidbody;

        void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }

        public float BrakeInput = 0.0f;
        public float CurrentSteerAngle = 0.0f;
        public float DownForce = 1000f;

        float m_OldRotation = 0f;

        private void SteerHelper()
        {
            // this if is needed to avoid gimbal lock problems that will make the car suddenly shift direction
            if (Mathf.Abs(m_OldRotation - transform.eulerAngles.y) < 10f)
            {
                var turnadjust = (transform.eulerAngles.y - m_OldRotation) * 0.644f;
                Quaternion velRotation = Quaternion.AngleAxis(turnadjust, Vector3.up);
                m_Rigidbody.velocity = velRotation * m_Rigidbody.velocity;
            }
            m_OldRotation = transform.eulerAngles.y;
        }

        private void AdjustTorque(float forwardSlip)
        {
            if (forwardSlip >= 0.5 && CurrTorque >= 0)
            {
                Debug.Log("slip:" + forwardSlip);
                CurrTorque -= 10000;
            }
            else
            {
                CurrTorque += 10000;
                if (CurrTorque > MaxTorque)
                {
                    CurrTorque = MaxTorque;
                }
            }
        }

        private void TractionControl()
        {
            WheelHit wheelHit;
            // loop through all wheels
            for (int i = 0; i < 4; i++)
            {
                m_WheelColliders[i].GetGroundHit(out wheelHit);

                AdjustTorque(wheelHit.forwardSlip);
            }
        }

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

        private void Boost(float boost)
        {
            m_Rigidbody.AddForce(
                    transform.forward * boost * BoostForce * Time.deltaTime,
                    ForceMode.Impulse);
        }

        private void Drive(float accel)
        {
            float torque = accel * (CurrTorque / 4f);
            for (int i = 0; i < 4; i++)
            {
                m_WheelColliders[i].motorTorque = torque;
            }
        }

        public void Move(float steer, float accel, float jump, float boost)
        {
            SteerHelper();
            ControlWheels(steer);
            Drive(accel);
            Boost(boost);
            TractionControl();
        }
    }
}
