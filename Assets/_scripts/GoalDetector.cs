using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class GoalDetector : MonoBehaviour
{
    public GameObject Ball;
    public Text GoalText;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Ball)
        {
            GoalText.text = "Goal Scored!";
        }
    }
}
