using JH;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeOrder : MonoBehaviour
{
    [Header("Init")]
    public RecipeList recipeList;

    [Header("Prefabs")]
    [SerializeField] List<GameObject> orderUI;
    [SerializeField] List<GameObject> IngredientUI;
    [SerializeField] List<Sprite> CookStateIcon;
    [SerializeField] GameObject Icon;

    [Header("Debug")]
    [SerializeField] RecipeData recipeDataDebug;

    private List<GameObject> OrderList = new List<GameObject>();

    private GameObject OrderUI;
    private GameObject InstantIngUI;
    private GameObject IconUI;

    private void Start()
    {

    }

    private void OrderIn(RecipeData recipe)
    { 
        // Recipe_IGD 생성
        int num = -1;
        for (int i = 0; i < 4; i++)
        {
            if (recipe.ingredients[i] == null)
            {
                if (num == -1)
                    return;
                OrderUI = Instantiate(orderUI[i-1], gameObject.transform);
                break;
            }
            else if (i == 3)
                OrderUI = Instantiate(orderUI[3], gameObject.transform);
            num++;
        }
        OrderUI.GetComponent<Recipe_IGD>().recipe = recipe;
        OrderList.Add(OrderUI);

        // 여기에 완성음식 추가

        // Recipe_IGD 내부 파란종이 생성
        for (int i = 0; i < num + 1; i++)       //num = 재료갯수
        {
            // 파란종이 내부 재료그림 추가
            switch (recipe.ingredientsState[i])
            {
                case IngredientState.Original:
                case IngredientState.Sliced:
                    // 재료 상태에 따라 UI 프리팹을 Instantiate하여 생성함
                    InstantIngUI = Instantiate(IngredientUI[0], OrderUI.GetComponent<Recipe_IGD>().IngredientGroup.transform);
                    // 아이콘을 생성, 재료의 스프라이트를 설정
                    IconUI = Instantiate(Icon, InstantIngUI.transform);
                    IconUI.GetComponent<Image>().sprite = recipe.ingredients[i].ingSprite;
                    break;
                case IngredientState.Paned:
                case IngredientState.Potted:
                    // 재료 상태에 따라 UI 프리팹을 Instantiate하여 생성함
                    InstantIngUI = Instantiate(IngredientUI[1], OrderUI.GetComponent<Recipe_IGD>().IngredientGroup.transform);
                    // 아이콘을 생성, 재료의 스프라이트를 설정
                    IconUI = Instantiate(Icon, InstantIngUI.transform);
                    IconUI.GetComponent<Image>().sprite = recipe.ingredients[i].ingSprite;
                    IconUI = Instantiate(Icon, InstantIngUI.transform);
                    switch (recipe.ingredientsState[i])
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

#region
#if UNITY_EDITOR
    [ContextMenu("[Debug]Add Order")]
    public void DebugAddOrder()
    {
        if(OrderList.Count < 4)
            OrderIn(recipeDataDebug);
    }

    [ContextMenu("[Debug]Delete Order")]
    public void DebugDeleteOrder()
    {
        if (OrderList.Count > 0)
        {
            Destroy(OrderList[0]);
            OrderList.RemoveAt(0);
        }
    }
#endif
    #endregion
}
