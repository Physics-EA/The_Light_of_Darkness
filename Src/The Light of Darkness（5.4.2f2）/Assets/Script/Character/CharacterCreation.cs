using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CharacterCreation : MonoBehaviour
{

    public GameObject[] characterPrefabs;
    private GameObject[] characterGameObjects;
    private int selectIndex = 0;
    private int length;
    public UIInput nameInput;



    void Start()
    {
        length = characterPrefabs.Length;
        characterGameObjects = new GameObject[length];

        for (int i = 0; i < length; i++)
        {
            characterGameObjects[i] = GameObject.Instantiate(characterPrefabs[i], transform.position, transform.rotation) as GameObject;
        }

        UpdateCharacterShow();
    }


    void UpdateCharacterShow()
    {
        characterGameObjects[selectIndex].SetActive(true);
        for (int i = 0; i < length; i++)
        {
            if (i != selectIndex)
            {
                characterGameObjects[i].SetActive(false);
            }
        }

    }


    public void OnNextButtonClick()
    {
        selectIndex++;
        selectIndex %= length;
        UpdateCharacterShow();
    }


    public void OnPrevButtonClick()
    {
        selectIndex--;
        if (selectIndex == -1)
        {
            selectIndex = length - 1;
        }
        UpdateCharacterShow();
    }

    public void OnOkButtonClick()
    {
        PlayerPrefs.SetInt("SelectedCharacterIndex", selectIndex);
        PlayerPrefs.SetString("name", nameInput.value);
        //加载下一个场景
        SceneManager.LoadScene("03_play");
    }


}
