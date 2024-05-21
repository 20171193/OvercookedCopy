using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms;

namespace Kyungmin
{
    public class InGameFlow : MonoBehaviour
    {
        [SerializeField] TimerBar timerBar;
        [SerializeField] ScoreUI scoreUI;
        [SerializeField] ResultUI resultUI;

        [SerializeField] TMP_Text scoreText;    // Score나타내는 Text
        [SerializeField] TMP_Text tipText;      // Tip나타내는 Text


        [SerializeField] float gameTime;
        [SerializeField] int curScore;          // 현재 점수
        [SerializeField] int curTip;            // 현재 팁
        [SerializeField] int totalScore;        // 총 점수
        [SerializeField] int multipleTip;       // 곱해지는 팁
        const int MaxMultipleTip = 4;
        const int Tip = 5;

        private void Start()
        {
            timerBar.gauge.maxValue = gameTime;
            StartCoroutine(OrderTimer());

        }
        public void RecipeResult(int score)
        {
            // 레시피 제출
            // 스코어, 팁 올려주기

            // 팁계산
            int addTip = Tip * multipleTip;

            // multipleTip이 4보다 작을때 
            if (multipleTip < MaxMultipleTip)
            {
                multipleTip++;

                scoreUI.DisableFire();
                if (multipleTip == MaxMultipleTip)
                {
                    scoreUI.EnableFire();
                }
            }

            scoreUI.GetCoin();

            curScore += score;                  // 누적된 score가 현재 점수가 됨
            curTip += addTip;                   // 누적된 tip점수가 현재 tip이 됨
            totalScore += addTip + score;       // 총 점수

            scoreUI.UpdateUI(totalScore, multipleTip);
        }

        public void GameTimeOut()
        {
            // n초 뒤에 결과창 UI 키기
            resultUI.gameObject.SetActive(true);
            // 결과창UI에 점수 Update하기 
            resultUI.UpdateUI(curScore, curTip, totalScore);
        }

        public void OnResultExit(InputValue value)
        {
            // 결과창 UI 끄기
            resultUI.gameObject.SetActive(false);
        }

        IEnumerator OrderTimer()
        {
            float curTime = gameTime;
            int prevTime = Mathf.FloorToInt(curTime);
            yield return null;


            while (curTime > 0)
            {
                curTime -= Time.deltaTime;

                // 내림 한 시간
                int floorTime = Mathf.FloorToInt(curTime);

                // 시간 갱신
                if(prevTime > floorTime)
                {
                    prevTime = floorTime;
                    timerBar.UpdateUI(floorTime);

                    if((floorTime % 30 == 0) && (floorTime > 31 ))
                    {
                        timerBar.GetAlarm();                      
                    }
                    else if (floorTime == 30)
                    {
                        timerBar.EnableAlarm();
                    }
                }
                yield return null;
            }

            curTime = 0;
            Debug.Log("시간 종료");
            Time.timeScale = 0;     // 일시 정지
            timerBar.DisableAlarm();
            GameTimeOut();

        }
    }
}
