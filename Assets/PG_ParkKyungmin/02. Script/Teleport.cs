using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Kyungmin
{
    public class Teleport : MonoBehaviour
    {
        [SerializeField] Transform targetPos;   // Player가 순간이동 할 위치
        [SerializeField] bool isPlayer;
        [SerializeField] LayerMask playerLayer;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) // Player의 태그를 확인하여 감지하는지 확인
            {
                isPlayer = true;
                Debug.Log("Player인지");

                // Player를 감지했을 때만 이동
                other.transform.position = targetPos.position;
                Debug.Log("Player순간이동");
            }
        }
    }

}