using JH;
using KIMJAEWON;
using System.Collections;
using UnityEngine;

public class ChoppingTable : Table
{
    // IngredientsObject
    private IngredientsObject ingObject;

    // 다지기 진행 표시 바
    [SerializeField] BillBoard choppingBar;
    // 다지기 진행 현재 시간 표시 바
    [SerializeField] RectTransform currentTimeBar;
    // 칼
    [SerializeField] GameObject knife;

    // 다지는데에 걸리는 총 시간
    [SerializeField] float choppingTime;
    // 다지기 현재 남은 시간
    [SerializeField] float currentTime;
    // 다지기 횟수
    [SerializeField] int choppingCount;
    // 다지기 속도 텀
    [SerializeField] float choppingInterval;


    private void OnEnable()
    {
        SetBillboardPos();
        choppingBar.gameObject.SetActive(false);
        choppingTime = 8.0f;
        currentTime = choppingTime;
        choppingCount = 8;
        choppingInterval = 0.5f;
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
        // 놓여진 아이템이 없을 경우 X
        if (placedItem == null)
            return;

        // 놓인 아이템의 Type이 Ingredient가 아닌 경우 X
        if (placedItem.Type != ItemType.Ingredient)
            return;

        // 놓인 아이템의 Type이 Ingredient인 경우 && 아이템의 State가 Original인 경우
        ingObject = (IngredientsObject)placedItem;
        if (ingObject.IngState != 0)
            return;

        Debug.Log("실행");
        choppingBar.gameObject.SetActive(true);
        // 애니메이션 실행
        chopping = StartCoroutine(ChoppingRoutine());
    }

    IEnumerator ChoppingRoutine()
    {
        float subtractTime = choppingTime / choppingCount;
        float subtractSize = currentTimeBar.sizeDelta.x / choppingCount;

        while (currentTime > 0.1f)
        {
            currentTime -= subtractTime;
            //currentTimeBar.sizeDelta = new Vector3(currentTimeBar.sizeDelta.x - subtractSize, currentTimeBar.sizeDelta.y);
            yield return new WaitForSeconds(choppingInterval);
        }

        StopCoroutine(chopping);
        // 애니메이션 중지
        choppingBar.gameObject.SetActive(false);
        Debug.Log("중지");

        Debug.Log("코루틴 끝");

    }
}
