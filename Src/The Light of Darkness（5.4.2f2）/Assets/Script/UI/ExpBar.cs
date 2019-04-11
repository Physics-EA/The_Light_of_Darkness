using UnityEngine;
using System.Collections;

public class ExpBar : MonoBehaviour
{
    public static ExpBar _instance;
    private UISlider progressBar;

    private void Awake()
    {
        _instance = this;
        progressBar = this.GetComponent<UISlider>();
    }


    public void SetValue(float value)
    {
        progressBar.value = value;
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
