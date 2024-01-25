using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterShot : MonoBehaviour, IDamageable
{
    //Simple debug to show a shot registered
    public void TakeDamage()
    {
        Debug.Log("Box: I am hit by a bullet!");
    }
}

