
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Kyungmin
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] Slider gauge;        // 게이지바
        [SerializeField] TMP_Text scoreText;
        [SerializeField] TMP_Text tipText;
        [SerializeField] Animator animator;

        [SerializeField] int tip = 1;       // 팁의 갯수
        [SerializeField] int maxTip = 4;    // 최대로 받을수 있는 팁
        public int tipScore = 0;    // 팁 점수
        public int score = 0;       // 현재 점수

        private void OnEnable()
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            scoreText.text = score.ToString();

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

        private void OnGetScore(InputValue value)
        { 
            score++;
            UpdateUI();
            animator.Play("Coin UI");
        }

        private void OnGetTip(InputValue value)
        {
            // maxTip(=4) 보다 tip이 적다면 tip이 더 올라갈 수 있음
            if (tip < maxTip)
            {
                tip++;
            }
            tipScore += tip * 10;

            score += tipScore;

            UpdateUI();
        }
    }
}