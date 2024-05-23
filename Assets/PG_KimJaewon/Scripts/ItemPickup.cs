using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KIMJAEWON
{
    public class ItemPickup : MonoBehaviour
    {
        private GameObject currentItem; // 현재 들고 있는 아이템을 추적
        public Transform dropPosition; // 아이템을 내려놓을 위치

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                // 현재 아이템을 들고 있지 않다면
                if (currentItem == null)
                {
                    Debug.Log("아이템을 집었다 !");
                    PickupPossible(); // 아이템을 들 수 있는지 확인하고 들기
                }
                else
                {
                    Debug.Log("아이템을 내려놓았다 !");
                    DropDownPossible(); // 아이템을 내려놓기
                }
            }
        }

        public void Pickup(Transform player)
        {
            currentItem = gameObject; // 현재 게임 오브젝트를 currentItem으로 설정
            transform.SetParent(player); // 아이템의 부모를 플레이어로 설정
                                         // Pickup(Collider.transform); // 아이템 위치를 플레이어 위치로 설정
            GetComponent<Collider>().enabled = false; // 아이템의 Collider를 비활성화
            GetComponent<Rigidbody>().isKinematic = true;
            Debug.Log("아이템을 획득하였습니다");
        }

        private void PickupPossible()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    Pickup(collider.transform);
                    return;
                }
            }
        }

        private void DropDownPossible()
        {
            currentItem.transform.SetParent(null); // 아이템의 부모를 해제
                                                   //currentItem.transform.position = dropPosition.position; // 지정된 위치에 아이템을 배치
            currentItem.GetComponent<Collider>().enabled = true; // 아이템의 Collider를 활성화
            currentItem.GetComponent<Rigidbody>().isKinematic = false;
            currentItem = null;
        }
    }
}