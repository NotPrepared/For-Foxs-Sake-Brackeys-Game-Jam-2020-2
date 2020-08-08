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
    private bool requireAck;
    private bool acked;

    private float interactCD;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        setDisplayText("", 0, false);
    }

    private void Update()
    {
        if (!hasContent) return;
        if (requireAck)
        {
            if (acked) return;
            interactCD -= Time.unscaledDeltaTime;
            if (interactCD > 0) return;
            if (Input.GetAxisRaw("Submit") == 1)
            {
                acked = true;
                TimerImpl.Instance.resumeTimer();
                // Delete text
                textDisplayRoot.SetActive(false);
                hasContent = false;
            }
        }
        else
        {
            fadeAfter -= Time.fixedDeltaTime;
            if (fadeAfter > 0) return;
            // Delete text
            textDisplayRoot.SetActive(false);
            hasContent = false;
        }
    }

    public void setDisplayText(string text, float fadeTime, bool reqAck)
    {
        hasContent = true;
        requireAck = reqAck;
        acked = false;
        if (reqAck)
        {
            TimerImpl.Instance.pauseTimer();
        }
        interactCD = 0.8f;
        fadeAfter = fadeTime;
        display.text = text;
        textDisplayRoot.SetActive(true);
    }
}