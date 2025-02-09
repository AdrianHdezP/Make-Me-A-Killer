using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum NPCType
{
    blue,
    red,
    normal
}

public class NPC : MonoBehaviour
{
    #region Componets

    public Animator anim {  get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }
    public NavMeshAgent agent { get; private set; }

    #endregion

    #region States

    public NPCStateMachine stateMachine {  get; private set; }


    public List<NPCState> states;

    public MoveState moveState { get; private set; }

    public PhoneState phoneState { get; private set; }
    public TalkState talkState { get; private set; }
    public EatState eatState { get; private set; }
    public DrinkState drinkState { get; private set; }
    public SmokeState smokeState { get; private set; }
    public DanceState danceState { get; private set; }
    public MusicState musicState { get; private set; }
    public TVState tvState { get; private set; }
    public PeeState peeState { get; private set; }

    public ExaminateBodyState examinateBodyState { get; private set; }
    public AlertState alertState { get; private set; }
    public DeadState deadState { get; private set; }

    #endregion

    #region Variables

    [Header("Type")]
    public NPCType type;

    [Header("Talk")]
    public int talkStateProbability;
    public float talkStateTimeDuration;
    public GameObject talkEmote;

    [Header("Phone")]
    public int phoneStateProbability;
    public float phoneStateTimeDuration;
    public GameObject phoneEmote;

    [Header("Eat")]
    public int eatStateProbability;
    public float eatStateTimeDuration;
    public GameObject eatEmote;

    [Header("Drink")]
    public int drinkStateProbability;
    public float drinkStateTimeDuration;
    public GameObject drinkEmote;

    [Header("Smoke")]
    public int smokeStateProbability;
    public float smokeStateTimeDuration;
    public GameObject smokeEmote;

    [Header("Dance")]
    public int danceStateProbability;
    public float danceStateTimeDuration;
    public GameObject danceEmote;

    [Header("Music")]
    public int musicStateProbability;
    public float musicStateTimeDuration;
    public GameObject musicEmote;

    [Header("TV")]
    public int tvStateProbability;
    public float tvStateTimeDuration;
    public GameObject tvEmote;

    [Header("Pee")]
    public int peeStateProbability;
    public float peeStateTimeDuration;
    public GameObject peeEmote;

    [Header("Examinate")]
    public GameObject examinateEmote;

    [Header("Alert")]
    public GameObject alertEmote;

    [Header("Door")]
    public Transform door;

    [HideInInspector] public NPCState nextState;
    [HideInInspector] public Targets targets;
    [HideInInspector] public bool inyectedState;
    [HideInInspector] public bool examinate;
    [HideInInspector] public Transform bodyTarget;
    [HideInInspector] public bool determine;
    [HideInInspector] public bool alert;
    [HideInInspector] public bool bloqMovement;
    [HideInInspector] public bool dead;
    [HideInInspector] public int hidden; // IF == 0 not hidden, si > 0 hidden
    [HideInInspector] public bool win;
    [HideInInspector] public bool loose;

    #endregion

    private void Start()
    {
        #region Initialize States

        stateMachine = new NPCStateMachine();

        moveState = new MoveState(this, stateMachine, "Move");

        talkState = new TalkState(this, stateMachine, "Idle", targets.talkTransform, talkStateProbability, talkStateTimeDuration);
        phoneState = new PhoneState(this, stateMachine, "Idle", targets.phoneTransform, phoneStateProbability, phoneStateTimeDuration);
        eatState = new EatState(this, stateMachine, "Idle", targets.eatTransform, eatStateProbability, eatStateTimeDuration);
        drinkState = new DrinkState(this, stateMachine, "Idle", targets.drinkTransform, drinkStateProbability, drinkStateTimeDuration);
        smokeState = new SmokeState(this, stateMachine, "Idle", targets.smokeTransform, smokeStateProbability, smokeStateTimeDuration);
        danceState = new DanceState(this, stateMachine, "Idle", targets.danceTransform, danceStateProbability, danceStateTimeDuration);
        musicState = new MusicState(this, stateMachine, "Idle", targets.musicTransform, musicStateProbability, musicStateTimeDuration);
        tvState = new TVState(this, stateMachine, "Idle", targets.tvTransform, tvStateProbability, tvStateTimeDuration);
        peeState = new PeeState(this, stateMachine, "Idle", targets.peeTransform, peeStateProbability, peeStateTimeDuration);

        examinateBodyState = new ExaminateBodyState(this, stateMachine, "Idle");
        alertState = new AlertState(this, stateMachine, "Idle");
        deadState = new DeadState(this, stateMachine, "Dead");

        states = new List<NPCState>();
        states.Add(talkState);
        states.Add(phoneState);
        states.Add(eatState);
        states.Add(drinkState);
        states.Add(smokeState);
        states.Add(danceState);
        states.Add(musicState);
        states.Add(tvState);
        states.Add(peeState);

        #endregion

        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        stateMachine.Initialize(moveState);
    }

    private void OnEnable()
    {
        if (dead)
            anim.enabled = false;
    }

    private void Update()
    {
        stateMachine.currentState.Update();

        if (dead && type == NPCType.red)
        {
            Debug.Log("Pierdes");
        }

        if (agent.velocity.magnitude > 0)
            anim.transform.up = agent.velocity.normalized;
    }

    public void MoveToTarget()
    {
        agent.SetDestination(nextState.targetTransform.position);

        if ((agent.CalculatePath(nextState.targetTransform.position, agent.path) && agent.pathStatus == NavMeshPathStatus.PathComplete) == false)
        {
            //stateMachine.currentState.Enter();
            stateMachine.ChangeState(moveState);
        }
    }

    public void MoveToTarget(Transform target)
    {
        agent.SetDestination(target.position);

        if ((agent.CalculatePath(target.position, agent.path) && agent.pathStatus == NavMeshPathStatus.PathComplete) == false)
        {
            //stateMachine.currentState.Enter();
            stateMachine.ChangeState(moveState);
        }
    }

    public void SetNextTarget()
    {
        int randomProbability = Random.Range(0, 100);
        NPCState randomNPCState = states[Random.Range(0, states.Count)];

        if (randomProbability >= randomNPCState.stateProbability)
        {
            nextState = randomNPCState;
        }
        else
        {
            SetNextTarget();
        }
    }

    public void InyectedState(NPCState newState)
    {
        if (dead)
            return;

        if (type == NPCType.red && bloqMovement)
            return;

        inyectedState = true;
        stateMachine.ChangeState(moveState);
        nextState = newState;
    }

    public void KillNPC() => stateMachine.ChangeState(deadState);   
}
