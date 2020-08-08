using System;
using TMPro;
using UnityEngine;


public class DisplayDeathReason : MonoBehaviour
{
    public static DisplayDeathReason Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void displayDeathReason(DeathType reason)
    {
        var textComponent = GetComponent<TextMeshProUGUI>();
        switch (reason)
        {
            case DeathType.TIME:
                textComponent.text = "Your time ran out.";
                break;
            case DeathType.BOUNDS:
                textComponent.text = "That was not the right way.";
                break;
            case DeathType.DAMAGE:
                textComponent.text = "Wow that was a lot of damage.";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(reason), reason, null);
        }
    }
}