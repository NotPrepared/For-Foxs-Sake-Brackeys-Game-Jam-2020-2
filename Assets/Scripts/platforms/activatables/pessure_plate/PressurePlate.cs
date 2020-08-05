using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : ActivatorBase
{
    private ActivationStateChangeEvent onStateChangeBacking;
    [SerializeField]
    private bool currentState;
    [SerializeField] private bool inverted;
    [SerializeField] private List<GameTagsEnum> input_tags;
    private List<string> tags;
    private int countOfColliders;

    private void Awake()
    {
        currentState = inverted;
        if (onStateChangeBacking == null)
        {
            onStateChangeBacking = new ActivationStateChangeEvent();
        }
        tags = new List<string>();
        input_tags.ForEach(it => tags.Add(GameTags.of(it)));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isTagInList(other.tag)) return;
        countOfColliders++;
        if (countOfColliders == 1)
        {
            currentState = true;
            onStateChangeBacking.Invoke(true);   
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!isTagInList(other.tag)) return;
        countOfColliders--;
        if (countOfColliders <= 0)
        {
            currentState = false;
            onStateChangeBacking.Invoke(false);   
        }
    }

    private bool isTagInList(string it) => tags.Contains(it);

    public override ActivationStateChangeEvent onStateChange => onStateChangeBacking;
    public override bool getCurrent() => currentState;
}