using System;
using UnityEngine;

namespace debug
{
    public class BuildHelper : MonoBehaviour
    {
        private void Start()
        {
            var transformPosition = transform.position;
            transformPosition.x = 0;
            transformPosition.y = 0;
            transform.position = transformPosition;
        }
    }
}