using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KIMJAEWON
{
    public class ObjectTrigger : MonoBehaviour
    {
        private Material originTile;
        [SerializeField]
        private Material changeTile;
        private Table nearestTable;

        private List<Table> tableList = new List<Table>();

        public Color color;

        private void Update()
        {
            CalculateDistance();
        }

        private void CalculateDistance()
        {
            if (tableList.Count < 1)
            {
                return;
            }
            else if (tableList.Count == 1)
            {
                if (nearestTable != null)
                {
                    nearestTable.ExitPlayer();
                }
                nearestTable = tableList[0];
            }
            else
            {
                float minDist = -1f;
                for (int i = 0; i < tableList.Count; i++)
                {
                    Table table = tableList[i];
                    float dist = Vector3.Distance(transform.position, table.transform.position);

                    if (minDist == -1f)
                    {
                        minDist = dist;
                        if (nearestTable != null)
                        {
                            nearestTable.ExitPlayer();
                        }
                        nearestTable = table;
                    }
                    else if (dist < minDist)
                    {
                        minDist = dist;
                        if (nearestTable != null)
                        {
                            nearestTable.ExitPlayer();
                        }
                        nearestTable = table;
                    }
                }
            }
            nearestTable.EnterPlayer();
        }
        private void OnTriggerEnter(Collider other)
        {
            // 충돌한 오브젝트의 태그가 Table이 맞다면
            if (other.CompareTag("Table"))
            {
                // 테이블 스크립트를 가져온다.
                Table table = other.GetComponent<Table>();
                tableList.Add(table);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Table"))
            {
                Table table = other.GetComponent<Table>();
                table.ExitPlayer();

                tableList.Remove(table);
            }
        }
    }
}