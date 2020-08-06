using UnityEngine;


public class DoorController : AxisMovingPlatform
{
    [SerializeField] private ActivatorBase activator;
    

    private void Start()
    {
        base.Start();
        freezeMovement = !activator.getCurrent();
        activator.onStateChange.AddListener(b => freezeMovement = !b);
    }
}