using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ChangeableTile : MonoBehaviour
{
    private Material originMT;
    [SerializeField]
    private MeshRenderer meshRenderer;

    [SerializeField]
    private Material changeMT;

    [SerializeField]
    private float rotSpeed;

    private Coroutine tileChangeRoutine;
    private Coroutine tileFloatRoutine;

    public UnityAction OnChangedTile;

    private void Awake()
    {
        originMT = meshRenderer.sharedMaterial;
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

    IEnumerator TileChangeRoutine()
    {
        float time = 180 / rotSpeed;
        float rate = 0;

        bool tileChange = false;
        Vector3 originPos = transform.position;
        Vector3 targetPos = transform.position + Vector3.up;

        Quaternion target = Quaternion.Euler(new Vector3(0, 0, 180));

        while (rate < 1)
        {
            rate += Time.deltaTime / time;
            // 타일 회전
            transform.rotation = Quaternion.Lerp(Quaternion.identity, target, rate);
            
            // 타일 이동
            if(!tileChange)
                transform.position = Vector3.Lerp(originPos, targetPos, rate * 2f);
            else
                transform.position = Vector3.Lerp(targetPos, originPos, (rate - 0.5f)*2f);

            if (!tileChange && rate >= 0.5)
            {
                tileChange = true;
                meshRenderer.sharedMaterial = changeMT;
                transform.position = targetPos;
            }
            yield return null;
        }

        transform.rotation = target;
        transform.position = originPos;
        OnChangedTile?.Invoke();
        yield return null;
    }
}
