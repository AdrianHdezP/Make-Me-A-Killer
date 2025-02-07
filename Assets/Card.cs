using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] UnityEvent OnCardCast;

    ScreenController screenController;
    Grabeable grab;
    TransformLerper lerper;

    Image cardImage;

    private void Awake()
    {
        screenController = FindFirstObjectByType<ScreenController>();

        grab = GetComponent<Grabeable>();   
        lerper = GetComponent<TransformLerper>();
        cardImage = GetComponent<Image>();
    }

    private void Update()
    {
        if (grab.grabbed) SetOpacity();
    }

    void SetOpacity()
    {
        Color color = cardImage.color;
        color.a = Mathf.Clamp((transform.position.y) / (screenController.minY + screenController.screenH * 0.35f), 0.2f, 1f);
        cardImage.color = color;
    }

    public void TryConsumeCard()
    {
        if (transform.position.y > screenController.minY + screenController.screenH * 0.35f)
        {
            lerper.locked = true;
            OnCardCast.Invoke();
            Destroy(gameObject);
        }
        else
        {
            lerper.locked = false;
        }
    }
}
