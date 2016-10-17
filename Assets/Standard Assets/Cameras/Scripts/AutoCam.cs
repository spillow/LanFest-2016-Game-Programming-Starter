using System;
using UnityEngine;

namespace UnityStandardAssets.Cameras
{
    public class AutoCam : PivotBasedCameraRig
    {
        [SerializeField]
        private float m_MoveSpeed = 3; // How fast the rig will move to keep up with target's position
        [SerializeField]
        private float m_TurnSpeed = 1; // How fast the rig will turn to keep up with target's rotation
        [SerializeField]
        private float m_SmoothTurnTime = 0.2f; // the smoothing for the camera's rotation

        public Transform m_NextToCar;

        private float m_CurrentTurnAmount; // How much to turn the camera
        private float m_TurnSpeedVelocityChange; // The change in the turn speed velocity

        public UnityStandardAssets.Vehicles.Car.CarController m_CarController;
        private Quaternion m_PrevRollRotation;

        private bool m_BallCam = false;

        public Transform m_Ball;

        private Vector3 m_InitPivotPos;

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
            m_InitPivotPos = m_Pivot.transform.localPosition;
        }

        public void BallCamPressed()
        {
            m_BallCam = !m_BallCam;
        }

        protected override void FollowTarget(float deltaTime)
        {
            // if no target, or no time passed then we quit early, as there is nothing to do
            if (!(deltaTime > 0) || m_Target == null)
            {
                return;
            }

            // camera position moves towards target position:
            transform.position = Vector3.Lerp(
                transform.position,
                m_Target.position,
                deltaTime * m_MoveSpeed);

            if (m_BallCam)
            {
                Vector3 CarToBall = m_Ball.position - m_Target.position;
                Vector3 cpy = CarToBall;
                CarToBall.y = 0;
                float t = Mathf.Abs(Vector3.Dot(CarToBall.normalized, cpy.normalized));

                Quaternion rotation = Quaternion.LookRotation(CarToBall, Vector3.up);

                transform.rotation = Quaternion.Lerp(
                                    transform.rotation,
                                    rotation,
                                    m_TurnSpeed * deltaTime);

                //Debug.Log("t: " + t);

                m_Pivot.localPosition = Vector3.Lerp(
                    m_NextToCar.localPosition,
                    m_InitPivotPos,
                    t);

                m_Pivot.LookAt(m_Ball);

                /*
                Vector3 CamToBall = m_Ball.position - m_Pivot.position;
                Quaternion rotation1 = Quaternion.LookRotation(CamToBall, Vector3.up);

                m_Pivot.transform.rotation = Quaternion.Lerp(
                                    m_Pivot.transform.rotation,
                                    rotation1,
                                    m_TurnSpeed * deltaTime);
                                    */

            }
            else // player cam
            {
                m_Pivot.rotation = m_Pivot.parent.rotation;
                m_Pivot.localPosition = m_InitPivotPos;

                // initialise some vars, we'll be modifying these in a moment
                var targetForward = m_Target.forward;

                // the camera's rotation is aligned towards the object's velocity direction.
                m_CurrentTurnAmount = Mathf.SmoothDamp(
                    m_CurrentTurnAmount,
                    1,
                    ref m_TurnSpeedVelocityChange,
                    m_SmoothTurnTime);

                // camera's rotation is split into two parts, which can have independent speed settings:
                // rotating towards the target's forward direction (which encompasses its
                // 'yaw' and 'pitch')
                targetForward.y = 0;
                if (targetForward.sqrMagnitude < float.Epsilon)
                {
                    targetForward = transform.forward;
                }

                var rollRotation = m_CarController.InContactWithSurface() ?
                    Quaternion.LookRotation(targetForward, Vector3.up) :
                    m_PrevRollRotation;

                m_PrevRollRotation = rollRotation;

                // and aligning with the target object's up direction (i.e. its 'roll')
                transform.rotation = Quaternion.Lerp(
                    transform.rotation,
                    rollRotation,
                    m_TurnSpeed * m_CurrentTurnAmount * deltaTime);
            }
        }
    }
}
