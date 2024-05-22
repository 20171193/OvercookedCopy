using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    public class Plate : Item
    {
        [SerializeField] RecipeList recipeList;
        [SerializeField] IngredientList ingredientPrefabs;
        // public Rigidbody rigid;

        [Header("Prefabs")]
        [SerializeField] FoodDish foodDishPrefab;

        private void Start()
        {
            recipeList = Manager_TEMP.recipemanager.recipeList;
            ingredientPrefabs = Manager_TEMP.recipemanager.ingredientList;

            rigid = gameObject.GetComponent<Rigidbody>();
            collid = gameObject.GetComponent<Collider>();
            meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
            SetOriginMT();
            rigid.isKinematic = true;
            collid.enabled = false;
        }

        /// <summary>그릇에 놓을 재료가 1가지인 ingredientObject 프리팹의 경우 사용하는 함수.</summary>
        public bool IngredientIN(GameObject GeneratePoint, IngredientsObject ingredientObject)
        {
            if (ingredientObject.IngState == IngredientState.Original)
                return false;
            List<IngredientsObject> buf = new List<IngredientsObject>();
            for (int i = 0; i < 4; i++) buf.Add(null);
            buf[0] = ingredientPrefabs.Find(ingredientObject);
            Debug.Log("?");
            for (int i = 0; i < recipeList.Recipe.Count; i++)
            {
                // Debug.Log(recipeList.Recipe[i].name);
                if (recipeList.IsRecipe(buf, i))
                {
                    Debug.Log($"found recipe : {recipeList.Recipe[i].name}");
                    GenerateFoodDish(GeneratePoint, buf, recipeList.Recipe[i]);
                    return true;
                }
            }
            return false;
        }

        /// <summary>그릇에 놓을 재료가 2개 이상인 foodDish 프리팹인 경우 사용하는 함수.</summary>
        public bool IngredientIN(GameObject GeneratePoint, FoodDish foodDish)
        {
            // Debug.Log(recipeList.Recipe[i].name);
            if (!foodDish.Plate)
            {
                Debug.Log("Put Food on Plate");
                SwapFoodDish(gameObject, foodDish);
                return true;
            }
            return false;
        }

        private void GenerateFoodDish(GameObject GeneratePoint, List<IngredientsObject> ingredientObject, RecipeData recipe)
        {
            FoodDish foodDish = Instantiate(foodDishPrefab, GeneratePoint.transform.position, Quaternion.identity);
            // foodDish.recipeList = Manager_TEMP.recipemanager.recipeList;
            foodDish.init = ingredientObject;
            foodDish.curRecipe = recipe;
            foodDish.initPlate = true;
            foodDish.rigid.isKinematic = rigid.isKinematic;
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

        /*
        public void GoTo(GameObject GoPotint)
        {
            rigid.isKinematic = true;
            gameObject.transform.position = GoPotint.transform.position;
            gameObject.transform.rotation = GoPotint.transform.rotation;
            gameObject.transform.SetParent(GoPotint.transform, true);
        }
        public void Drop()
        {
            gameObject.transform.SetParent(null);
        }
        */

        #region Debug
#if UNITY_EDITOR
        [Header("Debug")]
        [SerializeField] GameObject DebugGenPoint;
        [SerializeField] IngredientsObject DebugIngredientObject;
        // [SerializeField] RecipeData DebugRecipe;
        [SerializeField] FoodDish DebugFoodDish;
        [SerializeField] GameObject DebugGameObject;

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

        [ContextMenu("[Debug]GoTo")]
        public void DebugGoTo()
        {
            GoTo(DebugGameObject);
        }

        [ContextMenu("[Debug]Drop")]
        public void DebugDrop()
        {
            Drop();
        }

#endif
        #endregion

    }
}
