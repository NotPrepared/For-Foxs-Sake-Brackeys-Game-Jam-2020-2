using System;
using UnityEngine;


public class StoryTrigger : MonoBehaviour
{
    [SerializeField] private string text;
    [SerializeField] private float disappearAfter;
    [SerializeField] private bool requiresSpecificLayer;
    [SerializeField] private bool requiresLayerNow;
    [SerializeField] private bool requiresOtherTriggerFired;
    [SerializeField] private StoryTrigger requiredTrigger;
    [SerializeField] private bool requireAcknowledgement;

    [HideInInspector]
    public bool hasTriggered;

    private void Awake()
    {
        if (requiredTrigger == null)
        {
            if (requiresOtherTriggerFired)
            {
                throw new MissingReferenceException("IF Trigger is required than a instance must be passed.");
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;
        if (requiresSpecificLayer)
        {
            if (GameController.Instance.isPresent != requiresLayerNow)
            {
                return;
            }
        }

        if (requiresOtherTriggerFired)
        {
            if (requiredTrigger.hasTriggered == false)
            {
                return;
            }
        }

        TextDisplayController.Instance.setDisplayText(text, disappearAfter, requireAcknowledgement);
        hasTriggered = true;
    }
}