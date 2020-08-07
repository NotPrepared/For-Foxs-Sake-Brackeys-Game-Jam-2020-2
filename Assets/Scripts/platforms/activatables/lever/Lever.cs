using System.Collections.Generic;
using UnityEngine;


public class Lever : ActivatorBase
{
    private ActivationStateChangeEvent onStateChangeBacking;

    //Code by Lukas dont mind me
    [SerializeField] private bool flipX;
    [SerializeField] private bool flipY;

    //End code Lukas
    [SerializeField] private bool currentState;
    [SerializeField] private bool inverted;
    [SerializeField] private List<GameTagsEnum> input_tags;
    private List<string> tags;

    private void Awake()
    {
        currentState = inverted;
        if (onStateChangeBacking == null)
        {
            onStateChangeBacking = new ActivationStateChangeEvent();
        }

        tags = new List<string>();
        input_tags.ForEach(it => tags.Add(GameTags.of(it)));
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = currentState;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isTagInList(other.tag)) return;
        if (!other.isTrigger) return;

        currentState = !currentState;
        var spriteRenderer = GetComponent<SpriteRenderer>();

        //Changed Flipcode by Lukas
        if (flipX) spriteRenderer.flipX = currentState;
        if (flipY) spriteRenderer.flipX = currentState;
        //endcode Lukas
        onStateChangeBacking.Invoke(currentState);
    }

    private bool isTagInList(string it) => tags.Contains(it);

    public override ActivationStateChangeEvent onStateChange => onStateChangeBacking;
    public override bool getCurrent() => currentState;
}