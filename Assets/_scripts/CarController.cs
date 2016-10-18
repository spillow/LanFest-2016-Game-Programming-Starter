using System;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Car
{

    public class CarController : MonoBehaviour
    {
        void Start()
        {

        }

        public float BrakeInput { get { return 0.0f; }  }
        public float CurrentSteerAngle { get { return 0.0f; }  }
    }
}
