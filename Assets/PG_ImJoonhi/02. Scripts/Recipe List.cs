using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    public class RecipeList : MonoBehaviour
    {
        [SerializeField] List<RecipeData> Recipe;
        [SerializeField] List<RecipeData> finishedRecipe;
    }

}