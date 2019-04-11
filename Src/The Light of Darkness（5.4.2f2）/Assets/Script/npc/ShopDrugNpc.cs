using UnityEngine;
using System.Collections;

public class ShopDrugNpc : NPC
{
    /// <summary>
    /// 当鼠标在这个游戏物体之上的时候，会一直调用这个方法
    /// </summary>
    public void OnMouseOver()
    {
        //弹出药品购买列表
        if (Input.GetMouseButtonDown(0))
        {
            this.GetComponent<AudioSource>().Play();
            ShopDrug._instance.TransformState();
        }
    }
}
