using UnityEngine;
using System.Collections;

public class MovieCamera : MonoBehaviour
{

    public float speed = 10;
    private float endZ = -20;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z < endZ)
        {
            transform.Translate(new Vector3(0,0,1) * speed * Time.deltaTime,Space.World);
        }
    }
}
