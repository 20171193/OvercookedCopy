using JH;
using Photon.Pun;
using UnityEngine;

public class Table : MonoBehaviour, IHighlightable
{
    private Material originMT;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Material changeMT;

    // 테이블 위에 있는 아이템
    public Item placedItem;

    // 테이블에 아이템이 놓일 위치
    [SerializeField] GameObject generatePoint;

    // generatePoint 인덱스 찾기 위한 임시 변수
    [SerializeField] int childIndex;

    public int ChildIndex { get; set; }

    private void Awake()
    {
        originMT = meshRenderer.sharedMaterial;

        // generatePoint 있는지 null 체크 (에러 방지)
        Transform temp = transform.GetChild(childIndex);
        if (temp != null)
        {
            generatePoint = temp.gameObject;
        }

        // 게임 시작 시 테이블에 아이템이 미리 놓여져 있는 경우 해당 아이템 placedItem에 할당
        placedItem = transform.GetComponentInChildren<Item>();
        int placedItemIndex = transform.childCount;
        if (placedItem != null && generatePoint != null)
        {
            placedItem = transform.GetChild(placedItemIndex - 1).GetComponent<Item>();
            placedItem.transform.position = generatePoint.gameObject.transform.position;
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
    public virtual bool PutDownItem(Item item)
    {
        Debug.Log("table.PutDownItem");

        Item tempItem = null;

        // 1. 테이블에 아이템 없음
        if (placedItem == null)
        {
            // 플레이어에게 아이템 없음
            if (item == null)
                return false;

            // 플레이어에게 아이템 있음 (아이템을 테이블로)
            item.GoTo(generatePoint);
            placedItem = item;
            return true;
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
                            tempItem = tempPlate.IngredientIN(generatePoint, temp_PI_Ingredient);
                            if (tempItem != null)
                            {
                                // placedItem = tempItem;
                                gameObject.GetPhotonView().RPC("ChangePlacedItem", RpcTarget.All, tempItem.photonView.ViewID);
                                item.gameObject.GetPhotonView().RPC("DestroyItem", RpcTarget.MasterClient);
                                return true;
                            }
                            else
                            {
                                Debug.Log("Fail");
                                return false;
                            }

                        // (2) 손에 든 게 조합된 재료일 때
                        case ItemType.FoodDish:
                            FoodDish temp_PF_FoodDish = item as FoodDish;
                            if (tempPlate.IngredientIN(generatePoint, temp_PF_FoodDish))
                                item.gameObject.GetPhotonView().RPC("DestroyItem", RpcTarget.MasterClient);
                            return true;

                        // (3) 손에 든 게 프라이팬일 때
                        case ItemType.Pan:
                            Pan temp_PPan_Pan = item as Pan;
                            if (temp_PPan_Pan.isWellDone())
                            {
                                if (tempPlate.IngredientIN(generatePoint, temp_PPan_Pan.CookingObject))
                                    temp_PPan_Pan.TakeOut();
                                return true;
                            }
                            return false;
                    }
                    return false;

                // 1-2. 테이블에 재료 있을 떄
                case ItemType.Ingredient:
                    IngredientsObject tempIngredient = placedItem as IngredientsObject;
                    switch (item.Type)
                    {
                        // (1) 손에 든 게 접시일 때
                        case ItemType.Plate:
                            Plate temp_IP_Plate = item as Plate;
                            tempItem = temp_IP_Plate.IngredientIN(generatePoint, tempIngredient);
                            if (tempItem != null)
                            {
                                placedItem.gameObject.GetPhotonView().RPC("DestroyItem", RpcTarget.MasterClient);
                                placedItem = tempItem;
                                return true;
                            }
                            else
                            {
                                Debug.Log("Fail");
                                return false;
                            }

                        // (2) 손에 든 게 조합된 재료일 때
                        case ItemType.FoodDish:
                            FoodDish temp_IF_Plate = item as FoodDish;
                            if (temp_IF_Plate.Add(tempIngredient))
                            {
                                temp_IF_Plate.GoTo(generatePoint);
                                placedItem.gameObject.GetPhotonView().RPC("DestroyItem", RpcTarget.MasterClient);
                                placedItem = item;
                                return true;
                            }
                            return false;
                    }
                    return false;

                // 1-3. 테이블에 조합된 재료 있을 때
                case ItemType.FoodDish:
                    FoodDish tempFoodDish = placedItem as FoodDish;
                    switch (item.Type)
                    {
                        // (1) 손에 든 게 재료일 때
                        case ItemType.Ingredient:
                            IngredientsObject temp_FI_Ingredient = item as IngredientsObject;
                            if (tempFoodDish.Add(temp_FI_Ingredient))
                                item.gameObject.GetPhotonView().RPC("DestroyItem", RpcTarget.MasterClient);
                            return true;

                        // (2) 손에 든 게 조합된 재료일 때
                        case ItemType.FoodDish:
                            if (tempFoodDish.AddPlate())
                                item.gameObject.GetPhotonView().RPC("DestroyItem", RpcTarget.MasterClient);
                            return false;

                        // (3) 손에 든 게 프라이팬일 때
                        case ItemType.Pan:
                            Pan temp_FPan_Pan = item as Pan;
                            if (temp_FPan_Pan.isWellDone())
                            {
                                if (tempFoodDish.Add(temp_FPan_Pan.CookingObject))
                                    temp_FPan_Pan.TakeOut();
                                return true;
                            }
                            return false;
                    }
                    return false;

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
                                return true;
                            }
                            return false;

                        // (2) 손에 든 게 접시일 때
                        case ItemType.Plate:
                            Plate temp_PanP_Plate = item as Plate;
                            if (tempPan.isWellDone())
                            {
                                //if (temp_PanP_Plate.IngredientIN(GenPoint, tempPan.CookingObject))
                                tempPan.TakeOut();
                                return true;
                            }
                            return false;

                        // (3) 손에 든 게 조합된 재료일 때
                        case ItemType.FoodDish:
                            FoodDish temp_PanF_FoodDish = item as FoodDish;
                            if (tempPan.isWellDone())
                            {
                                if (temp_PanF_FoodDish.Add(tempPan.CookingObject))
                                    tempPan.TakeOut();
                                return true;
                            }
                            item.gameObject.GetPhotonView().RPC("DestroyItem", RpcTarget.MasterClient);
                            return false;
                    }
                    return false;
            }
            return false;
        }

        // 3. 쓰레기통
        // override 함 (base 없음)

    }

    // 테이블 아이템 변경 동기화
    [PunRPC]
    public void ChangePlacedItem(int ItemID)
    {
        placedItem = PhotonView.Find(ItemID).gameObject.GetComponent<Item>();
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
        // 불났으면 false

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
