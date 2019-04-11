using UnityEngine;
using System.Collections;

public class GameLoad : MonoBehaviour
{

    public GameObject magicianPrefab;
    public GameObject swordmanPrefab;

    void Awake()
    {
        int selectdIndex = PlayerPrefs.GetInt("SelectedCharacterIndex");
        string name = PlayerPrefs.GetString("name");

        GameObject go = null;
        if (selectdIndex == 0)
        {
            go = GameObject.Instantiate(magicianPrefab) as GameObject;
        }
        else if (selectdIndex == 1)
        {
            go = GameObject.Instantiate(swordmanPrefab) as GameObject;
        }
        go.GetComponent<PlayStatus>().name = name;
    }
}
