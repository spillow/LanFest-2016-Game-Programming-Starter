using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public enum GoalColor
{
    Blue,
    Orange
}

public class GoalDetector : MonoBehaviour
{
    public GameObject m_Ball;
    public GoalColor m_GoalColor;

    public Text GoalText;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == m_Ball.tag)
        {
            GoalText.text = "Goal Scored!";
        }
    }
}
