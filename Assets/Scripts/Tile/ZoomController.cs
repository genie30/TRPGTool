using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomController : MonoBehaviour
{
    [SerializeField]
    Camera cam;

    [SerializeField]
    float min, max, zoom, movestep;
    Vector3 startpos;

    private void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            var scr = Input.GetAxis("Mouse ScrollWheel");
            var tmp = cam.orthographicSize + scr * zoom;
            if (tmp < 3f)
            {
                cam.orthographicSize = 3f;
            }
            else if (tmp > 12)
            {
                cam.orthographicSize = 12f;
            }
            else
            {
                cam.orthographicSize = tmp;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            startpos = Input.mousePosition;
        }
        if (Input.GetMouseButton(1))
        {
            float x = (startpos.x - Input.mousePosition.x) / Screen.width;
            float y = (startpos.y - Input.mousePosition.y) / Screen.height;
            x *= movestep;
            y *= movestep;
            var movepos = cam.transform.position + new Vector3(x, y, 0);
            cam.transform.position = movepos;
        }
    }
}
