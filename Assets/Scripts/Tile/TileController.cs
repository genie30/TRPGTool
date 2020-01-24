using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileController : MonoBehaviour
{
    Vector3 pos, wpos;
    [SerializeField]
    Tilemap tilemap;

    [SerializeField]
    GameObject mark;

    int minx = -13, maxx = 11, miny = -11, maxy = 10;

    private void Update()
    {
        pos = Input.mousePosition;
        pos.z = 10f;
        wpos = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(pos));
        if(wpos.x >= minx && wpos.x <= maxx && wpos.y >= miny && wpos.y <= maxy)
        {
            mark.transform.position = wpos;
        }
    }

    private void ClickPos(Vector3 pos)
    {
    }
}
