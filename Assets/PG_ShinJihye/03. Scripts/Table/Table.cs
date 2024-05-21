using JH;
using JiHye;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;

public class TrashCan : Table
{

}

public class Table : MonoBehaviour, IHighlightable
{
    public enum TableState
    {
        Empty,              // 빈 테이블 (ONLY put down)
        Contain,            // 원재료, 헌 접시, 프라이팬, 냄비, 소화기 (ONLY pick up)
        Plate,              // 빈 접시, 재료 담긴 접시
        CookedIngredient,   // 조리된 재료가 있는 테이블
        burned,             // 불 붙은 테이블
    }

    private Material originMT;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Material changeMT;

    // 테이블 현재 상태
    private TableState currentState;

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

    public virtual void Interactable(Item item = null)
    {

    }
}
