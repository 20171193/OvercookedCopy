using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Jc;
using Photon.Realtime;

namespace Kyungmin
{
    public class InGameFlow : MonoBehaviourPunCallbacks, IPunObservable
    {
        [SerializeField] TimerBar timerBar;
        [SerializeField] ScoreUI scoreUI;
        [SerializeField] ResultUI resultUI;

        [SerializeField] TMP_Text scoreText;    // Score나타내는 Text
        [SerializeField] TMP_Text tipText;      // Tip나타내는 Text

        [SerializeField] float gameTime;
        [SerializeField] int curScore;          // 현재 점수
        [SerializeField] int curTip;            // 현재 팁
        [SerializeField] int multipleTip;       // 곱해지는 팁
        public int MultipleTip       // Get Set Property
        {
            get 
            { 
                return multipleTip; 
            }
            set
            {
                multipleTip = value;
                if(multipleTip == 4)
                {
                    // 불 애니메이션 재생
                    scoreUI.EnableFire();
                }
                else
                {
                    // 불 애니메이션 끔
                    scoreUI.DisableFire();
                }
            }
        }
        [SerializeField] int totalScore;        // 총 점수
        const int MaxMultipleTip = 4;
        const int Tip = 5;

        private float curTime;
        private bool isGameRunning = true;

        private void Start()
        {
            timerBar.gauge.maxValue = gameTime;
            curTime = gameTime;
            StartCoroutine(OrderTimer());
        }

        private void Update()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                // 마스터 클라이언트는 주기적으로 타이머 값을 업데이트
                photonView.RPC("UpdateTimer", RpcTarget.All, curTime);
            }
        }


        public void RecipeResult(int score)
        {
            // 레시피 제출 

            // 팁계산
            int addTip = Tip * MultipleTip;

            // multipleTip이 4보다 작을때 
            if (MultipleTip < MaxMultipleTip)
            {
                MultipleTip++;

                scoreUI.DisableFire();
                if (MultipleTip == MaxMultipleTip)
                {
                    scoreUI.EnableFire();
                }
            }

            curScore += score;                  // 누적된 score가 현재 점수가 됨
            curTip += addTip;                   // 누적된 tip점수가 현재 tip이 됨
            totalScore += addTip + score;       // 총 점수

            // PhotonNetwork를 사용하여 점수 업데이트 요청
            photonView.RPC("RequestScoreUpdate", RpcTarget.MasterClient, totalScore, MultipleTip);
        }

        [PunRPC]
        public void RequestScoreUpdate(int totalScore, int multipleTip, PhotonMessageInfo info)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                // 방장이 점수를 업데이트
                UpdateScore(totalScore, multipleTip);

                // 모든 클라이언트에게 업데이트된 점수를 전송
                photonView.RPC("UpdateScore", RpcTarget.All, totalScore, multipleTip);
            }
        }

        [PunRPC]
        public void UpdateScore(int totalScore, int multipleTip)
        {
            this.totalScore = totalScore;
            this.MultipleTip = multipleTip;


            scoreUI.GetCoin();
            scoreUI.UpdateUI(totalScore, multipleTip);
        }

        [PunRPC]
        public void UpdateTimer(float updatedTime)
        {
            curTime = updatedTime;
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
            int prevTime = Mathf.FloorToInt(curTime);
            yield return null;


            while (curTime > 0 && isGameRunning)
            {
                curTime -= Time.deltaTime;

                // 내림한 시간
                int floorTime = Mathf.FloorToInt(curTime);

                // 시간 갱신
                if (prevTime > floorTime)
                {
                    prevTime = floorTime;
                    timerBar.UpdateUI(floorTime);

                    // 30초 마다 애니매이션이 작동하게 하면서 마지막 30초에는 실행하지 않음
                    if ((floorTime % 30 == 0) && (floorTime > 31))
                    {
                        timerBar.GetAlarm();
                    }
                    // 마지막 30초에는 지속되는 애니메이션이 실행되게
                    else if (floorTime == 30)
                    {
                        timerBar.EnableAlarm();
                    }
                }
                yield return null;
            }

            curTime = 0;
            isGameRunning = false;
            Debug.Log("시간 종료");
            Time.timeScale = 0;     // 일시 정지
            timerBar.DisableAlarm();
            GameTimeOut();

        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(curScore);
                stream.SendNext(curTip);
                stream.SendNext(totalScore);
                stream.SendNext(MultipleTip);
            }
            else // if(stream.IsReading)
            {
                curScore = (int)stream.ReceiveNext();
                curTip = (int)stream.ReceiveNext();
                totalScore = (int)stream.ReceiveNext();
                MultipleTip = (int)stream.ReceiveNext();
                scoreUI.UpdateUI(totalScore, MultipleTip);
                scoreUI.GetCoin();
            }
        }
    }
}
