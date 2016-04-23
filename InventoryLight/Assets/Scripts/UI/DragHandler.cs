using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class DragHandler : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    private RectTransform rectTransform;

	// Use this for initialization
	void Start ()
	{
	}


    [SerializeField] private Vector3 startPosition;
    void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
       rectTransform.anchoredPosition3D = startPosition;
    }

    public bool Dragging = false;
    private Vector2 offset;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            print(Input.mousePosition);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!Dragging)
        {
            this.transform.position = eventData.position;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            Dragging = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Dragging)
        {
           this.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Dragging)
        {
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            Dragging = false;
        }
    }
}

