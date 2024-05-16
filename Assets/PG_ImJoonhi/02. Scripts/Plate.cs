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
        [SerializeField] FoodDish DebugFoodDish;

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
                if (recipeList.IsRecipe(buf, i))
                {
                    Debug.Log($"found recipe : {recipeList.Recipe[i].name}");
                    GenerateFoodDish(GeneratePoint, ingredientObject, recipeList.Recipe[i]);
                }
            }
        }

        public void IngredientIN(GameObject GeneratePoint, FoodDish foodDish)
        {
            // Debug.Log(recipeList.Recipe[i].name);
            if (!foodDish.Plate)
            {
                Debug.Log("Put Food on Plate");
                SwapFoodDish(gameObject, foodDish);
            }
        }

        private void GenerateFoodDish(GameObject GeneratePoint, IngredientsObject ingredientObject, RecipeData recipe)
        {
            FoodDish foodDish = Instantiate(foodDishPrefab, GeneratePoint.transform.position, Quaternion.identity);
            foodDish.recipeList = Manager_TEMP.recipemanager.recipeList;
            foodDish.init = ingredientObject;
            foodDish.curRecipe = recipe;
            foodDish.initPlate = true;
            foodDish.transform.SetParent(GeneratePoint.transform, true);
            Destroy(gameObject);
        }

        private void SwapFoodDish(GameObject GeneratePoint, FoodDish foodDish)
        {
            foodDish.AddPlate();
            foodDish.transform.position = GeneratePoint.transform.position;
            foodDish.transform.rotation = GeneratePoint.transform.rotation;
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

        [ContextMenu("[Debug]ADD FoodDish Plate")]
        public void DebugAddPlate()
        {
            IngredientIN(DebugGenPoint, DebugFoodDish);
        }
#endif
        #endregion

    }
}
