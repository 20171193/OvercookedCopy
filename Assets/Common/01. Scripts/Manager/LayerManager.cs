using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LayerManager : Singleton<LayerManager>
{
    [Header("테이블 + 아이템")]
    [SerializeField]
    public LayerMask interactableLM;

    [Header("테이블")]
    [SerializeField]
    public LayerMask tableLM;

    [Header("물 타일")]
    [SerializeField]
    public LayerMask waterTileLM;
}
