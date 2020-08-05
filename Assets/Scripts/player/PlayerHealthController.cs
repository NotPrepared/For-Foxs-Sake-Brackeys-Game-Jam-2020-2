using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


// ReSharper disable once CheckNamespace
public class PlayerHealthController : MonoBehaviour, IDamageable
{
    [SerializeField] private float dieBelowY = -200;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float currentHealth = 100;

    [SerializeField] public HealthChangedEvent onHealthChange;
    [SerializeField] private TextMeshProUGUI healthBar;

    private void Awake()
    {
        if (onHealthChange == null)
        {
            onHealthChange = new HealthChangedEvent();
        }
    }

    private void Start()
    {
        if (healthBar != null)
        {
            onHealthChange.AddListener(f => healthBar.text = $"{f:0.###}");
            healthBar.text = $"{currentHealth:0.###}";   
        }
        else
        {
            Debug.LogWarning("healthBar is set to null not updates for player health are displayed");
        }
    }


    public void applyDamage(float damage)
    {
        currentHealth -= damage;
        onHealthChange.Invoke(currentHealth);
        Debug.LogWarning("Took Damage: " + damage);
    }

    public int getID() => GetHashCode();

    private void FixedUpdate()
    {
        if (transform.position.y < dieBelowY)
        {
            applyDamage(maxHealth + 1);
        }
    }
}

[Serializable]
public class HealthChangedEvent : UnityEvent<float>
{ }