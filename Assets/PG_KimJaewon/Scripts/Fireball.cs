using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


namespace KJW
{
    public class Fireball : MonoBehaviour
    {
        [SerializeField] Vector3 targetPos;
        [SerializeField] float throwTime;
        private Collider collider;
        //private Slider slider;

        [SerializeField] Slider Slider;
         private void Awake()
        {
            // Fireball 의 콜라이더를 가져와서
            collider = GetComponent<Collider>();
            // Fireball 의 콜라이더가 처음부터 켜져있다면 플레이어가 받는 힘에 의해서 바닥으로 쳐박힌다.
            // 그래서 땅에 떨어지고 난 다음 Collider를 활성화시킨다.
            collider.enabled = false;

            //Slider slider = GetComponent<Slider>();
            //slider.enabled = false;
        }

        public void SetTargetPos(Vector3 position)
        {
            targetPos = position;

            StartCoroutine(ThrowFireballRoutine());
        }

        IEnumerator ThrowFireballRoutine()
        {
            // 포지션의 스타트포인트다.
            Vector3 startPoint = transform.position;
            // EndPoint이다 
            Vector3 endPoint = targetPos;

            // x, y, z speed 지정값 지정해준다.
            float xSpeed = (endPoint.x - startPoint.x) / throwTime;
            float zSpeed = (endPoint.z - startPoint.z) / throwTime;
            float ySpeed = -1 * (0.5f * Physics.gravity.y * throwTime * throwTime + startPoint.y) / throwTime;

            float curTime = 0f;
            while (curTime < throwTime)
            {
                curTime += Time.deltaTime;

                transform.position += new Vector3(xSpeed, ySpeed, zSpeed) * Time.deltaTime;
                ySpeed += Physics.gravity.y * Time.deltaTime;

                yield return null;
            }

            transform.position = endPoint;
            collider.enabled = true;
            //Slider.enabled = true;
            yield return null;

            //if(timerSlider != null)
            //{
            //    timerSlider.gameObject.SetActive(true);
            //    StartCorutine(SliderTimerRoutin());
            //}

            // 떨어지고 나서 할 일
        }
    }
}