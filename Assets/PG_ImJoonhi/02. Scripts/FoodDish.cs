using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JH
{
    public class FoodDish : Item, IPickable
    {
        [Header("Map Recipes")]
        [SerializeField] RecipeList recipeList;
        [SerializeField] IngredientList ingredientPrefabs;
        public List<IngredientsObject> init;    // 초기 재료 리스트
        public RecipeData curRecipe;            // 현제 래시피 (어느 래시피인지 저장)
        public bool initPlate;                  // 생성시 Plate 보유여부

        /*
        [Header("Components")]
        public Rigidbody rigid;
        */

        public bool Plate { get; private set; }                                             // 그릇 여부
        private List<IngredientsObject> ingredientList = new List<IngredientsObject>(4);    // 현제 포함된 재료 리스트
        private List<IngredientsObject> ingredientListDebug;                                // 현제 체크중인 재료LIst 디버그모드 확인용 리스트
        private int included = 0;                                                               // 현제 재료가 몇개 들어있는지

        private GameObject curPlate;                                                        // 그릇 게임오브젝트 (그릇 디자인)
        private GameObject CurrentObject;                                                   // 현제 음식 게임오브젝트

        

        private void Start()
        {
            recipeList = Manager_TEMP.recipemanager.recipeList;
            ingredientPrefabs = Manager_TEMP.recipemanager.ingredientList;
            
            rigid = gameObject.GetComponent<Rigidbody>();
            collid = gameObject.GetComponent<BoxCollider>();
            rigid.isKinematic = true;
            collid.enabled = false;

            if (initPlate)
                Plate = true;
            ingredientList = init.ToList();
            for (int i = 0; i < 4; i++) if (ingredientList[i] != null) included++;
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
            meshRenderer = transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
            SetOriginMT();
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
            meshRenderer = transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
            SetOriginMT();
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
        /// <param name="ingredient">ingredient는 IngredientsObject속성을 가리키며, 추가할 재료에 대한 인수입니다.</param>
        public bool Add(IngredientsObject ingredient)
        {
            if (!IsAcceptable(1))
                return false;
            List<IngredientsObject> buf = ingredientList.ToList();
            buf[included] = ingredientPrefabs.Find(ingredient);
            buf.Sort(0, included + 1, null);

            // [Debug] 현제 비교할 재료리스트 확인용
            ingredientListDebug = buf.ToList();

            Debug.Log(recipeList.Recipe.Count);
            // 레시피를 돌면서 들어온 재료로 인한 레시피가 있는지 확인 밑 추가;
            for (int i = 0; i < recipeList.Recipe.Count; i++)
            {
                Debug.Log(i);
                Debug.Log(recipeList.Recipe[i].name);
                if (recipeList.IsRecipe(buf, i) && recipeList.PlateState(Plate,i))
                {
                    // 레시피 발견시 현제 음식모델 삭제후 새 래시피 음식모델을 재생성
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
                    return true;
                }
            }
            return false;
        }

        /// <summary>음식에 있는 재들을 음식에 옮기는 함수</summary>
        /// <param name="foodAdd">foodAdd는 IngredientsObject List속성을 가리키며, 추가할 음식 리스트 대한 인수입니다.</param>
        public bool Add(List<IngredientsObject> foodAdd)
        {
            int ingNum = 0;
            for (int i = 0; i < 4; i++) if (foodAdd[i] != null) ingNum++;
            if (!IsAcceptable(ingNum))
                return false;
            List<IngredientsObject> buf = ingredientList.ToList();
            for (int i = 0; i < ingNum; i++) buf[included + i] = foodAdd[i];
            buf.Sort(0, included + 1, null);

            // [Debug] 현제 비교할 재료리스트 확인용
            ingredientListDebug = buf.ToList();

            // 레시피를 돌면서 들어온 재료로 인한 레시피가 있는지 확인 밑 추가;
            for (int i = 0; i < recipeList.Recipe.Count; i++)
            {
                Debug.Log(recipeList.Recipe[i].name);
                if (recipeList.IsRecipe(buf, i) && recipeList.PlateState(Plate, i))
                {
                    // 레시피 발견시 현제 음식모델 삭제후 새 래시피 음식모델을 재생성
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
                    return true;
                }
            }
            return false;
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
            rigid.isKinematic = false;
            gameObject.transform.SetParent(null);
        }
        */

        #region Debug
#if UNITY_EDITOR
        [Header("Debug")]
        [SerializeField] IngredientsObject DebugIngredient;
        [SerializeField] GameObject DebugGameObject;

        [ContextMenu("[Debug]Add Ingredients")]
        public void DebugAdd()
        {
            if (DebugIngredient != null)
                Add(DebugIngredient);
        }

        [ContextMenu("[Debug]Add Plate")]
        public void DebugOnPlate()
        {
            AddPlate();
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
