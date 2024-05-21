using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LayerManager : Singleton<LayerManager>
{
    [Header("테이블 + 아이템")]
    public LayerMask interactableLM;

    [Header("테이블")]
    public LayerMask tableLM;

    [Header("아이템")]
    public LayerMask itemLM;

    [Header("떨어진 아이템")]
    public LayerMask dropItemLM;

    [Header("물 타일")]
    [SerializeField]
    public LayerMask waterTileLM;
}
