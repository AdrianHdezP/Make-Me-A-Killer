using NavMeshPlus.Components;
using UnityEngine;
using UnityEngine.AI;

public class InvokeController : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    [SerializeField] ParticleSystem magicalParticles;
    [SerializeField] AudioSource errorSound;
    [SerializeField] Transform returnToDeckTf;
    public CardHolder cardHolder;


    public static InvokeController i;

    public enum InvokeState
    {
        none,
        casting,
    }

    public InvokeState state;
    Caster currentCast;
    Card currentCard;

    private void Awake()
    {
        i = this;

        returnToDeckTf.transform.position = new Vector3(returnToDeckTf.transform.position.x, returnToDeckTf.transform.position.y, 0);
        returnToDeckTf.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (state == InvokeState.casting && currentCast != null) CastObstucle();
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

        returnToDeckTf.gameObject.SetActive(false);
    }
}
