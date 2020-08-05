using System;
using TMPro;
using UnityEngine;


public class TextDisplayController : MonoBehaviour
{
    public static TextDisplayController Instance;
    [SerializeField] private TextMeshProUGUI display;
    [SerializeField] private GameObject textDisplayRoot;

    private float fadeAfter;
    private bool hasContent;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        setDisplayText("", 0);
    }

    private void FixedUpdate()
    {
        if (!hasContent) return;
        fadeAfter -= Time.fixedDeltaTime;
        if (fadeAfter > 0) return;
        // Delete text
        textDisplayRoot.SetActive(false);
        hasContent = false;
    }

    public void setDisplayText(string text, float fadeTime)
    {
        hasContent = true;
        fadeAfter = fadeTime;
        display.text = text;
       textDisplayRoot.SetActive(true);
    }
}