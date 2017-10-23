using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;


[RequireComponent(typeof(Image))]
public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
	public int itemCode = 0;
	public int itemScorePoints = 0;

    static public Item draggedItem;
    static public GameObject icon;
    static public Cell sourceCell;

    public delegate void DragEvent(Item item);
    static public event DragEvent OnItemDragStartEvent;
    static public event DragEvent OnItemDragEndEvent;

    public void OnBeginDrag(PointerEventData eventData) {
        sourceCell = GetComponentInParent<Cell>();
        draggedItem = this;
        icon = new GameObject("Icon");
        Image image = icon.AddComponent<Image>();
        image.sprite = GetComponent<Image>().sprite;
        image.raycastTarget = false;
        RectTransform iconRect = icon.GetComponent<RectTransform>();
        iconRect.sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, GetComponent<RectTransform>().sizeDelta.y);
        Canvas canvas = GetComponentInParent<Canvas>();

        if (canvas != null) {
            icon.transform.SetParent(canvas.transform, true);
            icon.transform.SetAsLastSibling();
        }

        if (OnItemDragStartEvent != null) {
            OnItemDragStartEvent(this);
        }
    }

    public void OnDrag(PointerEventData data) {
        if (icon != null) {
            icon.transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (icon != null) {
            Destroy(icon);
        }

        MakeVisible(true);

        if (OnItemDragEndEvent != null) {
            OnItemDragEndEvent(this);
        }

        draggedItem = null;
        icon = null;
        sourceCell = null;
    }

    public void MakeRaycast(bool condition) {
        Image image = GetComponent<Image>();

        if (image != null) {
            image.raycastTarget = condition;
        }
    }

    public void MakeVisible(bool condition) {
        GetComponent<Image>().enabled = condition;
    }
}
