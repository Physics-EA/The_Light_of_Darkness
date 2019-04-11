using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour
{
    
    private void OnMouseEnter()
    {
        CursorManager._instance.SetNpcTalk();
    }

    private void OnMouseExit()
    {
        CursorManager._instance.SetNormal();
    }

}
