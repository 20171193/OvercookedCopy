using JH;
using UnityEngine;

public class IngredientBox : Table
{
    // IngredientCreate
    private IngredientCreate ingCreate;

    // 꺼낸 재료 프리팹
    [SerializeField] GameObject ingObject;

    public override TableType Interactable()
    {
        //return base.Interactable();
        ingCreate.TakeIngredient(generatePoint);

        return TableType.IngredientBox;
    }
}
