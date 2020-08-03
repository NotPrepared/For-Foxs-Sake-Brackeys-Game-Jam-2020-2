using System;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : Activator
{
    private ActivationStateChangeEvent onStateChangeBacking;
    [SerializeField]
    private bool currentState;
    [SerializeField] private bool inverted;

    private void Awake()
    {
        currentState = inverted;
        if (onStateChangeBacking == null)
        {
            onStateChangeBacking = new ActivationStateChangeEvent();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        currentState = true;
        onStateChangeBacking.Invoke(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        currentState = false;
        onStateChangeBacking.Invoke(false);
    }

    public override ActivationStateChangeEvent onStateChange => onStateChangeBacking;
    public override bool getCurrent() => currentState;
}

public class ActivationStateChangeEvent : UnityEvent<bool>
{
}


public abstract class Activator : MonoBehaviour
{
    public abstract ActivationStateChangeEvent onStateChange { get; }

    public abstract bool getCurrent();
}