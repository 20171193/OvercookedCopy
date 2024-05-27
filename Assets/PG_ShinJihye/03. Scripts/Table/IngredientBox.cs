using JH;
using UnityEngine;

public class IngredientBox : Table
{
    // IngredientCreate
    public IngredientCreate ingCreate;

    private void Start()
    {
        tableType = TableType.IngredientBox;
    }

    public override Item PickUpItem()
    {
        return null;
    }

    public override bool PutDownItem(Item item)
    {
        return false;
    }

    public void Interactable(GameObject itemSocket)
    {
        ingCreate.TakeIngredient(itemSocket);   
    }
}
