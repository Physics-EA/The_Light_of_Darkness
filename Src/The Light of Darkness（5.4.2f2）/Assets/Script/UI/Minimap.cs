using UnityEngine;
using System.Collections;

public class Minimap : MonoBehaviour
{

    private Camera minimapCamera;

    private void Start()
    {
        minimapCamera = GameObject.FindGameObjectWithTag(Tags.minimap).GetComponent<Camera>();
    }

    /// <summary>
    /// 放大地图
    /// </summary>
    public void OnZoomInClick()
    {
        minimapCamera.orthographicSize--;
    }


    /// <summary>
    /// 缩小地图
    /// </summary>
    public void OnZoomOutClick()
    {
        minimapCamera.orthographicSize++;
    }
}
