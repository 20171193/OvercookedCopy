using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public List<Sprite> tileList = new List<Sprite>();  // 배치할 타일 목록
    public GameObject tilePrefab;  // 타일을 생성하기 위한 스탠실. 스프라이트를 할당하여 개인화 됨
    public int gridDimension;  // 그리드의 크기
    public float gridDistance;  // 타일 간 거리

    private GameObject[,] tileGrid;  // 2차원 배열의 타일 그리드

    private void Start()
    {
        tileGrid = new GameObject[gridDimension, gridDimension];  // (그리드의 크기 X 그리드의 크기)인 타일 그리드 생성
    }

    /* 그리드 초기화 */

    private void InitGrid()
    {

    }
}
