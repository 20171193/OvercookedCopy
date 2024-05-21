using UnityEngine;

namespace JH
{
    public class Table_Temp : MonoBehaviour
    {
        private Material originMT;
        [SerializeField]
        private MeshRenderer meshRenderer;

        [SerializeField]
        private Material changeMT;

        [SerializeField]
        GameObject GeneratePoint;

        // 테이블 위에 있는 아이템
        private Item ownItem = null;

        private void Awake()
        {
            originMT = meshRenderer.sharedMaterial;
        }

        public void EnterPlayer()
        {
            meshRenderer.sharedMaterial = changeMT;
        }

        public void ExitPlayer()
        {
            meshRenderer.sharedMaterial = originMT;
        }

        public virtual void GetItem(Item item)
        {

        }

        public virtual void Interactable(GameObject GenPoint, Item item = null)
        {
            // 테이블에 아이템이 없는경우
            if (ownItem = null)
            {
                // 둘다 없는경우
                if (item = null)
                    return;

                item.GoTo(this.GeneratePoint);
                item = ownItem;
            }
            // 테이블에는 아이템이 있는데 플레이어가 아이템이 없는경우
            if (item == null)
            {
                ownItem.GoTo(GenPoint);
                ownItem = null;
            }
            // 둘다 아이템이 있는경우
            else
            {
                switch (ownItem.Type)
                {
                    case ItemType.Plate:
                        Plate tempPlate = ownItem as Plate;
                        switch (item.Type)
                        {
                            case ItemType.Ingredient:
                                IngredientsObject temp_PI_Ingredient = item as IngredientsObject;
                                tempPlate.IngredientIN(GeneratePoint, temp_PI_Ingredient);
                                Destroy(item);
                                return;
                            case ItemType.FoodDish:
                                FoodDish temp_PF_FoodDish = item as FoodDish;
                                tempPlate.IngredientIN(GeneratePoint, temp_PF_FoodDish);
                                Destroy(item);
                                return;
                        }
                        return;
                    case ItemType.Ingredient:
                        IngredientsObject tempIngredient = ownItem as IngredientsObject;
                        switch (item.Type)
                        {
                            case ItemType.Plate:
                                Plate temp_IP_Plate = item as Plate;
                                temp_IP_Plate.IngredientIN(GeneratePoint, tempIngredient);
                                Destroy(ownItem);
                                ownItem = item;
                                return;

                            case ItemType.FoodDish:
                                FoodDish temp_IF_Plate = item as FoodDish;
                                temp_IF_Plate.Add(tempIngredient);
                                temp_IF_Plate.GoTo(GeneratePoint);
                                Destroy(ownItem);
                                ownItem = item;
                                return;
                        }
                        return;
                    case ItemType.FoodDish:
                        FoodDish tempFoodDish = ownItem as FoodDish;
                        switch (item.Type)
                        {
                            case ItemType.FoodDish:
                                tempFoodDish.AddPlate();
                                Destroy(item);
                                return;
                            case ItemType.Ingredient:
                                IngredientsObject temp_FI_Ingredient = item as IngredientsObject;
                                tempFoodDish.Add(temp_FI_Ingredient);
                                Destroy(item);
                                return;
                        }
                        return;
                    case ItemType.Pan:
                        Pan tempPan = ownItem as Pan;
                        switch (item.Type)
                        {
                            case ItemType.Ingredient:
                                IngredientsObject temp_PanI_Ingredient = item as IngredientsObject;
                                if (tempPan.isEmpty())
                                {
                                    tempPan.IngredientIN(temp_PanI_Ingredient);
                                    return;
                                }
                                return;
                            case ItemType.FoodDish:
                                FoodDish temp_PabF_FoodDish = item as FoodDish;
                                if (tempPan.isWellDone()) 
                                {
                                    if(temp_PabF_FoodDish.Add(tempPan.CookingObject))
                                        tempPan.TakeOut();
                                }  
                                Destroy(item);
                                return;
                            // case ItemType.Plate:
                        }
                        return;
                    case ItemType.Pot:
                        return;
                }
            }
        }
    }
}