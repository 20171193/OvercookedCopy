using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    Transform cam;

    private void Start()
    {
        cam = Camera.main.transform;
    }

    private void Update()
    {
        transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);
    }
}
