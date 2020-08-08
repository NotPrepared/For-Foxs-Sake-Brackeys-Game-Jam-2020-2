using System;
using System.Linq;
using TMPro;
using UnityEngine;


public class CheckedMarker : MonoBehaviour
{
    private bool isChecked;
    [SerializeField]
    private GameObject textGameObject;

    private const string CHECK = "[X] "; 

    public void updateCheckedState(bool state)
    {
        return;
        isChecked = state;
        var textComponent = textGameObject.GetComponent<TextMeshProUGUI>();
        var initialText = textComponent.text.Replace(CHECK, "");
        if (isChecked)
        {
            textGameObject.GetComponent<TextMeshProUGUI>().text = $"{CHECK}{initialText}";
        }
        else
        {
            textGameObject.GetComponent<TextMeshProUGUI>().text = initialText;
        }
    }

}