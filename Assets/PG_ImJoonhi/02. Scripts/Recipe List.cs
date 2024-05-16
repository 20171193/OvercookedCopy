using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    public class RecipeList : MonoBehaviour
    {
        public List<RecipeData> Recipe;
        public List<RecipeData> finishedRecipe;

        public GameObject PlatePrefab;

        private void Start()
        {
            Manager_TEMP.recipemanager.recipeList = this;
            // Manager.recipemanager.recipeList = this;
        }
    }

}