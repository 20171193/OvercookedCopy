using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JH
{
    public class FoodDish : MonoBehaviour, IPickable
    {
        [Header("Map Recipes")]
        [SerializeField] public RecipeList recipeList;
        public IngredientsObject init;
        public RecipeData curRecipe;
        public bool initPlate;

        public bool Plate { get; private set; }
        private List<IngredientsObject> ingredientList = new List<IngredientsObject>(4);
        private List<IngredientsObject> ingredientListDebug;
        private int included;

        private GameObject curPlate;
        private GameObject CurrentObject;

        [Header("Debug")]
        [SerializeField] IngredientsObject DebugIngredient;

        private void Start()
        {
            if (initPlate)
                Plate = true;
            for (int i = 0; i < 4; i++) ingredientList.Add(null);
            ingredientList[0] = init;
            included = 1;
            if (Plate)
            {
                curPlate = Instantiate(recipeList.PlatePrefab, gameObject.transform.position, Quaternion.identity);
                curPlate.transform.SetParent(gameObject.transform, true);
                CurrentObject = (GameObject)Instantiate(curRecipe.Model, gameObject.transform.position + new Vector3(0, curRecipe.platingInterval, 0), Quaternion.identity);
                CurrentObject.transform.SetParent(curPlate.transform, true);
            }
            else
            {
                CurrentObject = (GameObject)Instantiate(curRecipe.Model, gameObject.transform.position, Quaternion.identity);
                CurrentObject.transform.SetParent(gameObject.transform, true);
            }
        }

        /// <summary>그릇이 없을경우 그릇을 추가해주는 함수.</summary>
        public bool AddPlate()
        {
            if (Plate)
                return false;
            Plate = true;
            curPlate = Instantiate(recipeList.PlatePrefab, gameObject.transform.position, Quaternion.identity);
            curPlate.transform.SetParent(gameObject.transform, true);
            CurrentObject.transform.SetParent(curPlate.transform, true);
            return true;
        }

        /// <summary>음식이 추가재료를 감당가능한지 여부 확인 함수. 음식 조합 가능한 최대치는 4개 입니다.</summary>
        public bool IsAcceptable(int num)
        {
            if (included + num <= 4)
                return true;
            return false;
        }

        /// <summary>재료를 음식에 추가하는 함수</summary>
        /// /// <param name="ingredient">ingredient는 IngredientsObject속성을 가리키며, 추가할 재료에 대한 인수입니다.</param>
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
                    curRecipe = recipeList.Recipe[i];
                    if (Plate)
                    {
                        CurrentObject = (GameObject)Instantiate(curRecipe.Model, gameObject.transform.position + new Vector3(0, curRecipe.platingInterval, 0), Quaternion.identity);
                        CurrentObject.transform.SetParent(curPlate.transform, true);
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

        // 레시피 리스트 내에 있는 레시피인지 확인
        private bool IsRecipe(List<IngredientsObject> ingredient, int index)
        {
            for (int i = 0; i < 4; i++)
            {
                Debug.Log(i);
                // 그릇이 필수인 레시피
                if (recipeList.Recipe[index].needPlate)
                    if (!Plate)
                        return false;
                // 들고있는 재료갯수가 적을때
                if (ingredient[i] == null)
                    if (recipeList.Recipe[index].ingredients[i] != null)
                        return false;
                //들고있는 재료갯수가 많을때
                if (ingredient[i] != null)
                    if (recipeList.Recipe[index].ingredients[i] == null)
                        return false;
                // 들고있는 재료갯수가 같을때 서로 null값일경우 스킵
                if (ingredient[i] == null && recipeList.Recipe[index].ingredients[i] == null)
                    continue;
                // 들고있는 재료와 상태가 같은지 확인
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
        [ContextMenu("[Debug]Add Ingredients")]
        public void DebugAdd()
        {
            if (IsAcceptable(1) && DebugIngredient != null)
                AddIngredient(DebugIngredient);
        }

        [ContextMenu("[Debug]Add Plate")]
        public void DebugOnPlate()
        {
            AddPlate();
        }
#endif
        #endregion
    }
}
