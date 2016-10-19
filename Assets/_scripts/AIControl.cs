using UnityEngine;
using System.Collections;

public class AIControl : MonoBehaviour
{
    public WheelCollider[] m_WheelColliders = new WheelCollider[4];
    public GameObject[] m_WheelMeshes = new GameObject[4];

    private void Move()
    {
      // TODO 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }
}
