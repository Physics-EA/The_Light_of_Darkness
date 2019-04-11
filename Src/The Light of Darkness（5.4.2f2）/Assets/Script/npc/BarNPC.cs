using UnityEngine;
using System.Collections;

public class BarNPC : NPC
{

    public static BarNPC _instance;
    public TweenPosition questTween;
    public UILabel desLabel;

    public GameObject acceptBtnGo;
    public GameObject okBtnGo;
    public GameObject cancelBtnGo;

    public bool isInTask = false;//表示是否在任务中
    public int killCount = 0;//表示任务进度，已经杀死了几只小野狼

    private PlayStatus status;


    void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        status = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayStatus>();
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            this.GetComponent<AudioSource>().Play();
            if (isInTask)
            {
                ShowTaskProgress();
            }
            else
            {
                ShowTaskDes();
            }

            ShowQuest();
        }

    }

    void ShowQuest()
    {
        questTween.gameObject.SetActive(true);
        questTween.PlayForward();
    }

    void HideQuest()
    {
        questTween.PlayReverse();
    }

    public void OnKillWolf()
    {
        if (isInTask)
        {
            killCount++;
        }
    }

    void ShowTaskDes()
    {

        desLabel.text = "任务:\n杀死了10只狼\n\n奖励：\n1000金币";
        okBtnGo.SetActive(false);
        acceptBtnGo.SetActive(true);
        cancelBtnGo.SetActive(true);
    }

    void ShowTaskProgress()
    {
        desLabel.text = "任务:\n你 已经杀死了" + killCount + "\\10只狼\n\n奖励：\n1000金币";
        okBtnGo.SetActive(true);
        acceptBtnGo.SetActive(false);
        cancelBtnGo.SetActive(false);
    }

    public void OnCloseButtonClick()
    {
        HideQuest();
    }

    public void OnAcceptButtonClick()
    {

        ShowTaskProgress();
        isInTask = true;
    }

    public void OnOkButtonClick()
    {
        //完成任务
        if (killCount >= 10)
        {
            Inventory._instance.AddCoin(1000);
            killCount = 0;
            ShowTaskDes();
        }

        //没有完成任务
        else
        {
            HideQuest();
        }
    }

    public void OnCancelButtonClick()
    {
        HideQuest();
    }




}
