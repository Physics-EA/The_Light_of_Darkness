using UnityEngine;
using System.Collections;

public class PlayerDir : MonoBehaviour
{
    public GameObject effect_click_prefab;
    public Vector3 targetPosition = Vector3.zero;
    private bool isMoving = false;//表示鼠标是否按下
    private PlayerMove playerMove;
    private PlayerAttack attack;



    private void Start()
    {
        targetPosition = transform.position;
        playerMove = this.GetComponent<PlayerMove>();
        attack = this.GetComponent<PlayerAttack>();
    }


    void Update()
    {
        if (attack.state == PlayerState.Death) return;

        if (attack.isLockingTarget == false && Input.GetMouseButtonDown(0) && UICamera.hoveredObject == null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            bool isCollider = Physics.Raycast(ray, out hitInfo);
            if (isCollider && hitInfo.collider.tag == Tags.ground)
            {
                isMoving = true;
                ShowColickEffect(hitInfo.point);
                LookAtTarget(hitInfo.point);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isMoving = false;
        }


        if (isMoving)
        {
            //得到要移动的目标位置
            //让主角朝向目标位置
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            bool isCollider = Physics.Raycast(ray, out hitInfo);
            if (isCollider && hitInfo.collider.tag == Tags.ground)
            {
                LookAtTarget(hitInfo.point);
            }
        }
        else
        {
            if (playerMove.isMoving)
            {
                LookAtTarget(targetPosition);
            }
        }

    }

    //实例化出来点击的效果
    void ShowColickEffect(Vector3 hitPoint)
    {
        hitPoint = new Vector3(hitPoint.x, hitPoint.y + 0.05f, hitPoint.z);
        GameObject.Instantiate(effect_click_prefab, hitPoint, Quaternion.identity);
    }

    //让主角朝向目标位置
    void LookAtTarget(Vector3 hitPoint)
    {
        targetPosition = hitPoint;
        targetPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        this.transform.LookAt(targetPosition);
    }


}
