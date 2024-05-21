using KIMJAEWON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] TrashBin table;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Item item = transform.GetComponentInChildren<Item>();
            Debug.Log(item);

            //table.Interactable(item);
        }
    }
}
