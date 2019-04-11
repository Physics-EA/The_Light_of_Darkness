﻿using UnityEngine;
using System.Collections;

public class EquipmentItem : MonoBehaviour
{
    private UISprite sprite;
    public int id;

    private bool isHover = false;

    private void Awake()
    {
        sprite = this.GetComponent<UISprite>();
    }

    private void Update()
    {
        //当鼠标在这个装备栏之上的时候，检测鼠标右键的点击
        if (isHover)
        {
            //鼠标点击之后，表示卸下该装备
            if (Input.GetMouseButtonDown(1))
            {

                EquipmentUI._instance.TakeOff(id,this.gameObject);              
            }
        }
    }


    public void SetId(int id)
    {
        this.id = id;
        ObjectInfo info = ObjectsInfo._instance.GetObjectInfoById(id);
        SetInfo(info);
    }

    public void SetInfo(ObjectInfo info)
    {
        this.id = info.id;
        sprite.spriteName = info.icon_name;
    }

    public void OnHover(bool isOver)
    {
        isHover = isOver;

    }
}
