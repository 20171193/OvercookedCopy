using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Kyungmin
{
    public class InGameFlow : MonoBehaviour
    {
        [SerializeField] TimerBar timeBar;
        [SerializeField] ScoreUI scoreUI;
        [SerializeField] GameObject resultUI;

        [SerializeField] TMP_Text scoreText;    // Score나타내는 Text
        [SerializeField] TMP_Text tipText;      // Tip나타내는 Text

        private void Start()
        {
            // timeBar.OnTimeOut에 GameTimeOut 함수 등록
            timeBar.OnTimeOut += GameTimeOut;
        }

        public void GameTimeOut()
        {
            // 결과창 UI 키기
            resultUI.SetActive(true);
            scoreText.text = $"Final Score : {scoreUI.score}";
            tipText.text = $" Tip : {scoreUI.tipScore}";
        }

        public void OnResultExit(InputValue value)
        {
            // 결과창 UI 끄기
            resultUI.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
