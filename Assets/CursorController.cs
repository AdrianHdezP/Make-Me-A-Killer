using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite defaultSprite;
    public Sprite interactableSprite;
    public Sprite dragSprite;

    [SerializeField] Interactable current;
    Interactable lastInteractable;
    public enum EventStates
    {
        interactable,
        drag,
        ignore
    }

    [Header("RAYCASTING")]
    [SerializeField] GraphicRaycaster m_Raycaster;
    [SerializeField] EventSystem m_EventSystem;
    [SerializeField] RectTransform canvasRect;
    public ScreenController screenC;
    PointerEventData m_PointerEventData;

    [Header("Settings")]
    public float sensibility;
    [SerializeField] float dragThreshold;
    [SerializeField] float holdTime;
    [SerializeField] float doubleClickTime;

    int activeIndex;

    float clickT;
    bool clicked;
    float holdT;
    public bool holding;

    Vector3 holdPosition;


    RectTransform rectTransform;
    Image image;


    public static CursorController i;

    private void Awake()
    {
        i = this;

        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        SetCursorSprite(0);
    }

    private void Update()
    {
        MoveCursor();

        if (!holding) GetCurrentTrigger();
        if (clicked) DoubleClickTimer();
        if (Input.GetKeyDown(KeyCode.Mouse0)) GetLastTriggerPressed();
        if (current != null)
        {
            CurrentEventController();
            current.OnPointerHover.Invoke();
        }

        if (current && current.gameObject == null)
        {
            current = null;
            holding = false;
        }

        RaycastControl();

        if (holding && !current) holding = false;

        if (current && current.state != EventStates.ignore)
        {
            if (current.state == EventStates.interactable && activeIndex != 1) SetCursorSprite(1);
            else if (current.state == EventStates.drag && activeIndex != 2) SetCursorSprite(2);
        }
        else if (activeIndex != 0 && (!current || current.state == EventStates.ignore)) SetCursorSprite(0);
    }


    void MoveCursor()
    {
        float xMove = Input.GetAxisRaw("Mouse X");
        float yMove = Input.GetAxisRaw("Mouse Y");

        Vector3 newPos = transform.position;

        newPos.x += xMove * sensibility;
        newPos.y += yMove * sensibility;

        transform.position = CapPosToScreen(newPos, screenC);
    }

    void GetCurrentTrigger()
    {
        m_PointerEventData = new PointerEventData(m_EventSystem);
        m_PointerEventData.position = Camera.main.WorldToScreenPoint(transform.position);
        List<RaycastResult> hitResults = new List<RaycastResult>();
        m_Raycaster.Raycast(m_PointerEventData, hitResults);

        Interactable newTrigger = null;

        for (int i = 0; i < hitResults.Count; i++)
        {
            if (hitResults[i].gameObject.TryGetComponent(out Interactable trigger))
            {
                newTrigger = trigger;
                break;
            }
        }

        if (current && current != newTrigger)
        {
            current.OnPointerExit.Invoke();
            if (clicked) clicked = false;
            //ReleaseObject();
        }
        if (holding && !newTrigger) holding = false;
        if (current != newTrigger)
        {
            current = newTrigger;
            if (current) current.OnPointerEnter.Invoke();
        }      
    }
    void CurrentEventController()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && clicked)
        {

            current.OnDoubleClick.Invoke();

            holdT = 0f;
            clickT = 0f;
            holdPosition = transform.localPosition;

            clicked = false;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

            current.OnPointerDown.Invoke();
           
            holdT = 0f;
            clickT = 0f;
            holdPosition = transform.localPosition;

            clicked = true;
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            current.OnSecondaryPointerDown.Invoke();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            current.OnSecondaryPointerUp.Invoke();
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            current.OnPointerUp.Invoke();
            holdT = 0;
            holding = false;
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            if (holdT < holdTime) holdT += Time.deltaTime;
            else
            {
                if (!holding)
                {
                    holding = true;
                    current.OnPointerHold.Invoke();
                }
            }

            if (Vector3.Distance(holdPosition, transform.localPosition) > dragThreshold)
            {
                if (!holding)
                {
                    holding = true;
                    current.OnPointerDrag.Invoke();
                }
            }
        }
    }

    void GetLastTriggerPressed()
    {
        if (lastInteractable != current && lastInteractable) lastInteractable.OnPressOutside.Invoke();
        lastInteractable = current;
    }
    void DoubleClickTimer()
    {
        if (clickT < doubleClickTime) clickT += Time.deltaTime;
        else clicked = false;
    }

    void RaycastControl()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.forward);

        if (hit.collider)
        {
            if (hit.collider.TryGetComponent(out Raycaster rayCaster))
            {
                if (image.sprite != interactableSprite) SetCursorSprite(1);
                if (Input.GetKeyDown(KeyCode.Mouse0)) rayCaster.OnPointerDown.Invoke();
            }
        }
    }


    public void BringToFront(Transform tf)
    {
        tf.SetSiblingIndex(tf.parent.childCount - 1);
    }

    public void SetCursorSprite(int index)
    {
        Sprite selectedSprite;

        switch (index)
        {
            case 1:
                selectedSprite = interactableSprite; break;
            case 2:
                selectedSprite = dragSprite; break;
            default:
                selectedSprite = defaultSprite; break;

        }

        Vector2 pivot = selectedSprite.pivot;

        pivot.x = pivot.x / selectedSprite.rect.size.x;
        pivot.y = pivot.y / selectedSprite.rect.size.y;

        rectTransform.pivot = pivot;
        image.sprite = selectedSprite;

        activeIndex = index;
    }
    public Vector3 CapPosToScreen(Vector3 position, ScreenController parent)
    {

        Vector3 fixedposition = position;
        if (position.x < parent.minX)
        {
            fixedposition.x = parent.minX;
        }
        else if (position.x > parent.maxX)
        {
            fixedposition.x = parent.maxX;
        }
        if (position.y < parent.minY)
        {
            fixedposition.y = parent.minY;
        }
        else if (position.y > parent.maxY)
        {
            fixedposition.y = parent.maxY;
        }
        return fixedposition;
    }
}
