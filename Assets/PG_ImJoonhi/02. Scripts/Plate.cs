using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

        private void Start()
        {
            recipeList = Manager_TEMP.recipemanager.recipeList;
        }

        public void IngredientIN(GameObject GeneratePoint, IngredientsObject ingredientObject)
        {
            List<IngredientsObject> buf = new List<IngredientsObject>();
            for (int i = 0; i < 4; i++) buf.Add(null);
            buf[0] = ingredientObject;
            Debug.Log("?");
            for (int i = 0; i < recipeList.Recipe.Count; i++)
            {
                // Debug.Log(recipeList.Recipe[i].name);
                Debug.Log("!");
                if (recipeList.IsRecipe(buf, i))
                {
                    Debug.Log($"found recipe : {recipeList.Recipe[i].name}");
                    GenerateFoodDish(GeneratePoint, ingredientObject, recipeList.Recipe[i]);
                }
            }
        }

        private void GenerateFoodDish(GameObject GeneratePoint, IngredientsObject ingredientObject, RecipeData recipe)
        {
            FoodDish foodDish = Instantiate(foodDishPrefab, GeneratePoint.transform.position, Quaternion.identity);
            foodDish.recipeList = Manager_TEMP.recipemanager.recipeList;
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
            IngredientIN(DebugGenPoint, DebugIngredientObject);
            //GenerateFoodDish(DebugGenPoint, DebugIngredientObject, DebugRecipe);
        }
#endif
        #endregion

    }
}
