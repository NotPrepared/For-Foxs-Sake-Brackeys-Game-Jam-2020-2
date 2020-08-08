using System;
using UnityEngine;


public class DoorController : AxisMovingPlatform
{
    [SerializeField] private ActivatorBase activator;

    private bool hasListener;

    private new void Start()
    {
        base.Start();
        freezeMovement = !activator.getCurrent();
    }

    private void Update()
    {
        if (hasListener) return;
        Debug.LogWarning("Searching");
        if (activator.onStateChange != null)
        {
            Debug.LogWarning("Has Listener");
            activator.onStateChange?.AddListener(b => freezeMovement = !b);
            hasListener = true;
        }
    }
}