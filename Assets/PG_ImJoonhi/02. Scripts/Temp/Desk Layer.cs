using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskLayer : MonoBehaviour
{
    private void Start()
    {
        Vector3Int curLocation = new Vector3Int((int)gameObject.transform.position.x, (int)gameObject.transform.position.y, (int)gameObject.transform.position.z);
        Debug.Log($"{gameObject.name} : {curLocation}");
    }
}
