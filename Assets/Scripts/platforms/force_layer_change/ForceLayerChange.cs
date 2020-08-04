using System;
using UnityEngine;


// ReSharper disable once CheckNamespace
public class ForceLayerChange : MonoBehaviour
{
    private GameController gameController;
    private bool hasTriggered;
    [SerializeField] private bool changeToNow;


    private void Start()
    {
        gameController = GameController.Instance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;
        // Player has collided
        hasTriggered = true;
        gameController.changeLayer(changeToNow);
    }
}