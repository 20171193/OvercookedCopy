using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JH
{
    public class Food : MonoBehaviour
    {
        [Header("Map Recipes")]
        [SerializeField] RecipeList recipeList;
        public RecipeData init;

        private bool Dish;
        private List<IngredientsObject> ingredientList = new List<IngredientsObject>(4);
        private int included;
        private GameObject CurrentObject;

        [Header("Debug")]
        [SerializeField] IngredientsObject DebugIngredient;

        private void Start()
        {
            CurrentObject = (GameObject)Instantiate(init.Model, gameObject.transform.position, Quaternion.identity);
            CurrentObject.transform.SetParent(gameObject.transform, true);
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
            buf[included + 1] = ingredient;
            buf.Sort(0, included, null);
            for (int i = 0; i < recipeList.Recipe.Count; i++)
            {
                if (IsRecipe(buf, i))
                {
                    ingredientList = buf.ToList();
                    CurrentObject.SetActive(false);
                    Destroy(CurrentObject);
                    CurrentObject = (GameObject)Instantiate(recipeList.Recipe[i].Model, gameObject.transform);
                    CurrentObject.transform.SetParent(gameObject.transform, true);
                }
            }
        }

        private bool IsRecipe(List<IngredientsObject> ingredient, int index)
        {
            for (int i = 0; i < 4; i++)
            {
                if (ingredient[i].ingredientsData.id != recipeList.Recipe[index].ingredients[i].id)
                    return false;
                if (ingredient[i].IngState != recipeList.Recipe[index].ingredientsState[i])
                    return false;
            }
            return true;
        }

        #region Debug
#if UNITY_EDITOR
        [ContextMenu("Add Ingredients")]
        public void DebugAdd()
        {
            AddIngredient(DebugIngredient);
        }
#endif
        #endregion
    }
}
