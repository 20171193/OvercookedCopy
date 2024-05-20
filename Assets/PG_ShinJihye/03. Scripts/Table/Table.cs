using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Table: MonoBehaviour
{
    public enum TableState
    {
        Empty,              // 빈 테이블 (ONLY put down)
        Contain,            // 원재료, 헌 접시, 프라이팬, 냄비, 소화기 (ONLY pick up)
        Plate,              // 빈 접시, 재료 담긴 접시
        FilledPlate,        // 재료 담긴 접시가 있는 테이블
        CookedIngredient,   // 조리된 재료가 있는 테이블
    }

    [SerializeField]
    private Material originMT;
    [SerializeField]
    private MeshRenderer meshRenderer;

    [SerializeField]
    private Material changeMT;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        meshRenderer = child.GetComponent<MeshRenderer>();
        originMT = meshRenderer.sharedMaterial;
    }

    public void EnterPlayer()
    {
        meshRenderer.material = changeMT;
    }

    public void ExitPlayer()
    {
        meshRenderer.material = originMT;
    }

}
