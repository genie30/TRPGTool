using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class CharacterController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    Tilemap tilemap;
    bool drag;
    int minx = -13, maxx = 11, miny = -11, maxy = 10;

    bool isDubleClick;
    float doubleClickTime = 0.2f;

    Vector3 pos;

    private void Start()
    {
        tilemap = FindObjectOfType<Tilemap>();
    }

    private void Update()
    {
        if (isDubleClick) doubleClickTime -= Time.deltaTime;
        if (doubleClickTime < 0f) 
        {
            isDubleClick = false;
            doubleClickTime = 0.2f;
        }
        if(drag) GridMove(pos);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        drag = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        drag = false;
    }

    private void GridMove(Vector3 pos)
    {
        var wpos = tilemap.WorldToCell(pos);
        if(wpos.x >= minx || wpos.x <= maxx || wpos.y >= miny || wpos.y <= maxy) transform.position = wpos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        pos = Camera.main.ScreenToWorldPoint(eventData.position);
        pos.z = 10f;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isDubleClick)
        {
            DubleClick();
            return;
        }
        GameManager.ci = GetComponent<CharacterItem>();
        SelectCharacterPanel.instance.SelectChange();
        isDubleClick = true;
    }

    private void DubleClick()
    {
        CharacterEdit.instance.ValueSet();
    }
}
