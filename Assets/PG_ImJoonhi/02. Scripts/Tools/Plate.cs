using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
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

        private List<IngredientsObject> ingredientListDebug;

        private void Awake()
        {
            rigid = gameObject.GetComponent<Rigidbody>();
            collid = gameObject.GetComponent<BoxCollider>();
            rigid.isKinematic = true;
            collid.enabled = false;
        }

        private void Start()
        {
            recipeList = Manager_TEMP.recipemanager.recipeList;
            ingredientPrefabs = Manager_TEMP.recipemanager.ingredientList;
            meshRenderer = transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
            SetOriginMT();
        }

        /// <summary>그릇에 놓을 재료가 1가지인 ingredientObject 프리팹의 경우 사용하는 함수.</summary>
        public FoodDish IngredientIN(GameObject GeneratePoint, IngredientsObject ingredientObject)
        {
            List<IngredientsObject> buf = new List<IngredientsObject>();
            for (int i = 0; i < 4; i++) buf.Add(null);
            int ingredientTypeNum = 0;
            int ingredientNum = 0;
            buf[0] = ingredientPrefabs.Find(ingredientObject, ref ingredientTypeNum, ref ingredientNum);
            ingredientListDebug = buf.ToList();
            Debug.Log("?");
            for (int i = 0; i < recipeList.Recipe.Count; i++)
            {
                Debug.Log(recipeList.Recipe[i].name);
                if (recipeList.IsRecipe(buf, i))
                {
                    Debug.Log($"found recipe : {recipeList.Recipe[i].name}");
                    return GenerateFoodDish(GeneratePoint, buf, recipeList.Recipe[i], i, ingredientTypeNum, ingredientNum);
                }
            }
            return null;
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

        private FoodDish GenerateFoodDish(GameObject GeneratePoint, List<IngredientsObject> ingredientObject, RecipeData recipe, int recipeNum, int ingredientTypeNum, int ingredientNum)
        {
            /*
            FoodDish foodDish = Instantiate(foodDishPrefab, GeneratePoint.transform.position, Quaternion.identity);
            // foodDish.recipeList = Manager_TEMP.recipemanager.recipeList;
            foodDish.init = ingredientObject;
            foodDish.curRecipe = recipe;
            foodDish.initPlate = true;
            foodDish.rigid.isKinematic = rigid.isKinematic;
            foodDish.transform.SetParent(GeneratePoint.transform, true);
            Destroy(gameObject);
            return foodDish;
            */
            GameObject temp_foodDish = PhotonNetwork.Instantiate("Colaborate Food/Colaborate Food", GeneratePoint.transform.position, Quaternion.identity);
            FoodDish foodDish = temp_foodDish.GetComponent<FoodDish>();

            // 현제 클라이언트용
            foodDish.init = ingredientObject;
            foodDish.curRecipe = recipe;
            foodDish.initPlate = true;
            foodDish.rigid.isKinematic = rigid.isKinematic;

            // 현제 클라이언트 제외 클라이언트용
            Debug.Log("write");
            photonView.RPC("SetFoodDish", RpcTarget.Others, PhotonView.Get(foodDish).ViewID, recipeNum, ingredientTypeNum, ingredientNum);
            Debug.Log("foodDish Goto");

            foodDish.GoTo(GeneratePoint);
            // PhotonNetwork.Destroy(gameObject);
            gameObject.GetPhotonView().RPC("DestroyItem", RpcTarget.MasterClient);
            return foodDish;
        }

        [PunRPC]
        public void SetFoodDish(int foodDishID, int recipeNum, int ingredientTypeNum, int ingredientNum)
        {
            FoodDish foodDish = PhotonView.Find(foodDishID).gameObject.GetComponent<FoodDish>();
            /*
            foodDish.init = ingredientObject;
            foodDish.curRecipe = recipe;
            */
            List<IngredientsObject> buf = new List<IngredientsObject>();
            for (int i = 0; i < 4; i++) buf.Add(null);
            buf[0] = ingredientPrefabs.Find(ingredientTypeNum, ingredientNum);
            foodDish.init = buf;
            foodDish.curRecipe = recipeList.Recipe[recipeNum];
            foodDish.initPlate = true;
            foodDish.rigid.isKinematic = rigid.isKinematic;
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
