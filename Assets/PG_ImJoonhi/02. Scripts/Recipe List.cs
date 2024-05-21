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

        /// <summary>래시피 리스트의 index번째에 있는 래시피와 같은지 확인하는 함수.</summary>
        /// <param name="ingredient">ingredient는 IngredientsObject형을 가짅 List를 가리키며, 검사할 재료 조합 List 인수입니다.</param>
        /// <param name="index">index는 레시피 리스트의 몇번쨰 래시피와 비교할지 정하는 인수입니다.</param>
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

        /// <summary>래시피 리스트의 index번째에 있는 래시피가 Plate가 필요한 레시피인지 확인하는 함수.</summary>
        /// <param name="Plate">대조할 음식의 Plate 보유여부에 대한 인수입니다.</param>
        /// <param name="index">index는 레시피 리스트의 몇번쨰 래시피와 비교할지 정하는 인수입니다.</param>
        public bool PlateState(bool Plate, int index)
        {
            if (Recipe[index].needPlate)
                if (!Plate)
                    return false;
            return true;
        }
    }

}