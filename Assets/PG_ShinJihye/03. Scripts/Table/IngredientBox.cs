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

    public override bool IsInteractable(Item item = null)
    {
        if (item == null)
            return true;

        return false;
    }

    public override TableType Interactable()
    {
        //return base.Interactable();
        ingCreate.TakeIngredient(generatePoint);

        return TableType.IngredientBox;
    }
}
