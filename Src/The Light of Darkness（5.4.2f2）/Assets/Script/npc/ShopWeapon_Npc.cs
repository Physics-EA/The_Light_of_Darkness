using UnityEngine;
using System.Collections;

public class ShopWeapon_Npc : NPC
{

    //当鼠标在这个游戏物体之上的时候，会一直调用这个方法
    public void OnMouseOver()
    {
        //弹出来武器商店
        if (Input.GetMouseButtonDown(0))
        {
            GetComponent<AudioSource>().Play();
            ShopWeaponUI._instance.TransformState();
        }
    }

}
