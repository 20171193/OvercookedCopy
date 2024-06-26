using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ChangeableTile : MonoBehaviour
{
    // 초기 머터리얼
    private Material originMT;
    [SerializeField]
    private MeshRenderer meshRenderer;

    // 변경될 머터리얼 
    [SerializeField]
    private Material changeMT;

    // 타일  회전속도
    [SerializeField]
    private float rotSpeed;

    // 초기 위치
    private Vector3 originPos;

    // 변경될 위치
    private Vector3 targetPos;

    public UnityAction OnReversing;
    public UnityAction OnFloating;

    private Coroutine tileChangeRoutine;
    private Coroutine tileFloatRoutine;

    public UnityAction OnChangedTile;

    private void Awake()
    {
        originMT = meshRenderer.sharedMaterial;
        originPos = transform.position;
        targetPos = transform.position + Vector3.up;
    }

    // 이미 오픈된 스테이지일 경우
    public void OnChangedSetting()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        meshRenderer.sharedMaterial = changeMT;
    }

    public void OnChangeTile()
    {
        tileChangeRoutine = StartCoroutine(TileChangeRoutine());
    }

    public void ResetTile()
    {
        transform.rotation = Quaternion.identity;
        meshRenderer.material = originMT;
    }

    // 타일 뒤집기, 머터리얼 변경 루틴
    IEnumerator TileChangeRoutine()
    {
        OnReversing?.Invoke();
        float time = 180 / rotSpeed;
        float rate = 0;

        bool tileChange = false;

        Quaternion target = Quaternion.Euler(new Vector3(0, 0, 180));

        while (rate < 1)
        {
            rate += Time.deltaTime / time;
            // 타일 회전
            transform.rotation = Quaternion.Lerp(Quaternion.identity, target, rate);
            
            // 타일 이동
            if(rate < 0.5f)
                transform.position = Vector3.Lerp(originPos, targetPos, rate * 2f);

            if (!tileChange && rate >= 0.5)
            {
                tileChange = true;
                meshRenderer.sharedMaterial = changeMT;
                transform.position = targetPos;
                tileFloatRoutine = StartCoroutine(TileFloatingRoutine());
            }

            yield return null;
        }

        transform.rotation = target;
        OnChangedTile?.Invoke();
        yield return null;
    }

    IEnumerator TileFloatingRoutine()
    {
        OnFloating?.Invoke();
        float rate = 0f;
        while(rate < 1)
        {
            rate += Time.deltaTime;

            transform.position = Vector3.Lerp(targetPos, originPos, rate);
            yield return null;
        }
        transform.position = originPos;
        yield return null;
    }
}
