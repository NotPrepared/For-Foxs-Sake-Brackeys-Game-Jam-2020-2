using UnityEngine;
using UnityEngine.Events;

public abstract class ActivatorBase : MonoBehaviour
{
    public abstract ActivationStateChangeEvent onStateChange { get; }

    public abstract bool getCurrent();
}

public class ActivationStateChangeEvent : UnityEvent<bool>
{
}