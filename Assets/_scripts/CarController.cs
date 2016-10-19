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
        public float AirRollFactor = 5000f;
        public float FlipFactor = 16000f;
        public float JumpForce = 1000000f;

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
            CurrentSteerAngle = angle;
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

            BrakeInput = -1f*Mathf.Clamp(accel, -1, 0);
        }

        // Are all four wheels on a surface?
        public bool InContactWithSurface()
        {
            for (int i = 0; i < 4; i++)
            {
                WheelHit wheelhit;
                m_WheelColliders[i].GetGroundHit(out wheelhit);
                if (wheelhit.normal == Vector3.zero)
                    return false; // wheels aren't on the ground 
            }

            return true;
        }

        private void MoveInAir(float steer, float accel, float roll)
        {
            if (InContactWithSurface())
                return;

            if (roll > 0f)
            {
                Vector3 airRoll = new Vector3(
                    0f, 0f,
                    -1f * steer * roll * AirRollFactor * Time.deltaTime);
                m_Rigidbody.AddRelativeTorque(airRoll, ForceMode.Impulse);
            }
            else
            {
                Vector3 torqDir = new Vector3(
                    accel,
                    steer,
                    0f);
                m_Rigidbody.AddRelativeTorque(
                    torqDir * FlipFactor * Time.deltaTime, ForceMode.Impulse);
            }
        }

        private void Jump(float jump)
        {
            if (!InContactWithSurface())
                return;

            m_Rigidbody.AddForce(
                    transform.up * jump * JumpForce * Time.deltaTime,
                    ForceMode.Impulse);
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
