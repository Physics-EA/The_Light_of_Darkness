using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour
{
    private GameObject player;
    private Vector3 offsetPositon;
    public float distance = 0;
    public float scrollSpeed = 1;
    private bool isRotating = false;
    public float rotateSpeed = 1;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(Tags.player);
        transform.LookAt(player.transform.position);
        offsetPositon = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = offsetPositon + player.transform.position;
        RotateView();
        ScrollView();

    }


    void ScrollView()
    {
        
        distance = offsetPositon.magnitude;
        distance += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        distance = Mathf.Clamp(distance, 2, 10);
        offsetPositon = offsetPositon.normalized * distance;
    }

    void RotateView()
    {
        //Input.GetAxis("Mouse X");
        //Input.GetAxis("Mouse Y");
        if (Input.GetMouseButtonDown(1))
        {
            isRotating = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            isRotating = false;
        }

        if (isRotating&& UICamera.hoveredObject == null)
        {
            transform.RotateAround(player.transform.position, player.transform.up, rotateSpeed * Input.GetAxis("Mouse X"));

            Vector3 originalPos = transform.position;
            Quaternion originalRotation = transform.rotation;

            transform.RotateAround(player.transform.position, transform.right, -rotateSpeed * Input.GetAxis("Mouse Y"));

            float x = transform.eulerAngles.x;

            if (x < 10 || x > 60)
            {
                transform.position = originalPos;
                transform.rotation = originalRotation;
            }

        }

        offsetPositon = transform.position - player.transform.position;

    }

}
