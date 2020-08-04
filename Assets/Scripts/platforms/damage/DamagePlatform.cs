using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


// ReSharper disable once CheckNamespace
public class DamagePlatform : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private bool repeatDealDamage;
    [SerializeField] private float damageCooldown = 2f;
    private float damageCooldownTimer;

    [SerializeField] private List<GameObject> damageable;

    private Dictionary<int, IDamageable> presentObj;


    private void Awake()
    {
        if (damageable == null) damageable = new List<GameObject>();
        if (presentObj == null) presentObj = new Dictionary<int, IDamageable>();
        damageCooldownTimer = damageCooldown;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        try
        {
            var damageBehaviour = other.gameObject.GetComponent<IDamageable>();
            var obj = damageable.Find(it => it.GetComponent<IDamageable>() == damageBehaviour)
                .GetComponent<IDamageable>();
            if (obj == null) return;
            if (presentObj.ContainsKey(obj.getID())) return;
            presentObj.Add(obj.getID(), obj);
            //Debug.LogWarning("Added GameObj with id" + obj.getID());
        }
        catch (InvalidCastException ignored)
        {
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        try
        {
            var damageBehaviour = other.gameObject.GetComponent<IDamageable>();
            presentObj.Remove(damageBehaviour.getID());
            //Debug.LogWarning("Removed GameObj with id" + damageBehaviour.getID());
        }
        catch (InvalidCastException _)
        {
            /* Ignored */
        }
    }

    private void FixedUpdate()
    {
        damageCooldownTimer -= Time.fixedDeltaTime;
        if (damageCooldownTimer > 0) return;
        damageCooldownTimer = damageCooldown;
        if (presentObj.Count < 1) return;
        //Debug.LogWarning("Applying Damage to " + presentObj.Values.Count + " in frame: " + TimerImpl.Instance.getRemainingTime());
        presentObj.Values.ToList().ForEach(item => { item?.applyDamage(damage); });
        if (repeatDealDamage) return;
        presentObj.Clear();
    }
}