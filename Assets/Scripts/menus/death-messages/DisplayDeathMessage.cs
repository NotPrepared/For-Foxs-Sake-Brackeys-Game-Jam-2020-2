using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = System.Random;


public class DisplayDeathMessage : MonoBehaviour
{
    public static DisplayDeathMessage Instance;

    [SerializeField] private List<DeathMessage> msgs;
    [SerializeField] private List<DeathMessage> msgForceAppearance;

    private Dictionary<int, DeathMessage> specialized;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        if (msgs == null)
        {
            msgs = new List<DeathMessage>();
        }

        if (msgForceAppearance == null)
        {
            msgForceAppearance = new List<DeathMessage>();
        }

        if (specialized == null)
        {
            specialized = new Dictionary<int, DeathMessage>();
        }
    }

    public void displayMessage()
    {
        msgForceAppearance.ForEach(m => specialized.Add(m.comparisonInt, m));
        var deaths = PersistenceHandler.getPlayerDeaths();
        var random = new Random();
        DeathMessage candidate = null;
        var test = specialized;
        if (specialized.ContainsKey(deaths))
        {
            var deathMessage = specialized[deaths];
            candidate = deathMessage;
        }

        while (candidate == null)
        {
            var index = random.Next(0, msgs.Count);
            var temp = msgs[index];
            if (temp.isApplicable(deaths)) candidate = temp;
        }
        // Applicable Message - Assign to Textfield
        GetComponent<TextMeshProUGUI>().text = candidate.getMessage(deaths);
    }
}

[Serializable]
public class DeathMessage
{
    [SerializeField] private string text;
    [SerializeField] public PredicateType predicateType;
    [SerializeField] public int comparisonInt;

    public string getMessage(int playerDeaths)
    {
        return text.Replace("{death}", playerDeaths.ToString());
    }

    public bool isApplicable(int playerDeaths)
    {
        switch (predicateType)
        {
            case PredicateType.NONE:
                return true;
            case PredicateType.EQUAL:
                return equal(playerDeaths, comparisonInt);
            case PredicateType.LESS:
                return less(playerDeaths, comparisonInt);
            case PredicateType.MORE:
                return more(playerDeaths, comparisonInt);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private bool equal(int a, int b) => a == b;
    private bool less(int a, int b) => a < b;
    private bool more(int a, int b) => a > b;
}

[Serializable]
public enum PredicateType
{
    NONE,
    EQUAL,
    LESS,
    MORE
}