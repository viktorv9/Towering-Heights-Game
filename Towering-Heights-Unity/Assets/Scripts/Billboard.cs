using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private GameObject cam;

    void Start()
    {
        cam = GameObject.FindWithTag("VirtualCamera");
    }

    void Update()
    {
        var lookPos = cam.transform.position - transform.position;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = rotation;
    }
}
