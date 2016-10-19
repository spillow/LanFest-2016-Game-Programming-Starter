using UnityEngine;
using System.Collections;

public class AIControl : MonoBehaviour
{
    public Transform Ball;

    public WheelCollider[] m_WheelColliders = new WheelCollider[4];
    public GameObject[] m_WheelMeshes = new GameObject[4];

    public float MaxSteerAngle = 15.0f;
    public float DriveTorque = 3000f;

    // Use this for initialization
    void Start()
    {

    }

    private void Move()
    {
        for (int i = 0; i < 4; i++)
        {
            Quaternion quat;
            Vector3 position;
            m_WheelColliders[i].GetWorldPose(out position, out quat);
            m_WheelMeshes[i].transform.position = position;
            m_WheelMeshes[i].transform.rotation = quat;
        }

        for (int i = 0; i < 4; i++)
        {
            m_WheelColliders[i].motorTorque = DriveTorque;
        }

        Vector3 vec = Ball.position - transform.position;
        vec = new Vector3(vec.x, 0f, vec.z);
        vec.Normalize();

        float angle = Vector3.Angle(transform.forward, vec);
        Vector3 cross = Vector3.Cross(transform.forward, vec);

        if (cross.y < 0f)
            angle = -angle;

        angle = Mathf.Clamp(angle, -MaxSteerAngle, MaxSteerAngle);

        m_WheelColliders[0].steerAngle = angle;
        m_WheelColliders[1].steerAngle = angle;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }
}
