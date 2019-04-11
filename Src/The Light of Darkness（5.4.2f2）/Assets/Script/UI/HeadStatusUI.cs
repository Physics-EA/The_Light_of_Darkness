﻿using UnityEngine;
using System.Collections;

public class HeadStatusUI : MonoBehaviour
{
    public static HeadStatusUI _instance;

    private UILabel name;

    private UISlider hpBar;
    private UISlider mpBar;

    private UILabel hpLabel;
    private UILabel mpLabel;
    private PlayStatus ps;

    void Awake()
    {
        _instance = this;

        name = transform.Find("Name").GetComponent<UILabel>();
        hpBar = transform.Find("HP").GetComponent<UISlider>();
        mpBar = transform.Find("MP").GetComponent<UISlider>();

        hpLabel = transform.Find("HP/Thumb/Label").GetComponent<UILabel>();
        mpLabel = transform.Find("MP/Thumb/Label").GetComponent<UILabel>();
    }

    void Start()
    {
        ps = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayStatus>();
        UpdateShow();
    }

    public void UpdateShow()
    {
        name.text = "Lv." + ps.level + " " + ps.name;
        hpBar.value = ps.hp_remain / ps.hp;
        mpBar.value = ps.mp_remain / ps.mp;

        if (ps.hp_remain >= 0)
        {
            hpLabel.text = ps.hp_remain + "/" + ps.hp;
        }
        else
        {
            hpLabel.text = 0 + "/" + ps.hp;
        }
        if (ps.mp_remain >= 0)
        {
            mpLabel.text = ps.mp_remain + "/" + ps.mp;
        }
        else
        {
            mpLabel.text = 0 + "/" + ps.mp;
        }

    }


}
