using JH;
using UnityEngine;

public class IngredientBox : Table
{
    // IngredientCreate
    private IngredientCreate ingCreate;

    // 꺼낸 재료 프리팹
    [SerializeField] GameObject ingObject;

    private void Start()
    {
        tableType = TableType.IngredientBox;
    }

    public void Interactable(GameObject itemSocket)
    {
        ingCreate.TakeIngredient(itemSocket);
    }
}
