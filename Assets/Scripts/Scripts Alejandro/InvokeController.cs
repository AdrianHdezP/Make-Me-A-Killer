using NavMeshPlus.Components;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Rendering.GPUSort;

public class InvokeController : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    [SerializeField] ParticleSystem magicalParticles;
    [SerializeField] AudioSource errorSound;
    [SerializeField] Transform returnToDeckTf;
    public CardHolder cardHolder;

    [Header("CARDS")]
    [SerializeField] int cardNumber;
    [SerializeField] Card[] cards;

    [SerializeField] float[] cardOdds;
    [SerializeField] bool fixOdds;

    public InvokeState state;


    public static InvokeController i;

    public enum InvokeState
    {
        none,
        casting,
    }

    Caster currentCast;
    Card currentCard;

    private void Awake()
    {
        i = this;

        returnToDeckTf.transform.position = new Vector3(returnToDeckTf.transform.position.x, returnToDeckTf.transform.position.y, 0);
        //returnToDeckTf.gameObject.SetActive(false);
    }

    private void OnValidate()
    {
        if (fixOdds)
        {
            float[] oldOdds = cardOdds;
            float totalAmount = 0;

            foreach (float odd in oldOdds)
            {
                totalAmount += odd;
            }

            for (int i = 0; i < cardOdds.Length; i++)
            {
                cardOdds[i] = oldOdds[i] / totalAmount * 100;
            }

            fixOdds = false;
        }
    }

    private void Start()
    {
        GenerateCards();
    }

    private void Update()
    {
        if (state == InvokeState.casting && currentCast != null) CastObstucle();
    }

    [ContextMenu("GENERATE")]
    public void GenerateCards()
    {
        foreach (Transform tf in cardHolder.GetComponentInChildren<Transform>())
        {
            if (tf != cardHolder.transform) Destroy(tf.gameObject);
        }


        for (int i = 0; i <= cardNumber; i++)
        {
            if (i == 0)
            {
                Instantiate(cards[0], cardHolder.transform);
                i++;
            }
            else
            {
                float r = Random.Range(0, 100f);
                float currentOdds = cardOdds[0];

                for (int e = 0; e < cards.Length; e++)
                {
                    if (r <= currentOdds)
                    {
                        Instantiate(cards[e], cardHolder.transform);
                        break;
                    }
                    else if (e+1 < cards.Length) currentOdds += cardOdds[e+1];
                    else
                    {
                        Instantiate(cards[cards.Length-1], cardHolder.transform);
                    }
                }
            }
        }
    }

    void CastObstucle()
    {
        if (Input.GetKey(KeyCode.Mouse1) && (currentCast.GetComponent<CasterObstacle>() || currentCast.GetComponent<CasterSmoke>() || currentCast.GetComponent<CasterCarpet>()))
        {
            currentCast.transform.rotation *= Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            TryEndCast();
        }

        if (currentCast != null) currentCast.transform.position = new Vector3(CursorController.i.transform.position.x, CursorController.i.transform.position.y, 0);

    }

    public void StartCast(Caster object_, Card card)
    {
        returnToDeckTf.gameObject.SetActive(true);

        currentCard = card;
        currentCard.gameObject.SetActive(false);

        currentCast = Instantiate(object_, null);
        currentCast.transform.position = new Vector3(CursorController.i.transform.position.x, CursorController.i.transform.position.y, 0);
        state = InvokeState.casting;
    }
    public void TryEndCast()
    {
        if (currentCast.canBePlaced && Vector3.Distance(currentCast.transform.position, returnToDeckTf.position) > 2f)
        {
            state = InvokeState.none;

            Instantiate(magicalParticles, currentCast.transform.position, Quaternion.identity);

            currentCast.Place();
            currentCast = null;

            Destroy(currentCard.gameObject);
            currentCard = null;
        }
        else 
        {
            state = InvokeState.none;

            if (!currentCast.canBePlaced && Vector3.Distance(currentCast.transform.position, returnToDeckTf.position) > 2f) errorSound.Play();

            Destroy(currentCast.gameObject);
            currentCast = null;


            currentCard.gameObject.SetActive(true);
            currentCard.lerper.locked = false;
            currentCard.transform.position = CursorController.i.transform.position;

            currentCard = null;

            Debug.Log("CANT BE PLACED HERE!!");
        }

        //returnToDeckTf.gameObject.SetActive(false);
    }
}
