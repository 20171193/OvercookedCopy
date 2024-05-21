using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Jc
{
    public class PlayerAction : MonoBehaviour
    {
        // 오브젝트 머터리얼 변경
        private Material originTile;
        [SerializeField]
        private Material changeTile;

        // 현재 가장 가까운 테이블
        private Table nearestTable;
        // 현재 가장 가까운 아이템
        private Item nearestItem;

        // 현재 짚고 있는 아이템
        private Item pickedItem;

        // 부딪힌 테이블 모음
        private List<Table> tableList = new List<Table>();
        // 부딪힌 아이템 모음
        private List<Item> itemList = new List<Item>();

        // 오브젝트를 들고있는 상태인지?
        private bool isPickUp = false;

        private void SetTable(Table table)
        {
            // 이전 테이블 처리
            if (nearestTable != null)
                nearestTable.ExitPlayer();

            nearestTable = table;
            nearestTable.EnterPlayer();
        }

        private void SetItem(Item item)
        {
            // 이전 아이템 처리
            if (nearestItem != null)
                nearestItem.ExitPlayer();

            nearestItem = item;
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
            foreach(Table table in tableList)
            {
                // 할당된 테이블이 없을 경우
                if(minDist == -1)
                {
                    tmpTable = table;
                    minDist = (table.transform.position - transform.position).sqrMagnitude;
                    continue;
                }

                // 가까운 테이블 갱신
                float tempDist = (table.transform.position - transform.position).sqrMagnitude;
                if(tempDist < minDist)
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

        private void OnItemInteract(InputValue value)
        {
            // 아이템을 들고 있는 상태라면
            if(isPickUp)
            {
                // 내려놓기
                PutDown();
            }
            else
            {
                // 집기
                PickUp();
            }
        }

        private void TableInteract()
        {
            // 가까운 테이블이 없는 경우
            if (nearestTable == null) return;
        }

        private void PickUp()
        {
            // 상호작용 할 오브젝트가 없는 경우
            if (nearestItem == null && nearestTable == null) return; 

            // 가까운 아이템이 있는 경우 아이템 집기
            // 우선순위 적용


            // 가까운 아이템이 없는 경우 테이블 위의 아이템 집기

        }
        
        private void PutDown()
        {
            // 집고있는 아이템이 없는 경우
            if (!isPickUp || pickedItem == null) return;


        }

        private void OnTriggerEnter(Collider other)
        {
            // 부딪힌 테이블 세팅
            if (Manager.Layer.tableLM.Contain(other.gameObject.layer))
            {
                tableList.Add(other.GetComponent<Table>());
                SetTable(FindTable());
            }

            // 부딪힌 아이템 세팅
            // 현재 아이템을 들고있는 상태가 아닐 경우
            if(!isPickUp && Manager.Layer.itemLM.Contain(other.gameObject.layer))
            {
                itemList.Add(other.GetComponent<Item>());
                SetItem(FindItem());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // 벗어난 테이블 세팅
            if (Manager.Layer.tableLM.Contain(other.gameObject.layer))
            {
                Table temp = other.GetComponent<Table>();
                
                if (temp == null) // 컴포넌트 예외처리
                    return;
                if (!tableList.Contains(temp)) // 리스트 예외처리
                    return;

                if (temp == nearestTable)
                    nearestTable = null;

                tableList.Remove(temp);
                SetTable(FindTable());
            }

            // 부딪힌 아이템 세팅
            if (Manager.Layer.itemLM.Contain(other.gameObject.layer))
            {
                Item temp = other.GetComponent<Item>();

                if (temp == null) // 컴포넌트 예외처리
                    return;
                if (!itemList.Contains(temp)) // 리스트 예외처리
                    return;

                if (temp == nearestItem)
                    nearestItem = null;

                itemList.Remove(temp);
                SetItem(FindItem());
            }
        }
    }
}

