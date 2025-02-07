using UnityEngine;

public class TransformLerper : MonoBehaviour
{
    public Vector3 targetPosition;
    public Quaternion targetRotation;

    [SerializeField] float positionSpeed = 1;
    [SerializeField] float rotationSpeed = 1;

    CardHolder cardHolder;

    [HideInInspector] public bool locked;

    private void Awake()
    {
        targetPosition = transform.position;
        targetRotation = transform.rotation;

        cardHolder = GetComponentInParent<CardHolder>();
    }

    private void Update()
    {
        if (locked) return;

        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.05f && transform.rotation != targetRotation) transform.rotation = targetRotation;
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

        if (Vector3.Distance(transform.position, targetPosition) < 0.001f && transform.position != targetPosition) transform.position = targetPosition;
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, positionSpeed * Time.fixedDeltaTime);
        }
    }

    public void Select()
    {
        cardHolder.selectedDisk = this;
    }
    public void Deselect()
    {
        if (cardHolder.selectedDisk == this) cardHolder.selectedDisk = null;
    }
}
