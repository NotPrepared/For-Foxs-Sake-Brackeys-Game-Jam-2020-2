using System;
using UnityEngine;


// ReSharper disable once CheckNamespace
public class PortalController : MonoBehaviour
{
    [SerializeField] private GameObject destination;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        other.gameObject.transform.position = destination.transform.position;
    }
}