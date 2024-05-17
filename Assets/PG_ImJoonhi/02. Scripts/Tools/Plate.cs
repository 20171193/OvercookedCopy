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

        /// <summary>그릇에 놓을 재료가 1가지인 ingredientObject 프리팹의 경우 사용하는 함수.</summary>
        public void IngredientIN(GameObject GeneratePoint, IngredientsObject ingredientObject)
        {
            if (ingredientObject.IngState != IngredientState.Sliced)
                return;
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
                    GenerateFoodDish(GeneratePoint, buf, recipeList.Recipe[i]);
                }
            }
        }

        /// <summary>그릇에 놓을 재료가 2개 이상인 foodDish 프리팹인 경우 사용하는 함수.</summary>
        public void IngredientIN(GameObject GeneratePoint, FoodDish foodDish)
        {
            // Debug.Log(recipeList.Recipe[i].name);
            if (!foodDish.Plate)
            {
                Debug.Log("Put Food on Plate");
                SwapFoodDish(gameObject, foodDish);
            }
        }

        private void GenerateFoodDish(GameObject GeneratePoint, List<IngredientsObject> ingredientObject, RecipeData recipe)
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

        public void GoTo(GameObject GoPotint)
        {
            gameObject.transform.SetParent(GoPotint.transform, true);
        }
        public void Drop()
        {
            gameObject.transform.SetParent(null);
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
