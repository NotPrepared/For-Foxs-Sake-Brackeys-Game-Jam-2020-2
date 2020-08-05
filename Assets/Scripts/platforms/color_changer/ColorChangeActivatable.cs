using System;
using UnityEngine;
using UnityEngine.Events;


public class ColorChangeActivatable : MonoBehaviour
{
    private Color initial;
    [SerializeField]
    private Color activated;
    [SerializeField]
    private Activator activator;

    private SpriteRenderer spriteRender;

    private void Awake()
    {
        spriteRender = gameObject.GetComponent<SpriteRenderer>();
        initial = spriteRender.color;
        // Set Listener
        activator.onStateChange.AddListener(changeColor);
    }

    private void Start()
    {
        changeColor(activator.getCurrent());
    }

    private void changeColor(bool active) => spriteRender.color = active ? activated : initial;
}