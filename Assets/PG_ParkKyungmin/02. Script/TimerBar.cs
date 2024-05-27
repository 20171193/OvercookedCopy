using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

namespace Kyungmin
{ 
    public class TimerBar : MonoBehaviour
    {
        public Slider gauge;        // 게이지바
        [SerializeField] TMP_Text text;
        [SerializeField] Animator animator;

        public void UpdateUI(int time)
        {
            // 게이지바를 현재시간에 맞춤 
            gauge.value = time;

            // 분, 초 계산
            int minute = time / 60;
            int second = time % 60;

            // 시간을 00:00 형식으로 표시
            text.text = minute.ToString("00") + ":" + second.ToString("00");           

        }

        public void GetAlarm()
        {
            // 30초 단위로 작동되는 애니메이션
            animator.SetTrigger("OnAlarm");
        }

        public void EnableAlarm()
        {
            // 30초 남았을때 애니메이션 재생   
            animator.SetBool("IsPlayLoop", true);
        }

        public void DisableAlarm()
        {
            // 애니메이션 끄기 
            animator.SetBool("IsPlayLoop", false);
        }
    }
}