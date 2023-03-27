// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public FloatVariable HP;

    public bool ResetHP;

    public FloatReference StartingHP;

    private void Start()
    {
        if (ResetHP)
            HP.SetValue(StartingHP);
    }

    private void OnTriggerEnter(Collider other)
    {
        DamageDealer damage = other.gameObject.GetComponent<DamageDealer>();
        if (damage != null)
            HP.ApplyChange(-damage.DamageAmount);
    }


}
