using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Kyungmin
{
    public class Teleport : MonoBehaviour
    {
        [SerializeField] GameObject outObj;     // 텔레포트 출구
        [SerializeField] float exitDistance;    // Player와 출구의 거리

        private void OnTriggerEnter(Collider other)
        {
            // 매개변수를 통해 감지된 collider를 가지고 있는 gameObject(= Player)를 텔레포트 시킴
            TeleportStart(other.gameObject);
        }

        private void TeleportStart(GameObject player)
        {

            // Player를 텔레포트 출구로 바로 이동 하게 되면
            // Player가 무한으로 텔레포트를 타는 문제가 발생하게 되어
            // exitDistance를 두어서 출구가 아닌 출구에서 exitDistance만큼 앞으로 텔레포트 하게 함
            player.transform.position = outObj.transform.position - outObj.transform.forward * exitDistance;
        }
    }
}

