using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;


[RequireComponent(typeof(Image))]
public class Cell : MonoBehaviour, IDropHandler {
    public enum CellType { Swap, DropOnly, DragOnly }

	public CellType cellType = CellType.Swap;

    public Color emptyColour = new Color();
    public Color fullColour = new Color();

	public int cellCode = 0;

    void OnEnable() {
        Item.OnItemDragStartEvent += OnAnyItemDragStart;
        Item.OnItemDragEndEvent += OnAnyItemDragEnd;
    }

    void OnDisable() {
        Item.OnItemDragStartEvent -= OnAnyItemDragStart;
        Item.OnItemDragEndEvent -= OnAnyItemDragEnd;
    }

    void Start() {
        SetBackgroundState(GetComponentInChildren<Item>() == null ? false : true);
    }

	private void OnAnyItemDragStart(Item item) {
        Item myItem = GetComponentInChildren<Item>();

        if (myItem != null) {
            myItem.MakeRaycast(false);

            if (myItem == item) {
                switch (cellType) {
                    case CellType.DropOnly:
                        Item.icon.SetActive(false);
                        break;
                    default:
                        item.MakeVisible(false);
                        SetBackgroundState(false);
                        break;
                }
            }
        }
    }

    private void OnAnyItemDragEnd(Item item) {
        Item myItem = GetComponentInChildren<Item>();

        if (myItem != null) {
            if (myItem == item) {
                SetBackgroundState(true);
            }

            myItem.MakeRaycast(true);
        } else {
            SetBackgroundState(false);
        }
    }

    public void OnDrop(PointerEventData data) {
        if (Item.icon != null) {
            if (Item.icon.activeSelf == true) {
                Item item = Item.draggedItem;
                Cell sourceCell = Item.sourceCell;

                if ((item != null) && (sourceCell != this)) {
                    switch (cellType) {
                        case CellType.Swap:
                            Item currentItem = GetComponentInChildren<Item>();

                            switch (sourceCell.cellType) {
                                case CellType.Swap:
                                    SwapItems(sourceCell, this);
                                    break;
                                default:
                                    PlaceItem(item.gameObject);
                                    break;
                            }
                            break;
						case CellType.DropOnly:
							PlaceItem (item.gameObject);
                            break;
                        default:
                            // Nothing to do
                            break;
                    }
                }

                if (item.GetComponentInParent<Cell>() == null) {
                    Destroy(item.gameObject);
                }

				StartCoroutine(NotifyOnDragEnd());
            }
        }
    }

    private void SetBackgroundState(bool condition) {
        GetComponent<Image>().color = condition ? fullColour : emptyColour;
    }

    public void RemoveItem() {
        foreach (Item item in GetComponentsInChildren<Item>()) {
            Destroy(item.gameObject);
        }

        SetBackgroundState(false);
    }

    public void PlaceItem(GameObject itemObj) {
        RemoveItem();

        if (itemObj != null) {
            itemObj.transform.SetParent(transform, false);
            itemObj.transform.localPosition = Vector3.zero;
            Item item = itemObj.GetComponent<Item>();

            if (item != null) {
                item.MakeRaycast(true);
            }

            SetBackgroundState(true);
        }
    }

    public Item GetItem() {
        return GetComponentInChildren<Item>();
    }

    public void SwapItems(Cell firstCell, Cell secondCell) {
        if ((firstCell != null) && (secondCell != null)) {
            Item firstItem = firstCell.GetItem();
            Item secondItem = secondCell.GetItem();

            if (firstItem != null) {
                firstItem.transform.SetParent(secondCell.transform, false);
                firstItem.transform.localPosition = Vector3.zero;
                secondCell.SetBackgroundState(true);
            }

            if (secondItem != null) {
                secondItem.transform.SetParent(firstCell.transform, false);
                secondItem.transform.localPosition = Vector3.zero;
                firstCell.SetBackgroundState(true);
            }
        }
    }

	private IEnumerator NotifyOnDragEnd() {
		while (Item.draggedItem != null) {
			yield return new WaitForEndOfFrame();
		}

		gameObject.SendMessageUpwards("OnItemPlaced", null, SendMessageOptions.DontRequireReceiver);
	}
}
