using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileController : MonoBehaviour
{
    Vector3 pos, wpos;
    [SerializeField]
    Tilemap tilemap;

    private void Update()
    {
        pos = Input.mousePosition;
        pos.z = 10f;
        wpos = Camera.main.ScreenToWorldPoint(pos);
        ClickPos(wpos);
    }

    private void ClickPos(Vector3 pos)
    {
        Vector3Int ipos = tilemap.WorldToCell(pos);
        Debug.Log(ipos);
    }
}
