using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

namespace Kyungmin
{ 
    public class TimerBar : MonoBehaviour
    {
        [SerializeField] Slider gauge;        // 게이지바
        [SerializeField] TMP_Text text;

        [SerializeField] float gameTime;      // 게임진행시간


        private void Awake()
        {
            StartCoroutine(OrderTimer()); // 타이머 시작
        }
        private void OnEnable()
        {
            // 게임진행시간이 mawValue가 되게 설정
            gauge.maxValue = gameTime;
        }

        IEnumerator OrderTimer()
        {
            // 
            float curTime = gameTime;
            yield return null;

            while (curTime > 0)
            {
                curTime -= Time.deltaTime;

                // 분, 초 계산
                int minute = (int)curTime / 60;
                int second = (int)curTime % 60;

                // 게이지바를 현재시간에 맞춤 
                gauge.value = curTime;

                // 시간을 00:00 형식으로 표시
                text.text = minute.ToString("00") + ":" + second.ToString("00");
                yield return null;

                if (curTime <= 0)     // 현재 시간이 0이거나 0보다 작을 경우 종료
                {
                    curTime = 0;
                    Debug.Log("시간 종료");
                    Time.timeScale = 0;     // 일시 정지

                    yield break;
                }
            }
        }
    }
}