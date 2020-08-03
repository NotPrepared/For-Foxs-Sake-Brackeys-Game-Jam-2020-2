using System;
using UnityEngine;

// ReSharper disable once CheckNamespace
public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float fallDelay;
    [SerializeField] private float revokeJumpableDelay;
    [SerializeField] private GameObject fallTarget;

    private ITimer timer;
    [SerializeField]
    private float fallTimestamp;
    [SerializeField]
    private float revokeJumpTimestamp;
    [SerializeField]
    private bool hasFallTimestamp;

    private bool isFalling;
    private void Start()
    {
        timer = TimerImpl.Instance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasFallTimestamp) return;
        if (other.gameObject == fallTarget) return;
        startTimer();
    }
    

    private void startTimer()
    {
        fallTimestamp = timer.getRemainingTime() - fallDelay;
        hasFallTimestamp = true;
    }

    private void Update()
    {
        if (isFalling)
        {
            if (timer.getRemainingTime() <= revokeJumpTimestamp)
            {
                fallTarget.GetComponent<BoxCollider2D>().isTrigger = true;
            }

            if (transform.position.y < -100)
            {
                Destroy(fallTarget);
            }

            return;
        }

        if (!hasFallTimestamp) return;
        
        if (timer.getRemainingTime() <= fallTimestamp)
        {
            isFalling = true;
            var rb = fallTarget.AddComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            revokeJumpTimestamp = timer.getRemainingTime() - revokeJumpableDelay;
        }
    }
}