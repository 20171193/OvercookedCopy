using JH;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Progress;

public class Table : MonoBehaviour, IHighlightable
{
    private Material originMT;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Material changeMT;

    // 테이블 위에 있는 아이템
    public Item placedItem;

    // 테이블에 아이템이 놓일 위치 (소켓)
    [SerializeField] GameObject generatePoint;


    private void Awake()
    {
        originMT = meshRenderer.sharedMaterial;
        //placedItem = GetComponent<Item>();

        if (transform.childCount >= 2 && transform.GetChild(1) != null)
        {
            placedItem = transform.GetChild(1).GetComponent<Item>();
        }
        
    }
    public void EnterPlayer()
    {
        meshRenderer.sharedMaterial = changeMT;
    }
    public void ExitPlayer()
    {
        meshRenderer.sharedMaterial = originMT;
    }


    // 테이블에 아이템 놓기 (item: 플레이어가 들고 있는 아이템)
    public virtual void PutDownItem(Item item)
    {
        Debug.Log("table.PutDownItem");

        // 1. 테이블에 아이템 없음
        if (placedItem = null)
        {
            // 플레이어에게 아이템 없음 (return)
            if (item = null)
                return;

            // 플레이어에게 아이템 있음 (아이템을 테이블로)
            item.GoTo(generatePoint);
            placedItem = item;
        }

        // 2. 테이블에 아이템 있음
        else
        {
            switch (placedItem.Type)
            {
                // 1-1. 테이블에 접시 있을 때
                case ItemType.Plate:
                    Plate tempPlate = placedItem as Plate;
                    switch (item.Type)
                    {
                        // (1) 손에 든 게 재료일 때
                        case ItemType.Ingredient:
                            IngredientsObject temp_PI_Ingredient = item as IngredientsObject;
                            if (tempPlate.IngredientIN(generatePoint, temp_PI_Ingredient))
                                Destroy(item);
                            return;

                        // (2) 손에 든 게 조합된 재료일 때
                        case ItemType.FoodDish:
                            FoodDish temp_PF_FoodDish = item as FoodDish;
                            if (tempPlate.IngredientIN(generatePoint, temp_PF_FoodDish))
                                Destroy(item);
                            return;

                        // (3) 손에 든 게 프라이팬일 때
                        case ItemType.Pan:
                            Pan temp_PPan_Pan = item as Pan;
                            if (temp_PPan_Pan.isWellDone())
                            {
                                if (tempPlate.IngredientIN(generatePoint, temp_PPan_Pan.CookingObject))
                                    temp_PPan_Pan.TakeOut();
                            }
                            return;
                    }
                    return;

                // 1-2. 테이블에 재료 있을 떄
                case ItemType.Ingredient:
                    IngredientsObject tempIngredient = placedItem as IngredientsObject;
                    switch (item.Type)
                    {
                        // (1) 손에 든 게 접시일 때
                        case ItemType.Plate:
                            Plate temp_IP_Plate = item as Plate;
                            if (temp_IP_Plate.IngredientIN(generatePoint, tempIngredient))
                            {
                                Destroy(placedItem);
                                placedItem = item;
                            }
                            return;

                        // (2) 손에 든 게 조합된 재료일 때
                        case ItemType.FoodDish:
                            FoodDish temp_IF_Plate = item as FoodDish;
                            if (temp_IF_Plate.Add(tempIngredient))
                            {
                                temp_IF_Plate.GoTo(generatePoint);
                                Destroy(placedItem);
                                placedItem = item;
                            }
                            return;
                    }
                    return;

                // 1-3. 테이블에 조합된 재료 있을 때
                case ItemType.FoodDish:
                    FoodDish tempFoodDish = placedItem as FoodDish;
                    switch (item.Type)
                    {
                        // (1) 손에 든 게 재료일 때
                        case ItemType.Ingredient:
                            IngredientsObject temp_FI_Ingredient = item as IngredientsObject;
                            if (tempFoodDish.Add(temp_FI_Ingredient))
                                Destroy(item);
                            return;

                        // (2) 손에 든 게 조합된 재료일 때
                        case ItemType.FoodDish:
                            if (tempFoodDish.AddPlate())
                                Destroy(item);
                            return;

                        // (3) 손에 든 게 프라이팬일 때
                        case ItemType.Pan:
                            Pan temp_FPan_Pan = item as Pan;
                            if (temp_FPan_Pan.isWellDone())
                            {
                                if (tempFoodDish.Add(temp_FPan_Pan.CookingObject))
                                    temp_FPan_Pan.TakeOut();
                            }
                            return;
                    }
                    return;

                // 1-4. 테이블에 프라이팬 있을 때
                case ItemType.Pan:
                    Pan tempPan = placedItem as Pan;
                    switch (item.Type)
                    {
                        // (1) 손에 든 게 재료일 때
                        case ItemType.Ingredient:
                            IngredientsObject temp_PanI_Ingredient = item as IngredientsObject;
                            if (tempPan.isEmpty())
                            {
                                tempPan.IngredientIN(temp_PanI_Ingredient);
                                return;
                            }
                            return;

                        /*
                        case ItemType.Plate:
                            Plate temp_PanP_Plate = item as Plate;
                            if (tempPan.isWellDone())
                            {
                                if (temp_PanP_Plate.IngredientIN(GenPoint, tempPan.CookingObject))
                                    tempPan.TakeOut();
                            }
                            return;
                        case ItemType.FoodDish:
                            FoodDish temp_PanF_FoodDish = item as FoodDish;
                            if (tempPan.isWellDone())
                            {
                                if (temp_PanF_FoodDish.Add(tempPan.CookingObject))
                                    tempPan.TakeOut();
                            }
                            Destroy(item);
                            return;
                        */
                    }
                    return;
            }
        }

        // 3. 쓰레기통
        // override 함 (base 없음)
    }


    // 테이블에 있는 아이템 집기
    public virtual Item PickUpItem()
    {
        Debug.Log("table.PickUpItem");

        Item returnItem = placedItem;
        placedItem = null;
        
        return returnItem;
    }

    // 테이블에 아이템 놓을 수 있는지 여부
    public virtual bool PutDownItem()
    {
        if (placedItem == null)
        {
            return true;
        }
        return false;
    }

    // 테이블 상호작용
    public virtual void Interactable()
    {
        Debug.Log("table.Interactable");

        // 1. 도마 : 다지기


    }
}
