using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JH
{
    public class Food : MonoBehaviour
    {
        [Header("Map Recipes")]
        [SerializeField] RecipeList recipeList;
        public IngredientsObject init;
        public RecipeData curRecipe;

        public bool Dish;
        private List<IngredientsObject> ingredientList = new List<IngredientsObject>(4);
        private List<IngredientsObject> ingredientListDebug;
        private int included;

        private GameObject curDish;
        private GameObject CurrentObject;

        [Header("Debug")]
        [SerializeField] IngredientsObject DebugIngredient;

        private void Start()
        {
            for (int i = 0; i < 4; i++) ingredientList.Add(null);
            ingredientList[0] = init;
            included = 1;
            if (Dish)
            {
                curDish = Instantiate(recipeList.DishPrefab, gameObject.transform.position, Quaternion.identity);
                curDish.transform.SetParent(gameObject.transform, true);
                CurrentObject = (GameObject)Instantiate(curRecipe.Model, gameObject.transform.position, Quaternion.identity);
                CurrentObject.transform.SetParent(curDish.transform, true);
            }
            else
            {
                CurrentObject = (GameObject)Instantiate(curRecipe.Model, gameObject.transform.position, Quaternion.identity);
                CurrentObject.transform.SetParent(gameObject.transform, true);
            }
        }

        public void OnDish()
        {
            CurrentObject.transform.SetParent(curDish.transform, true);
        }

        public bool AddDish()
        {
            if (Dish)
                return false;
            Dish = true;
            curDish = Instantiate(recipeList.DishPrefab, gameObject.transform.position, Quaternion.identity);
            curDish.transform.SetParent(gameObject.transform, true);
            CurrentObject.transform.SetParent(curDish.transform, true);
            return true;
        }

        public bool IsAcceptable(int num)
        {
            if (included + num <= 4)
                return true;
            return false;
        }

        public void AddIngredient(IngredientsObject ingredient)
        {
            List<IngredientsObject> buf = ingredientList.ToList();
            buf[included] = ingredient;
            ingredientListDebug = buf.ToList();
            buf.Sort(0, included + 1, null);
            ingredientListDebug.Sort(0, included + 1, null);
            for (int i = 0; i < recipeList.Recipe.Count; i++)
            {
                Debug.Log(recipeList.Recipe[i].name);
                if (IsRecipe(buf, i))
                {
                    Debug.Log($"found recipe : {recipeList.Recipe[i].name}");
                    ingredientList = buf.ToList();
                    CurrentObject.SetActive(false);
                    Destroy(CurrentObject);
                    if (Dish)
                    {
                        CurrentObject = (GameObject)Instantiate(curRecipe.Model, gameObject.transform.position, Quaternion.identity);
                        CurrentObject.transform.SetParent(curDish.transform, true);
                    }
                    else
                    {
                        CurrentObject = (GameObject)Instantiate(curRecipe.Model, gameObject.transform.position, Quaternion.identity);
                        CurrentObject.transform.SetParent(gameObject.transform, true);
                    }
                    curRecipe = recipeList.Recipe[i];
                    included++;
                }
            }
        }

        private bool IsRecipe(List<IngredientsObject> ingredient, int index)
        {
            for (int i = 0; i < 4; i++)
            {
                Debug.Log(i);
                if (ingredient[i] == null)
                    if (recipeList.Recipe[index].ingredients[i] != null)
                        return false;
                if (ingredient[i] != null)
                    if (recipeList.Recipe[index].ingredients[i] == null)
                        return false;
                if (ingredient[i] == null && recipeList.Recipe[index].ingredients[i] == null)
                    continue;
                if (recipeList.Recipe[index].ingredients[i] != null)
                {
                    if (ingredient[i].ingredientsData.id != recipeList.Recipe[index].ingredients[i].id)
                        return false;
                    if (ingredient[i].IngState != recipeList.Recipe[index].ingredientsState[i])
                        return false;
                }
            }
            return true;
        }

        #region Debug
#if UNITY_EDITOR
        [ContextMenu("Add Ingredients")]
        public void DebugAdd()
        {
            if (IsAcceptable(1) && DebugIngredient != null)
                AddIngredient(DebugIngredient);
        }

        public void DebugOnDish()
        {
            OnDish();
        }
#endif
        #endregion
    }
}
