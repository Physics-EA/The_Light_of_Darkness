using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillUI : MonoBehaviour
{
    public static SkillUI _instance;
    private TweenPosition tween;
    private bool isShow = false;
    private PlayStatus ps;

    public UIGrid grid;
    public GameObject skillItemPrefab;

    public int[] magicianSkillIdList;
    public int[] swordmanSkillIdList;


    // Use this for initialization
    void Awake()
    {
        _instance = this;
        tween = this.GetComponent<TweenPosition>();
    }


    private void Start()
    {
        ps = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayStatus>();
        int[] idList = null;
        switch (ps.heroType)
        {
            case HeroType.Magician:
                idList = magicianSkillIdList;
                break;
            case HeroType.Swordman:
                idList = swordmanSkillIdList;
                break;
        }

        foreach (int id in idList)
        {

            GameObject itemGo = NGUITools.AddChild(grid.gameObject, skillItemPrefab);

            grid.AddChild(itemGo.transform);

            itemGo.GetComponent<SkillItem>().SetId(id);

        }

    }



    public void TransformState()
    {
        if (isShow)
        {
            tween.PlayReverse();
            isShow = false;
        }
        else
        {
            tween.PlayForward();
            isShow = true;
            UpdateShow();
        }
    }

    void UpdateShow()
    {
        SkillItem[] items = this.GetComponentsInChildren<SkillItem>();
        foreach (SkillItem item in items)
        {
            item.UpdateShow(ps.level);
        }

    }



}
