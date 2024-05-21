using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemPickup : MonoBehaviour
{
    public Transform dropPosition; // 아이템을 떨어뜨릴 위치
    private Transform currentItem; // 현재 플레이어가 가지고 있는 아이템
    public List<Transform> tableList = new List<Transform>(); // 테이블 리스트

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (currentItem == null)
            {
                PickupPossible();
            }
            else if (currentItem != null)
            {
                DropDownPossible();
            }
        }
    }

    private void PickupPossible()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f, Manager.Layer.interactableLM);
        float minDist = float.MaxValue;
        Transform closestItem = null;

        foreach (Collider collider in colliders)
        {
            if (Manager.Layer.tableLM.Contain(collider.gameObject.layer))
            {
                tableList.Add(collider.transform); // 테이블 리스트에 추가
                continue;
            }

            float dist = Vector3.Distance(transform.position, collider.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closestItem = collider.transform;
            }
        }

        if (closestItem != null)
        {
            Debug.Log("가장 가까운 아이템을 인식했습니다.");
            Pickup(closestItem);
        }
        else
        {
            Debug.Log("주변에 아이템이 없습니다.");
        }
    }

    public void Pickup(Transform item)
    {
        item.SetParent(transform); // 아이템의 부모를 플레이어로 설정
        item.GetComponent<Collider>().enabled = false; // 아이템의 Collider를 비활성화
        item.GetComponent<Rigidbody>().isKinematic = true; // 아이템의 Rigidbody를 Kinematic 해제
        Debug.Log("아이템을 획득했습니다.");
        currentItem = item;
    }

    private void DropDownPossible()
    {
        if (currentItem == null)
        {
            Debug.LogWarning("내려놓을 아이템이 없습니다.");
            return;
        }

        if (tableList.Count == 0)
        {
            Debug.LogWarning("테이블이 없습니다.");
            return;
        }

        Transform closestTable = FindClosestTable();
        if (closestTable != null)
        {
            currentItem.SetParent(null); // 아이템의 부모를 해제
            currentItem.position = closestTable.position; // 가장 가까운 테이블 위치에 아이템을 배치
            currentItem.GetComponent<Collider>().enabled = true; // 아이템의 Collider를 활성화
            currentItem.GetComponent<Rigidbody>().isKinematic = false; // 아이템의 Rigidbody를 Kinematic 해제
            currentItem = null; // 현재 아이템을 null로 설정
        }
        else
        {
            Debug.LogWarning("가장 가까운 테이블을 찾을 수 없습니다.");
        }
    }

    private Transform FindClosestTable()
    {
        Transform closestTable = null;
        float minDist = float.MaxValue;

        foreach (Transform table in tableList)
        {
            float dist = Vector3.Distance(transform.position, table.position);
            if (dist < minDist)
            {
                minDist = dist;
                closestTable = table;
            }
        }

        return closestTable;
    }
}
