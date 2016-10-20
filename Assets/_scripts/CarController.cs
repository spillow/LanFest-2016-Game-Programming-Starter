using System;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Car
{
    public class CarController : MonoBehaviour
    {
        public WheelCollider[] m_WheelColliders = new WheelCollider[4];
        public GameObject[] m_WheelMeshes = new GameObject[4];

        private float CurrTorque = 0f;
        public float MaxTorque = 10000f;
        public float MaxSteerAngle = 30f;

        private Rigidbody m_Rigidbody;

        void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }

        public float BrakeInput = 0.0f;
        public float CurrentSteerAngle = 0.0f;

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
            CurrentSteerAngle = angle;
        }

        private void Boost(float boost)
        {
            // TODO
        }

        private void Drive(float accel)
        {
            float torque = accel * (CurrTorque / 4f);
            for (int i = 0; i < 4; i++)
            {
                m_WheelColliders[i].motorTorque = torque;
            }

            BrakeInput = -1f * Mathf.Clamp(accel, -1, 0);
        }

        // Is any wheel on a surface?
        public bool InContactWithSurface()
        {
            for (int i = 0; i < 4; i++)
            {
                WheelHit wheelhit;
                m_WheelColliders[i].GetGroundHit(out wheelhit);
                if (wheelhit.normal != Vector3.zero)
                    return true; // wheels aren't on the ground 
            }

            return false;
        }

        private void MoveInAir(float steer, float accel, float roll)
        {
            // TODO
        }

        private void Jump(float jump)
        {
            // TODO
        }

        public void Move(float steer, float accel, float jump, float boost, float roll)
        {
            SteerHelper();
            ControlWheels(steer);
            Drive(accel);
            Boost(boost);
            Jump(jump);
            MoveInAir(steer, accel, roll);
            TractionControl();
        }
    }
}
