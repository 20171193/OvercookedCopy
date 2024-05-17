using System;
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
            Debug.Log("Recipe Ready");
            if (Manager_TEMP.Inst != null)
                Manager_TEMP.recipemanager.recipeList = this;
            // Manager.recipemanager.recipeList = this;
        }

        public bool IsRecipe(List<IngredientsObject> ingredient, int index )
        {
            for (int i = 0; i < 4; i++)
            {
                Debug.Log(i);
                // 들고있는 재료갯수가 적을때
                if (ingredient[i] == null)
                    if (Recipe[index].ingredients[i] != null)
                        return false;
                //들고있는 재료갯수가 많을때
                if (ingredient[i] != null)
                    if (Recipe[index].ingredients[i] == null)
                        return false;
                // 들고있는 재료갯수가 같을때 서로 null값일경우 스킵
                if (ingredient[i] == null && Recipe[index].ingredients[i] == null)
                    continue;
                // 들고있는 재료와 상태가 같은지 확인
                if (Recipe[index].ingredients[i] != null)
                {
                    if (ingredient[i].ingredientsData.id != Recipe[index].ingredients[i].id)
                        return false;
                    if (ingredient[i].IngState != Recipe[index].ingredientsState[i])
                        return false;
                }
            }
            return true;
        }

        public bool PlateState(bool Plate, int index)
        {
            if (Recipe[index].needPlate)
                if (!Plate)
                    return false;
            return true;
        }
    }

}