using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CheckTile : MonoBehaviour
{
    public Tilemap tilemap;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //tilemap.SwapTile(tilemap.GetTile(new Vector3Int(0, 0, 0)), tiles);
        }
    }





}
