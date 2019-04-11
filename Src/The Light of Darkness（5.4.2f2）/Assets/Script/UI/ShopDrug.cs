﻿using UnityEngine;
using System.Collections;

public class ShopDrug : MonoBehaviour
{
    public static ShopDrug _instance;
    private TweenPosition tween;
    private bool isShow = false;

    public GameObject numberDialog;
    public UIInput numberInput;

    private int buy_id = 0;

    // Use this for initialization
    void Start()
    {
        _instance = this;
        tween = this.GetComponent<TweenPosition>();
        numberDialog = this.transform.Find("NumberDialog").gameObject;
        numberInput = this.transform.Find("NumberDialog/NumberInput").GetComponent<UIInput>();

        numberDialog.SetActive(false);

    }

    public void TransformState()
    {
        if (isShow == false)
        {
            tween.PlayForward();
            isShow = true;
        }
        else
        {
            tween.PlayReverse();
            isShow = false;
        }
    }

    public void OnCloseButtonClick()
    {
        TransformState();
    }


    public void OnBuyId1001()
    {
        Buy(1001);
    }



    public void OnBuyId1002()
    {
        Buy(1002);
    }

    public void OnBuyId1003()
    {
        Buy(1003);
    }

    void Buy(int id)
    {
        ShowNumberDialog();
        buy_id = id;
    }

    public void OnOKButtonClick()
    {
        int count = int.Parse(numberInput.value);
        ObjectInfo info = ObjectsInfo._instance.GetObjectInfoById(buy_id);
        int price = info.price_buy;
        int price_total = price * count;

        bool success = Inventory._instance.GetCoin(price_total);

        if (success)//取款成功，可以购买
        {
            if (count > 0)
            {
                Inventory._instance.GetId(buy_id, count);
            }
        }
        else
        {

        }

        numberDialog.SetActive(false);
    }

    void ShowNumberDialog()
    {
        numberDialog.SetActive(true);
        numberInput.value = "0";
    }

}
