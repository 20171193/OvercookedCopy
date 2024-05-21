using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class ResultUI : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text tipText;
    [SerializeField] TMP_Text totalScoreText;


    public void UpdateUI(int score, int tipScore, int totalScore)
    {
        // 결과창 출력
        scoreText.text = $"Score : {score}";
        tipText.text = $" Tip : {tipScore}";
        totalScoreText.text = $"Total Score : {totalScore}";
    }

}
