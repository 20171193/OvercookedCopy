using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    public class Plate : MonoBehaviour, IPickable
    {
        [SerializeField] RecipeList recipeList;

        [Header("Prefabs")]
        [SerializeField] FoodDish foodDishPrefab;

        [Header("Debug")]
        [SerializeField] GameObject DebugGenPoint;
        [SerializeField] IngredientsObject DebugIngredientObject;
        [SerializeField] RecipeData DebugRecipe;

        private void GenerateFoodDish(GameObject GeneratePoint, IngredientsObject ingredientObject, RecipeData recipe)
        {
            FoodDish foodDish = Instantiate(foodDishPrefab, gameObject.transform.position, Quaternion.identity);
            foodDish.init = ingredientObject;
            foodDish.curRecipe = recipe;
            foodDish.initPlate = true;
            Destroy(gameObject);
        }


        #region Debug
#if UNITY_EDITOR
        [ContextMenu("[Debug]Generate FoodDish")]
        public void DebugGenerate()
        {
            GenerateFoodDish(DebugGenPoint, DebugIngredientObject, DebugRecipe);
        }
#endif
        #endregion

    }
}
