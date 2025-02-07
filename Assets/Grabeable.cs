using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabeable : MonoBehaviour
{
    [SerializeField] Transform grabbedObject;

    [HideInInspector] public bool grabbed;

    Vector3 posOffset;
    Transform grabber;

    Vector3 startLocalPos;

    [SerializeField] float speedMultiplier;
    Vector2 velocity;
    Vector2 lastPos;

    TransformLerper lerper;

    private void Awake()
    {
        lerper = GetComponent<TransformLerper>();
    }

    private void Update()
    {
        if (grabbed)
        {
            ExecuteGrab();
            TiltCard();
        }
    }

    private void FixedUpdate()
    {
        if (grabbed)
        {
            velocity = ((Vector2)transform.position - lastPos) / Time.fixedDeltaTime;
            lastPos = transform.position;
        }
        else velocity = Vector2.zero;
    }

    public void StartGrab()
    {
        if (!grabbed)
        {
            grabber = CursorController.i.transform;
            posOffset = grabber.position - grabbedObject.position;
            grabbed = true;
            CursorController.i.holding = true;
        }
    }

    public void StartTemporalGrab()
    {
        if (!grabbed)
        {
            startLocalPos = grabbedObject.transform.localPosition;
            grabber = CursorController.i.transform;
            posOffset = Vector3.zero;
            grabbed = true;
            CursorController.i.holding = true;
        }
    }
    public void EndTemporalGrab()
    {
        grabber = null;
        posOffset = Vector3.zero;
        grabbedObject.transform.localPosition = startLocalPos;

        grabbed = false;
    }

    void ExecuteGrab()
    {
        grabbedObject.position = CursorController.i.CapPosToScreen(grabber.position - posOffset, CursorController.i.screenC);      
        CursorController.i.BringToFront(grabbedObject);
    }
    public void BringToFront()
    {
        CursorController.i.BringToFront(grabbedObject);
    }

    public void EndGrab()
    {
        grabber = null;
        posOffset = Vector3.zero;

        grabbed = false;
    }

    void TiltCard()
    {
        lerper.targetRotation = Quaternion.AngleAxis(velocity.x * speedMultiplier, Vector3.forward) * Quaternion.AngleAxis(velocity.y * speedMultiplier, Vector3.up) * Quaternion.AngleAxis(velocity.x * speedMultiplier, Vector3.right);
    }
}
