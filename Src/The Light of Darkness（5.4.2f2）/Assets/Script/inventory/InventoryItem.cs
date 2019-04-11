using UnityEngine;
using System.Collections;

public class InventoryItem : UIDragDropItem
{

    private UISprite sprite;
    private int id;

    private bool isHover = false;

    private void Awake()
    {
        sprite = this.GetComponent<UISprite>();
    }

    private void Update()
    {
        if (isHover)
        {
            //显示提示信息
            InventoryDes._instance.Show(id);

            if (Input.GetMouseButtonDown(1))
            {
                //出来穿戴功能
                bool success = EquipmentUI._instance.Dress(id);
                if (success)
                {
                    transform.parent.GetComponent<InventoryItemGrid>().MinusNumber();
                }
            }

        }


    }

    protected override void OnDragDropRelease(GameObject surface)
    {
        base.OnDragDropRelease(surface);

        if (surface != null)
        {
            //当拖放到一个空的格子里
            if (surface.tag == Tags.inventory_item_grid)
            {
                //拖放到自己的格子里
                if (surface == this.transform.parent.gameObject)
                {

                }
                else
                {
                    InventoryItemGrid oldParent = this.transform.parent.GetComponent<InventoryItemGrid>();
                    this.transform.parent = surface.transform;
                    ResetPosition();
                    InventoryItemGrid newParent = surface.GetComponent<InventoryItemGrid>();
                    newParent.SetId(oldParent.id, oldParent.num); oldParent.ClearInfo();
                }
            }
            //当拖放到一个有物品的格子里面
            else if (surface.tag == Tags.inventory_item)
            {
                InventoryItemGrid grid1 = this.transform.parent.GetComponent<InventoryItemGrid>();
                InventoryItemGrid grid2 = surface.transform.parent.GetComponent<InventoryItemGrid>();

                int id = grid1.id;
                int num = grid1.num;
                grid1.SetId(grid2.id, grid2.num);
                grid2.SetId(id, num);
            }
            //拖到的快捷方式里面
            else if (surface.tag == Tags.shortcut)
            {
                surface.GetComponent<ShortCutGrid>().SetInventory(id);
            }

        }

        ResetPosition();
    }

    public void SetId(int id)
    {
        ObjectInfo info = ObjectsInfo._instance.GetObjectInfoById(id);
        sprite.spriteName = info.icon_name;
    }


    public void SetIconName(int id, string icon_Name)
    {
        sprite.spriteName = icon_Name;
        this.id = id;
    }

    void ResetPosition()
    {
        transform.localPosition = Vector3.zero;
    }


    public void OnHoverOver()
    {
        isHover = true;
    }

    public void OnHoverOut()
    {
        isHover = false;
    }

}
