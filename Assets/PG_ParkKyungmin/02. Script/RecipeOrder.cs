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
        int num = 0;
        for (int i = 0; i < 4; i++)
        {
            if (recipe.ingredients[i] == null)
                OrderUI = Instantiate(orderUI[i], gameObject.transform);
            else if (i == 3)
                OrderUI = Instantiate(orderUI[3], gameObject.transform);
            num++;
        }
        OrderUI.GetComponent<Recipe_IGD>().recipe = recipe;
        OrderList.Add(OrderUI);
        // Recipe_IGD 내부 파란종이 생성
        for (int i = 0; i < num + 1; i++)
        {
            // 파란종이 내부 재료그림 추가
            switch (recipe.ingredientsState[i])
            {
                case IngredientState.Original:
                case IngredientState.Sliced:
                    InstantIngUI = Instantiate(IngredientUI[0], OrderUI.GetComponent<Recipe_IGD>().IngredientGroup.transform);
                    IconUI = Instantiate(Icon, InstantIngUI.transform);
                    IconUI.GetComponent<Image>().sprite = recipe.ingredients[i].ingSprite;
                    break;
                case IngredientState.Paned:
                case IngredientState.Potted:
                    InstantIngUI = Instantiate(IngredientUI[1], OrderUI.GetComponent<Recipe_IGD>().IngredientGroup.transform);
                    IconUI = Instantiate(Icon, InstantIngUI.transform);
                    IconUI.GetComponent<Image>().sprite = recipe.ingredients[i].ingSprite;
                    IconUI = Instantiate(Icon, InstantIngUI.transform);
                    switch (recipe.ingredientsState[i])
                    {
                        case IngredientState.Paned:
                            IconUI.GetComponent<Image>().sprite = CookStateIcon[0];
                            break;
                        case IngredientState.Potted:
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
