using JH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Recipe_IGD : MonoBehaviour
{
    [Header("Recipe")]
    public RecipeData recipe;
    public RecipeOrder recipeOrder;

    [Header("UI")]
    public RectTransform IngredientGroup;
    public RectTransform Image_BackGround;
    public Image finishedImage;
    [SerializeField] Slider gauge;        // 게이지바
    [SerializeField] float time;          // 총 시간


    private void OnEnable()
    {
        // 게이지 최대값을 총 시간으로 설정
        gauge.maxValue = time;
        // 게이지 현재값을 총 시간으로 설정
        gauge.value = time;

        StartCoroutine(RecipeTime());
    }

    IEnumerator RecipeTime()
    {
        while (time > 0)
        {
            // 시간 감소
            time -= Time.deltaTime;

            // 게이지바를 현재 시간에 맞춤
            gauge.value = time;

            yield return null;
        }

        // 게이지 최종 업데이트
        gauge.value = 0;

        Debug.Log("레시피 종료");
        yield return null;
    }
}


