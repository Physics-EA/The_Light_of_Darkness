﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MagicSphere : MonoBehaviour
{

    public float attack = 0;

    private List<WolfBaby> wolfList = new List<WolfBaby>();


    public void OnTriggerEnter(Collider col)
    {
        if (col.tag == Tags.enemy)
        {
            WolfBaby baby = col.GetComponent<WolfBaby>();
            int index = wolfList.IndexOf(baby);
            if (index == -1)
            {
                baby.TakeDamage((int)attack);
                wolfList.Add(baby);
            }
        }
    }
}
