using JH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recipe_IGD : MonoBehaviour
{
    [Header("Recipe")]
    public RecipeData recipe;

    [Header("UI")]
    public RectTransform IngredientGroup;
    public RectTransform Image_BackGround;
    public Image finishedImage;
}
