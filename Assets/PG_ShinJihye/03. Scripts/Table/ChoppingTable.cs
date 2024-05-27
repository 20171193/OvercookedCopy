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
    //[SerializeField] GameObject slicedItemPrefab;

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


    public override bool IsInteractable(Item item = null)
    {
        if (item != null)
            return false;

        if (placedItem == null && placedItem.Type != ItemType.Ingredient && ingObject.IngState != 0)
            return false;

        return true;
    }

    public void Interactable()
    {
        // 다지기 상호작용 실행
        Debug.Log("시작");

        choppingBar.gameObject.SetActive(true);  // UI 바 나타남

        // Player 애니메이션 실행

        chopping = StartCoroutine(ChoppingRoutine(placedItem));  // 코루틴 실행

        Debug.Log("완료");
    }


    Coroutine chopping;

    IEnumerator ChoppingRoutine(Item placedItem)
    {
        currentTimeBarSizeX = currentTimeBar.sizeDelta.x;  // 시간 표시 바 가로 길이
        float subtractTime = choppingTime / choppingCount;  // 1번 다질 때 줄어드는 시간
        float subtractSize = currentTimeBarSizeX / choppingCount;  // 1번 다질 때 줄어드는 UI 바

        Debug.Log("코루틴 실행");

        yield return new WaitForSeconds(choppingInterval);

        // 다지는 동안 시간, UI 바 줄어듦
        while (choppingTime > 0.1f && currentTimeBarSizeX > 0)
        {
            choppingTime -= subtractTime;
            currentTimeBarSizeX -= subtractSize;
            currentTimeBar.sizeDelta = new Vector2(currentTimeBarSizeX, currentTimeBar.sizeDelta.y);
            yield return new WaitForSeconds(choppingInterval);
            Debug.Log("while");
        }

        // -- 다지기 끝

        // Player 애니메이션 중지

        choppingBar.gameObject.SetActive(false);  // UI 바 사라짐

        ingObject.Slice();  // 재료 다지면 Sliced 프리팹으로 바뀜

        // Original 아이템 삭제
        /*PhotonNetwork.Destroy(placedItem.gameObject);
        Debug.Log("삭제함");*/

        // Sliced 아이템 생성

        /*slicedItemPrefab = PhotonNetwork.Instantiate("Lettuce_sliced", generatePoint.transform.position, generatePoint.transform.rotation);
        slicedItemPrefab.transform.SetParent(generatePoint.transform, true);
        Item newPlacedItem = slicedItemPrefab.GetComponent<Item>();
        Debug.Log(newPlacedItem);
        this.placedItem = newPlacedItem;
        Debug.Log("생성함");*/

        InitChoppingValue();

        Debug.Log("코루틴 중지");
    }
}
