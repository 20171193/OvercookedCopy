using JH;
using Kyungmin;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using Photon.Pun;
using Jc;
public class RecipeOrder : MonoBehaviourPunCallbacks
{
    [Header("Init")]
    public RecipeList recipeList;
    public InGameFlow inGameFlow;

    [Header("Prefabs")]
    [SerializeField] List<GameObject> orderUI;
    [SerializeField] List<GameObject> IngredientUI;
    [SerializeField] List<Sprite> CookStateIcon;
    [SerializeField] GameObject Icon;

    [Header("Debug")]
    [SerializeField] RecipeData recipeDataDebug;

    [SerializeField]
    private GameManager gameManager;

    private List<GameObject> OrderList = new List<GameObject>();

    private GameObject OrderUI;
    private GameObject InstantIngUI;
    private GameObject IconUI;

    private void Start()
    {

    }

    public void RecipeSpawnRoutine()
    {
        // 20초에 한번씩 랜덤 호출
        if (PhotonNetwork.IsMasterClient)
        {
            RandomRecipe();
            InvokeRepeating("RandomRecipe", 20.0f, 20.0f);
        }
    }

    private void RandomRecipe()         // 랜덤 생성
    {
        // 레시피가 비어있는지 확인
        if (recipeList.Recipe == null || recipeList.Recipe.Count == 0 || OrderList.Count >= 4)
        {
            Debug.Log("레시피가 비어있음");
            return;
        }

        // 인덱스의 0번째에서 finishedRecip의 마지막 인덱스 [n]번째 사이에서 랜덤
        int randomIndex = UnityEngine.Random.Range(0, recipeList.finishedRecipe.Count-1);

        // 레시피 생성 요청
        photonView.RPC("OrderIn", RpcTarget.All, randomIndex);
        Debug.Log($"{randomIndex}번째 생성");
    }

    [PunRPC]
    private void OrderIn(int randomIndex)
    {
        RecipeData randomRecipe = recipeList.finishedRecipe[randomIndex];

        // Recipe_IGD 생성
        int num = -1;
        for (int i = 0; i < 4; i++)
        {
            if (randomRecipe.ingredients[i] == null)
            {
                if (num == -1)
                    return;
                OrderUI = Instantiate(orderUI[i - 1], gameObject.transform);
                break;
            }
            else if (i == 3)
                OrderUI = Instantiate(orderUI[3], gameObject.transform);
            num++;
        }
        OrderUI.GetComponent<Recipe_IGD>().recipe = randomRecipe;
        OrderList.Add(OrderUI);

        // 완성음식 icon 추가
        OrderUI.GetComponent<Recipe_IGD>().finishedImage.GetComponent<Image>().sprite = randomRecipe.FoodSprite;

        // Recipe_IGD 내부 파란종이 생성
        for (int i = 0; i < num + 1; i++)       //num = 재료갯수
        {
            // 파란종이 내부 재료그림 추가
            switch (randomRecipe.ingredientsState[i])
            {
                case IngredientState.Original:
                case IngredientState.Sliced:
                    // 재료 상태에 따라 UI 프리팹을 Instantiate하여 생성함
                    InstantIngUI = Instantiate(IngredientUI[0], OrderUI.GetComponent<Recipe_IGD>().IngredientGroup.transform);
                    // 아이콘을 생성, 재료의 스프라이트를 설정
                    IconUI = Instantiate(Icon, InstantIngUI.transform);
                    IconUI.GetComponent<Image>().sprite = randomRecipe.ingredients[i].ingSprite;
                    break;
                case IngredientState.Paned:
                case IngredientState.Potted:
                    // 재료 상태에 따라 UI 프리팹을 Instantiate하여 생성함
                    InstantIngUI = Instantiate(IngredientUI[1], OrderUI.GetComponent<Recipe_IGD>().IngredientGroup.transform);
                    // 아이콘을 생성, 재료의 스프라이트를 설정
                    IconUI = Instantiate(Icon, InstantIngUI.transform);
                    IconUI.GetComponent<Image>().sprite = randomRecipe.ingredients[i].ingSprite;
                    IconUI = Instantiate(Icon, InstantIngUI.transform);
                    switch (randomRecipe.ingredientsState[i])
                    {
                        case IngredientState.Paned:
                            // 상태가 'Paned'인 경우 아이콘 스프라이트를 설정
                            IconUI.GetComponent<Image>().sprite = CookStateIcon[0];
                            break;
                        case IngredientState.Potted:
                            // 상태가 'Potted'인 경우 아이콘 스프라이트를 설정
                            IconUI.GetComponent<Image>().sprite = CookStateIcon[1];
                            break;
                    }
                    break;
            }
        }
    }

    public void OnOrderOut(Recipe_IGD igd)
    {
        Destroy(igd.gameObject);
        OrderList.Remove(igd.gameObject);

        // 레시피에 따른 스코어로 수정해야함
        inGameFlow.RecipeResult(10);
    }

    #region
#if UNITY_EDITOR
    //[ContextMenu("[Debug]Add Order")]
    //public void DebugAddOrder()
    //{
    //    if (OrderList.Count < 4)
    //        OrderIn(recipeDataDebug);
    //}

    //[ContextMenu("[Debug]Delete Order")]
    //public void DebugDeleteOrder()
    //{
    //    if (OrderList.Count > 0)
    //    {
    //        Destroy(OrderList[0]);
    //        OrderList.RemoveAt(0);
    //    }
    //}


#endif
    #endregion
}
