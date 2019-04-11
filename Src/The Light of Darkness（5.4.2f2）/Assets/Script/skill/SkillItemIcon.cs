using UnityEngine;
using System.Collections;

public class SkillItemIcon : UIDragDropItem
{
    private int skillId;

    protected override void OnDragDropStart()
    {
        base.OnDragDropStart();

        skillId = transform.parent.GetComponent<SkillItem>().id;
        transform.parent = transform.root;
        this.GetComponent<UISprite>().depth = 40;
    }

    protected override void OnDragDropRelease(GameObject surface)
    {
        base.OnDragDropRelease(surface);
        //当一个技能拖放到快捷方式上的时候
        if (surface != null && surface.tag == Tags.shortcut)
        {
            surface.GetComponent<ShortCutGrid>().SetSkill(skillId);
        }
    }
}
