﻿using System;
using System.Collections.Generic;
using UnityEngine;


// ReSharper disable once CheckNamespace
public abstract class Pushable : MonoBehaviour
{
    [SerializeField] public float weight;
    [SerializeField] public bool usesRB;
    [SerializeField] private List<MonoBehaviour> targets;
    private HashSet<IPusher> pushables;


    private void Awake()
    {
        pushables = new HashSet<IPusher>();
        if (targets == null) throw new UnassignedReferenceException("Require a targets which can push object");
        foreach (var target in targets)
        {
            if (target is IPusher pusher)
            {
                pushables.Add(pusher);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var pusherComponent = other.gameObject.GetComponent<IPusher>();
        pusherComponent?.pushStarted(weight, this);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        var pusherComponent = other.gameObject.GetComponent<IPusher>();
        pusherComponent?.pushStopped(this);
    }
}

public class PushablePlatform : Pushable
{
    private void Start()
    {
    }
}

public interface IPusher
{
    void applyPush(float force);
    void pushStarted(float collectiveWeight, Pushable target);
    void pushStopped(Pushable target);
}