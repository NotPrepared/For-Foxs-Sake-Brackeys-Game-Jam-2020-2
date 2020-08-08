using System;
using UnityEngine;


public class AwardMoveTrigger : MonoBehaviour
{
    [SerializeField] private MoveTypes typeToAward;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(GameTags.PLAYER)) return;
        other.gameObject.GetComponent<CharacterController2D>().enableMove(typeToAward);
    }
}