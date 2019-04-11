using UnityEngine;
using System.Collections;

public class MinimapCamera : MonoBehaviour
{

    public static MinimapCamera Instance; //declare this to global script

    [HideInInspector]
    public Transform Target;
    void Start()
    {

        GameObject hero;
        hero = GameObject.FindGameObjectWithTag("Player");
        Target = hero.GetComponent<Transform>();
    }


    // Update is called once per frame
    void Update()
    {

        //Follow player
        transform.position = new Vector3(Target.position.x, transform.position.y, Target.position.z);


    }

}
