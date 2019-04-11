using UnityEngine;
using System.Collections;

public class EquipmentUI : MonoBehaviour
{

    public static EquipmentUI _instance;
    private TweenPosition tween;
    private bool isShow = false;

    private GameObject headgear;
    private GameObject armor;
    private GameObject righthand;
    private GameObject lefthand;
    private GameObject shoe;
    private GameObject accessory;

    private PlayStatus ps;

    public GameObject equipmentItem;

    public int attack = 0;
    public int def = 0;
    public int speed = 0;


    // Use this for initialization
    void Start()
    {
        _instance = this;
        tween = _instance.GetComponent<TweenPosition>();

        headgear = transform.Find("Headgear").gameObject;
        armor = transform.Find("Armor").gameObject;
        righthand = transform.Find("RightHand").gameObject;
        lefthand = transform.Find("LeftHand").gameObject;
        shoe = transform.Find("Shoe").gameObject;
        accessory = transform.Find("Accessory").gameObject;

        ps = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayStatus>();

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


    /// <summary>
    /// 处理物品穿戴功能
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool Dress(int id)
    {
        ObjectInfo info = ObjectsInfo._instance.GetObjectInfoById(id);
        if (info.type != ObjectType.Equip)
        {
            return false;//穿戴不成功
        }

        if (ps.heroType == HeroType.Magician)
        {
            if (info.applicationType == ApplicationType.Swordman)
            {
                return false;
            }
        }

        if (ps.heroType == HeroType.Swordman)
        {
            if (info.applicationType == ApplicationType.Magician)
            {
                return false;
            }
        }

        GameObject parent = null;

        switch (info.dressType)
        {
            case DressType.Headgear:
                parent = headgear;
                break;
            case DressType.Armor:
                parent = armor;
                break;
            case DressType.LeftHand:
                parent = lefthand;
                break;
            case DressType.RightHand:
                parent = righthand;
                break;
            case DressType.Shoe:
                parent = shoe;
                break;
            case DressType.Accessory:
                parent = accessory;
                break;
        }

        EquipmentItem item = parent.GetComponentInChildren<EquipmentItem>();

        //穿戴了同样类型的装备
        if (item != null)
        {
            Inventory._instance.GetId(item.id);//把已经穿戴的装备卸下，放回物品栏
            item.SetInfo(info);
        }
        //没有穿戴同样类型的装备
        else
        {
            GameObject itemGo = NGUITools.AddChild(parent, equipmentItem);
            itemGo.transform.localPosition = Vector3.zero;
            itemGo.GetComponent<EquipmentItem>().SetInfo(info);
        }

        UpdateProperty();
        return true;
    }

    public void TakeOff(int id, GameObject go)
    {
        Inventory._instance.GetId(id);
        GameObject.Destroy(go);
        UpdateProperty();

    }

    void UpdateProperty()
    {
        this.attack = 0;
        this.def = 0;
        this.speed = 0;

        EquipmentItem headgearItem = headgear.GetComponentInChildren<EquipmentItem>();
        PlusProperty(headgearItem);
        EquipmentItem armorItem = armor.GetComponentInChildren<EquipmentItem>();
        PlusProperty(armorItem);
        EquipmentItem leftHandItem = lefthand.GetComponentInChildren<EquipmentItem>();
        PlusProperty(leftHandItem);
        EquipmentItem rightHandItem = righthand.GetComponentInChildren<EquipmentItem>();
        PlusProperty(rightHandItem);
        EquipmentItem shoeItem = shoe.GetComponentInChildren<EquipmentItem>();
        PlusProperty(shoeItem);
        EquipmentItem accessoryItem = accessory.GetComponentInChildren<EquipmentItem>();
        PlusProperty(accessoryItem);
    }

    void PlusProperty(EquipmentItem item)
    {
        if (item != null)
        {
            ObjectInfo equipInfo = ObjectsInfo._instance.GetObjectInfoById(item.id);
            this.attack += equipInfo.attack;
            this.def += equipInfo.def;
            this.speed += equipInfo.speed;
        }

    }

}
