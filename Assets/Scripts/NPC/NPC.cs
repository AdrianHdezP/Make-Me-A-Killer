using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum Task
{
    Talk,
    Phone,
    Drink,
    Dance,
    Pee,
}

public class NPC : MonoBehaviour
{
    #region Componets

    public Animator anim {  get; private set; }
    public NavMeshAgent agent { get; private set; }

    #endregion

    #region States

    public NPCStateMachine stateMachine {  get; private set; }


    public List<NPCState> states;

    public MoveState moveState { get; private set; }
    public PhoneState phoneState { get; private set; }
    public TalkState talkState { get; private set; }

    #endregion

    #region Variables

    public int talkStateProbability;
    public float talkStateTimeDuration;
    public int phoneStateProbability;
    public float phoneStateTimeDuration;

    [HideInInspector] public NPCState nextState;
    [HideInInspector] public Targets targets;

    #endregion

    private void Awake()
    {
        #region Initialize States

        stateMachine = new NPCStateMachine();

        moveState = new MoveState(this, stateMachine, "Move");

        talkState = new TalkState(this, stateMachine, "Talk", targets.talkTransform, talkStateProbability, talkStateTimeDuration);
        phoneState = new PhoneState(this, stateMachine, "Phone", targets.phoneTransform, phoneStateProbability, phoneStateTimeDuration);

        states = new List<NPCState>();
        states.Add(talkState);
        states.Add(phoneState);

        #endregion
    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        stateMachine.Initialize(moveState);
    }

    private void Update()
    {
        stateMachine.currentState.Update();
    }

    public void MoveToTarget() => agent.SetDestination(nextState.targetTransform.position);

    public void SetNextTarget()
    {
        int randomProbability = Random.Range(0, 100);
        NPCState randomNPCState = states[Random.Range(0, states.Count)];

        Debug.Log(randomProbability.ToString() + " - " + randomNPCState.stateProbability.ToString());

        if (randomProbability >= randomNPCState.stateProbability)
        {
            nextState = randomNPCState;
            Debug.Log(nextState.ToString());
        }
        else
        {
            SetNextTarget();
        }
    }
}
