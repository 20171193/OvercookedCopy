
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

namespace Kyungmin
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] Slider gauge;        // 게이지바
        [SerializeField] TMP_Text totalscoreText;
        [SerializeField] TMP_Text tipText;


        private void OnEnable()
        {
            // score와 tip의 초기값 설정 
            UpdateUI(0, 0);
        }

        public void UpdateUI(int score, int tip)
        {

            totalscoreText.text = score.ToString();

            // 게이지의 value가 tip의 수치에 맞게 오를 수 있게함
            gauge.value = tip;

            if (tip == 0)
            {
                // tip이 0일때는 text가 보이지 않게
                tipText.text = "";
            }
            else
            {
                // tip이 0이 아닐때는 tip의 수치를 나타내줌
                tipText.text = $"Tip X {tip}";
            }
        }
    }
}