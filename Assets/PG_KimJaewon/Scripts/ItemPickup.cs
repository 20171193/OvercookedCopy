using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public void Pickup(Transform player)
    {
        transform.parent = player;
        GetComponent<Collider>().enabled = false;
        Debug.Log("아이템을 획득하였습니다");
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("아이템을 집었다 !");
            PickupPossible();
        }
    }

    private void PickupPossible()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);
        foreach(Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {

                Pickup(collider.transform);
                return;
            }
        }
    }
}
