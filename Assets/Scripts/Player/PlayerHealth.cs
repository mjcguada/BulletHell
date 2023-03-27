// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public FloatVariable Health;
    public bool ResetHealth = true;
    public FloatReference StartingHealth;

    [Header("Events")]
    public UnityEvent DamageEvent;
    public UnityEvent DeathEvent;

    private void Start()
    {
        if (ResetHealth)
            Health.SetValue(StartingHealth);
    }

    private void OnTriggerEnter(Collider other)
    {
        DamageDealer damage = other.gameObject.GetComponent<DamageDealer>();
        if (damage != null)
        {
            Health.ApplyChange(-damage.DamageAmount);
            DamageEvent.Invoke();
        }

        if (Health.Value <= 0.0f)
        {
            DeathEvent.Invoke();
        }
    }
}
