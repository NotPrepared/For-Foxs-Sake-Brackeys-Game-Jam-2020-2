using System;
using UnityEngine;


public class PortalRotateSprite : MonoBehaviour
{

    [SerializeField]
    private float speed = 50f;
    private void FixedUpdate()
    {
        transform.Rotate (0, 0,speed * Time.fixedDeltaTime);
    }
}