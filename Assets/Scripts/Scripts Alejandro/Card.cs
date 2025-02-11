using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] Caster castObject;

    ScreenController screenController;
    Grabeable grab;
    [HideInInspector] public TransformLerper lerper;

    Image cardImage;

    bool beenCasted;

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
        else if (cardImage.color.a != 1)
        {
            Color color = cardImage.color;
            color.a = 1;
            cardImage.color = color;
        }
    }

    private void OnEnable()
    {
        if (beenCasted)
        {
            beenCasted = false;
            grab.grabbed = false;

            cardImage.raycastTarget = false;

            StartCoroutine(ResetCard());
        }
    }


    void SetOpacity()
    {
        Color color = cardImage.color;

        if (transform.position.y > screenController.minY + screenController.screenH * 0.15f)
        {
            color.a = 0.2f;
        }
        else
        {
            color.a = 1;
        }

        // color.a = Mathf.Clamp(1 -(transform.position.y - screenController.minY) / ((screenController.screenH * 0.45f) - screenController.minY), 0.2f, 1f);
        cardImage.color = color;
    }
    public void TryConsumeCard()
    {
        if (transform.position.y > screenController.minY + screenController.screenH * 0.15f)
        {      
            InvokeController.i.StartCast(castObject, this);              
            beenCasted = true;
        }
        else
        {
            lerper.locked = false;
            grab.grabbed = false;           
        }
    }


    IEnumerator ResetCard()
    {
        lerper.locked = false;

        while (Vector3.Distance(transform.position, lerper.targetPosition) > 0.05f)
        {
            SetOpacity();
            yield return null;
        }

        cardImage.raycastTarget = true;
    }
}
