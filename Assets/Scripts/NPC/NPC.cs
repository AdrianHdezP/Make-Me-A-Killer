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

    [SerializeField] SpriteRenderer rederer;
    [SerializeField] Sprite deadSprite;

    public Animator anim {  get; private set; }
    public NavMeshAgent agent { get; private set; }

    #endregion

    #region States

    public NPCStateMachine stateMachine {  get; private set; }


    public List<NPCState> states;

    public MoveState moveState { get; private set; }

    public PhoneState phoneState { get; private set; }
    public TalkState talkState { get; private set; }
    public DrinkState drinkState { get; private set; }
    public DanceState danceState { get; private set; }

    public ExaminateBodyState examinateBodyState { get; private set; }
    public DeadState deadState { get; private set; }

    #endregion

    #region Variables

    public NPCType type;
    public int talkStateProbability;
    public float talkStateTimeDuration;
    public int phoneStateProbability;
    public float phoneStateTimeDuration;
    public int drinkStateProbability;
    public float drinkStateTimeDuration;
    public int danceStateProbability;
    public float danceStateTimeDuration;

    [HideInInspector] public NPCState nextState;
    [HideInInspector] public Targets targets;
    [HideInInspector] public bool inyectedState;
    [HideInInspector] public bool examinate;
    [HideInInspector] public Vector2 bodyTarget;
    [HideInInspector] public bool dead;
    public int hidden; // IF == 0 not hidden, si > 0 hidden

    #endregion

    private void Start()
    {
        #region Initialize States

        stateMachine = new NPCStateMachine();

        moveState = new MoveState(this, stateMachine, "Move");

        talkState = new TalkState(this, stateMachine, "Idle", targets.talkTransform, talkStateProbability, talkStateTimeDuration);
        phoneState = new PhoneState(this, stateMachine, "Idle", targets.phoneTransform, phoneStateProbability, phoneStateTimeDuration);
        drinkState = new DrinkState(this, stateMachine, "Idle", targets.drinkTransform, drinkStateProbability, drinkStateTimeDuration);
        danceState = new DanceState(this, stateMachine, "Idle", targets.drinkTransform, danceStateProbability, danceStateTimeDuration); //AÑADIDO DANCE STATE :)

        examinateBodyState = new ExaminateBodyState(this, stateMachine, "Idle");
        deadState = new DeadState(this, stateMachine, "Dead");

        states = new List<NPCState>();
        states.Add(talkState);
        states.Add(phoneState);
        states.Add(drinkState);
        states.Add(danceState);

        #endregion

        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        stateMachine.Initialize(moveState);
    }

    private void OnEnable()
    {
        if (dead)
        {
            anim.enabled = false;
            rederer.sprite = deadSprite;
        }

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

    public void MoveToTarget(Vector2 target)
    {
        agent.SetDestination(target);

        if ((agent.CalculatePath(target, agent.path) && agent.pathStatus == NavMeshPathStatus.PathComplete) == false)
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

        //if (newState == examinateBodyState)
        //{
        //    stateMachine.ChangeState(examinateBodyState);
        //    return;
        //}

        inyectedState = true;
        stateMachine.ChangeState(moveState);
        nextState = newState;
    }

    public void KillNPC() => stateMachine.ChangeState(deadState);   
}
