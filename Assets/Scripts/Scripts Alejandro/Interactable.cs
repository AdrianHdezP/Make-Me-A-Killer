using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] bool UseDebugs;
    [SerializeField] public CursorController.EventStates state;

    [Space]
    public UnityEvent OnPointerEnter;
    public UnityEvent OnPointerExit;
    public UnityEvent OnPointerHover;

    [Space]
    public UnityEvent OnPointerDown;

    [Space]
    public UnityEvent OnDoubleClick;

    [Space]
    public UnityEvent OnPointerUp;
    public UnityEvent OnSecondaryPointerDown;
    public UnityEvent OnSecondaryPointerUp;

    [Space]
    public UnityEvent OnPointerHold;
    public UnityEvent OnPointerDrag;
    public UnityEvent OnPressOutside;

    private void Awake()
    {
        if (UseDebugs)
        {
            OnPointerDown.AddListener(PointerDownDebug);
            OnPointerHover.AddListener(PointerHoverDebug);
            OnSecondaryPointerDown.AddListener(SecondaryPointerDownDebug);
            OnSecondaryPointerUp.AddListener(SecondaryPointerUpDebug);
            OnPointerUp.AddListener(PointerUpDebug);
            OnDoubleClick.AddListener(DoubleClickDebug);
            OnPointerEnter.AddListener(PointerEnterDebug);
            OnPointerExit.AddListener(PointerExitDebug);
            OnPointerHold.AddListener(PointerHoldDebug);
            OnPointerDrag.AddListener(PointerDragDebug);
            OnPressOutside.AddListener(OutsidePressDebug);
        }
    }

    public void PointerDownDebug()
    {
        Debug.Log("POINTER IS DOWN ON " + name);
    }
    public void SecondaryPointerDownDebug()
    {
        Debug.Log("SECONDARY POINTER IS DOWN ON " + name);
    }
    public void SecondaryPointerUpDebug()
    {
        Debug.Log("SECONDARY POINTER IS UP ON " + name);
    }
    public void PointerHoverDebug()
    {
        Debug.Log("POINTER HOVERING " + name);
    }
    public void PointerUpDebug()
    {
        Debug.Log("POINTER IS UP ON " + name);
    }
    public void PointerEnterDebug()
    {
        Debug.Log("POINTER ENTERED " + name);
    }
    public void PointerExitDebug()
    {
        Debug.Log("POINTER IS OUT OF " + name);
    }
    public void PointerHoldDebug()
    {
        Debug.Log("POINTER IS HOLDING ON " + name);
    }
    public void PointerDragDebug()
    {
        Debug.Log("POINTER IS DRAGGING " + name);
    }
    public void DoubleClickDebug()
    {
        Debug.Log("DOUBLE CLICK ON " + name);
    }
    public void OutsidePressDebug()
    {
        Debug.Log("DESELECTED " + name);
    }
    public void GroupSelectionDebug()
    {
        Debug.Log("GROUP SELECTED " + name);
    }
}
