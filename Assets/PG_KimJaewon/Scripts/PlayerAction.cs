using Jc;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KIMJAEWON
{
    public class PlayerAction : MonoBehaviour
    {
        [Header("에디터 세팅")]
        [SerializeField] PhotonView view;
        [SerializeField] Animator anim;
        [SerializeField] PlayerAudioController audioController;
        // 플레이어 아이템 소켓
        [SerializeField]
        private Transform itemSocket;

        [SerializeField]
        private bool debug; // 디버그 모드 Gizmos

        [SerializeField]
        private float range;
        [SerializeField, Range(0, 360)]
        private float angle;

        private float preAngle;
        private float cosAngle;
        private float CosAngle
        {
            get
            {
                if (preAngle == angle)
                    return cosAngle;

                preAngle = angle;
                cosAngle = Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad);
                return cosAngle;
            }
        }

        // 현재 가장 가까운 테이블
        [SerializeField]
        private Table nearestTable;
        // 현재 가장 가까운 아이템
        [SerializeField]
        private Item nearestItem;

        // 현재 짚고 있는 아이템
        [SerializeField]
        private Item pickedItem;

        // 부딪힌 테이블 모음
        private List<Table> tableList = new List<Table>();
        // 부딪힌 아이템 모음
        private List<Item> itemList = new List<Item>();

        // 오브젝트를 들고있는 상태인지?
        [Header("밸런싱")]
        [SerializeField]
        private bool isPickUp = false;

        // 내려놓기 성공했는지 실패했는지
        [SerializeField]
        private bool successPutDown;

        // 테이블과 상호작용중인지?
        [SerializeField]
        private bool onInteract;

        public void SetPickedItem(Item Pickeditem)
        {
            pickedItem = Pickeditem;
        }
        private void SetTable(Table table)
        {
            // 이전 테이블 처리
            if (nearestTable != null)
                nearestTable.ExitPlayer();

            nearestTable = table;
            if (nearestTable == null) return;
            nearestTable.EnterPlayer();
        }
        private void SetItem(Item item)
        {
            // 이전 아이템 처리
            if (nearestItem != null)
                nearestItem.ExitPlayer();

            nearestItem = item;
            if (nearestItem == null) return;
            nearestItem.EnterPlayer();
        }

        // 가장 가까운 테이블 도출
        private Table FindTable()
        {
            // 부딪힌 테이블이 없는 경우
            if (tableList.Count < 1) return null;
            // 부딪힌 테이블이 한 개일 경우
            if (tableList.Count == 1) return tableList[0];

            Table tmpTable = null;
            float minDist = -1f;
            // 여러 개일 경우
            foreach (Table table in tableList)
            {
                // 전방에 위치한 테이블 인 경우
                Vector3 dir = (table.transform.position - transform.position).normalized;
                if (Vector3.Dot(transform.forward, dir) >= CosAngle)
                {
                    return table;
                }

                // 할당된 테이블이 없을 경우
                if (minDist == -1)
                {
                    tmpTable = table;
                    minDist = (table.transform.position - transform.position).sqrMagnitude;
                    continue;
                }

                // 가까운 테이블 갱신
                float tempDist = (table.transform.position - transform.position).sqrMagnitude;
                if (tempDist < minDist)
                {
                    tmpTable = table;
                    minDist = tempDist;
                }
            }
            return tmpTable;
        }
        // 가장 가까운 아이템 도출
        private Item FindItem()
        {
            // 부딪힌 아이템이 없는 경우
            if (itemList.Count < 1) return null;
            // 부딪힌 아이템이 한 개일 경우
            if (itemList.Count == 1) return itemList[0];

            Item tmpItem = null;
            float minDist = -1f;
            // 여러 개일 경우
            foreach (Item item in itemList)
            {
                // 할당된 테이블이 없을 경우
                if (minDist == -1)
                {
                    tmpItem = item;
                    minDist = (item.transform.position - transform.position).sqrMagnitude;
                    continue;
                }

                // 가까운 테이블 갱신
                float tempDist = (item.transform.position - transform.position).sqrMagnitude;
                if (tempDist < minDist)
                {
                    tmpItem = item;
                    minDist = tempDist;
                }
            }
            return tmpItem;
        }

        private void OnItemControll(InputValue value)
        {
            // 아이템을 들고 있는 상태라면
            if (isPickUp)
            {
                Debug.Log("아이템을 들고 있습니다.");
                PutDown();
            }
            else
            {
                // 집기
                PickUp();
            }
        }

        private void OnTableInteract(InputValue value)
        {
            // 아이템을 들고 있는 상태라면
            if (isPickUp)
            {
                // 추후 던지기 구현
                return;
            }
            else
            {
                // 가까운 테이블이 없는 경우
                if (nearestTable == null) return;
                else
                {
                    // 테이블 상호작용
                    TableInteract();
                }
            }
        }
        private void TableInteract()
        {
            // 가까운 테이블이 없는 경우
            if (nearestTable == null) return;

            TableType type = nearestTable.TableType;

            // 상호작용이 불가능한 경우
            if (type == TableType.None) 
                return;
            if (!nearestTable.IsInteractable(pickedItem))
                return;

            // 각 테이블 별 상호작용
            switch (type)
            {
                // 도마 테이블
                case TableType.ChoppingTable:
                    ChoppingTable choppingTable = nearestTable.GetComponent<ChoppingTable>();
                    if (choppingTable == null) return;
                    choppingTable.Interactable();

                    // Chopping 애니메이션

                    audioController.PlaySFX(PlayerAudioController.SFXType.Chop);
                    break;
                // 재료상자 테이블
                case TableType.IngredientBox:
                    IngredientBox igdBox = nearestTable.GetComponent<IngredientBox>();
                    if (igdBox == null) return;
                    igdBox.Interactable(itemSocket.gameObject);

                    audioController.PlaySFX(PlayerAudioController.SFXType.PickUp);
                    anim.SetBool("IsPicked", true);
                    isPickUp = true;
                    break;

                // 싱크대 테이블
                case TableType.Sink:
                    // 일정시간 머무르기
                    audioController.PlaySFX(PlayerAudioController.SFXType.Wash);

                    break;
                    
            }
        }

        private void PickUp()
        {
            if (nearestItem == null && nearestTable == null)    // 상호작용 할 오브젝트가 없는 경우
                return;
            else if (nearestItem == null)    // 가까운 드랍 아이템이 없는 경우
            {
                pickedItem = nearestTable.PickUpItem();
                if (pickedItem == null) return;
            }
            else if (nearestTable == null)   // 가까운 테이블이 없는 경우
            {
                pickedItem = nearestItem;
            }
            // 아이템과 테이블 모두 존재할 경우
            // 거리계산 후 해당 오브젝트 PickUp
            else
            {
                float itemDist = (nearestItem.transform.position - transform.position).sqrMagnitude;
                float tableDist = (nearestTable.transform.position - transform.position).sqrMagnitude;

                // 떨어진 아이템 집기
                // 추후 조건추가 (아이템을 가지고있지 않은 테이블 일 경우)
                if (itemDist < tableDist)
                {
                    pickedItem = nearestItem;
                }
                // 테이블 위 아이템 집기
                else
                {
                    pickedItem = nearestTable.PickUpItem();
                    if (pickedItem == null) return;
                }
            }


            audioController.PlaySFX(PlayerAudioController.SFXType.PickUp);
            anim.SetBool("IsPicked", true);
            pickedItem.GoTo(itemSocket.gameObject);
            isPickUp = true;
            pickedItem.ExitPlayer();
        }

        private void PutDown()
        {
            // 집고있는 아이템이 없는 경우
            if (!isPickUp || pickedItem == null) return;

            // 가까운 테이블이 없는 경우
            if (nearestTable == null)
            {
                // 아이템 드롭 
                audioController.PlaySFX(PlayerAudioController.SFXType.PutDown);

                pickedItem.Drop();
                anim.SetBool("IsPicked", false);
                isPickUp = false;
                pickedItem = null;
            }
            // 테이블 위에 올려놓기
            else
            {
                Item putDownItem = pickedItem;
                successPutDown = nearestTable.PutDownItem(putDownItem);

                // 아이템을 올려놓을 수 있다면
                if (successPutDown)
                {
                    audioController.PlaySFX(PlayerAudioController.SFXType.PutDown);

                    anim.SetBool("IsPicked", false);
                    isPickUp = false;
                    pickedItem = null;
                }
                return;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (view.IsMine == false)
            {
                return;
            }

            // 부딪힌 테이블 세팅
            if (Manager.Layer.tableLM.Contain(other.gameObject.layer))
            {
                tableList.Add(other.GetComponent<Table>());
                SetTable(FindTable());
            }

            // 부딪힌 아이템 세팅
            // 현재 아이템을 들고있는 상태가 아닐 경우
            if (!isPickUp && Manager.Layer.dropItemLM.Contain(other.gameObject.layer))
            {
                itemList.Add(other.GetComponent<Item>());
                SetItem(FindItem());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (view.IsMine == false)
            {
                return;
            }
            // 벗어난 테이블 세팅
            if (Manager.Layer.tableLM.Contain(other.gameObject.layer))
            {
                Table temp = other.GetComponent<Table>();

                if (temp == null) // 컴포넌트 예외처리
                    return;
                if (!tableList.Contains(temp)) // 리스트 예외처리
                    return;

                tableList.Remove(temp);
                SetTable(FindTable());
                // 벗어난 테이블이 가장 가까운 테이블일 경우
                if (temp == nearestTable)
                {
                    nearestTable = null;
                    // 현재 테이블 내에서 상호작용 중일경우
                    if(onInteract)
                    {
                        // 상호작용 종료 이벤트처리
                    }
                }
            }

            // 부딪힌 아이템 세팅
            if (Manager.Layer.dropItemLM.Contain(other.gameObject.layer))
            {
                Item temp = other.GetComponent<Item>();

                if (temp == null) // 컴포넌트 예외처리
                    return;
                if (!itemList.Contains(temp)) // 리스트 예외처리
                    return;

                itemList.Remove(temp);
                SetItem(FindItem());
                if (temp == nearestItem)
                    nearestItem = null;
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (debug == false)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range);

            Vector3 rightDir = Quaternion.Euler(0, angle * 0.5f, 0) * transform.forward;
            Vector3 leftDir = Quaternion.Euler(0, angle * -0.5f, 0) * transform.forward;

            Debug.DrawRay(transform.position, rightDir * range, Color.cyan);
            Debug.DrawRay(transform.position, leftDir * range, Color.cyan);
        }

        public void OnTeleportIn()
        {
            anim.SetTrigger("OnTeleportIn");
            GetComponent<PlayerInput>().enabled = false;
        }
        public void OnTeleportOut()
        {
            anim.SetTrigger("OnTeleportOut");
            StartCoroutine(Extension.ActionDelay(0.2f, () => GetComponent<PlayerInput>().enabled = true));
        }
    }
}

