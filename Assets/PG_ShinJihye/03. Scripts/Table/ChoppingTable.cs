using JH;
using KIMJAEWON;
using Photon.Pun;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ChoppingTable : Table
{
    // IngredientsObject
    private IngredientsObject ingObject;
    // 칼
    [SerializeField] GameObject knife;

    // 다지기 진행 표시 바
    [SerializeField] BillBoard choppingBar;
    // 다지기 현재 시간 표시 바
    [SerializeField] RectTransform currentTimeBar;
    // 다지는데에 걸리는 총 시간
    [SerializeField] float choppingTime;
    // 다지기 총 횟수
    [SerializeField] int choppingCount;
    // 다지기 속도 텀
    [SerializeField] float choppingInterval;

    // 다진 후 바뀔 프리팹
    [SerializeField] GameObject slicedItemPrefab;

    // test
    [SerializeField] float currentTimeBarSizeX;
    

    private void OnEnable()
    {
        SetBillboardPos();
        InitChoppingValue();
        choppingBar.gameObject.SetActive(false);
    }


    // 시작 시 Chopping Time Bar UI 테이블 위쪽으로 위치
    private void SetBillboardPos()
    {
        float quaternionY = transform.eulerAngles.y;
        float dirValue = 1.0f;
        Vector3 dir = Vector3.zero;

        switch (quaternionY)
        {
            case 0:
                dir = new Vector3(0, 0, dirValue);
                break;
            case 90:
                dir = new Vector3(-dirValue, 0, 0);
                break;
            case 180:
                dir = new Vector3(0, 0, -dirValue);
                break;
            case 270:
                dir = new Vector3(dirValue, 0, 0);
                break;
        }

        choppingBar.transform.Translate(dir);
    }

    // 시작 시 다지기 관련 변수 초기화
    private void InitChoppingValue()
    {
        choppingTime = 8;
        currentTimeBar.sizeDelta = new Vector2(0.8f, 0.12f);
    }


    public override bool PutDownItem(Item item)
    {
        base.PutDownItem(item);
        knife.SetActive(false);

        return true;
    }

    public override Item PickUpItem()
    {
        Item returnItem = base.PickUpItem();
        knife.SetActive(true);
        
        return returnItem;
    }


    Coroutine chopping;

    // 상호작용 - 다지기
    public override void Interactable()
    {
        ingObject = (IngredientsObject)placedItem;

        // 놓여진 아이템 있고, 아이템의 Type이 Ingredient이고, 아이템의 State가 Original인 경우
        if (placedItem != null && placedItem.Type == ItemType.Ingredient && ingObject.IngState == 0)
        {
            Debug.Log("시작");

            choppingBar.gameObject.SetActive(true);
            // 애니메이션 실행
            chopping = StartCoroutine(ChoppingRoutine(placedItem));



            Debug.Log("완료");
        }

        return;
    }

    IEnumerator ChoppingRoutine(Item placedItem)
    {
        currentTimeBarSizeX = currentTimeBar.sizeDelta.x;
        float subtractTime = choppingTime / choppingCount;
        float subtractSize = currentTimeBarSizeX / choppingCount;

        Debug.Log("코루틴 실행");

        yield return new WaitForSeconds(choppingInterval);

        while (choppingTime > 0.1f && currentTimeBarSizeX > 0)
        {
            choppingTime -= subtractTime;
            currentTimeBarSizeX -= subtractSize;
            currentTimeBar.sizeDelta = new Vector2(currentTimeBarSizeX, currentTimeBar.sizeDelta.y);
            yield return new WaitForSeconds(choppingInterval);
            Debug.Log("while");
        }

        // 애니메이션 중지
        choppingBar.gameObject.SetActive(false);

        // Original 아이템 삭제
        PhotonNetwork.Destroy(placedItem.gameObject);
        Debug.Log("삭제함");

        // Sliced 아이템 생성
        slicedItemPrefab = PhotonNetwork.Instantiate("Lettuce_sliced", generatePoint.transform.position, generatePoint.transform.rotation);
        slicedItemPrefab.transform.SetParent(generatePoint.transform, true);
        Item newPlacedItem = slicedItemPrefab.GetComponent<Item>();
        Debug.Log(newPlacedItem);
        this.placedItem = newPlacedItem;
        Debug.Log("생성함");

        InitChoppingValue();

        Debug.Log("코루틴 중지");
    }
}
