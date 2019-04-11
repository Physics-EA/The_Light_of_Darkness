using UnityEngine;
using System.Collections;

public class PressAnyKey : MonoBehaviour
{
    private bool isAnyKeyDown = false;
    private GameObject buttonContainer;

    private void Start()
    {
        buttonContainer = this.transform.parent.Find("buttonContainer").gameObject;
    }

    void Update()
    {
        if (isAnyKeyDown == false)
        {
            if (Input.anyKey)
            {
                ShowButton();
            }
        }
    }

    void ShowButton()
    {
        buttonContainer.SetActive(true);
        this.gameObject.SetActive(false);
        isAnyKeyDown = true;
    }


}
