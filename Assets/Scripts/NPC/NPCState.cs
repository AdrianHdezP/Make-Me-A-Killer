using UnityEngine;

public class NPCState
{
    protected NPC NPC;
    protected NPCStateMachine stateMachine;
    private string animBoolName;
    public Transform targetTransform;
    public int stateProbability;
    public float stateTimeDuration;

    public float stateTimer;

    public NPCState(NPC _NPC, NPCStateMachine _stateMachine, string _animBoolName)
    {
        this.NPC = _NPC;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public NPCState(NPC _NPC, NPCStateMachine _stateMachine, string _animBoolName, Transform _targetTransform, int _stateProbability, float _stateTimeDuration)
    {
        this.NPC = _NPC;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
        this.targetTransform = _targetTransform;
        this.stateProbability = _stateProbability;
        this.stateTimeDuration = _stateTimeDuration;
    }

    public virtual void Enter()
    {
        NPC.anim.SetBool(animBoolName, true);
        stateTimer = stateTimeDuration; //SET LA DURACION AQUI PARA PODER DARSELA COMO VALOR AL CREAR EL STATE!!!
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        NPC.anim.SetBool(animBoolName, false);
    }
}
