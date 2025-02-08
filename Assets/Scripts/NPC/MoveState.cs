using UnityEngine;

public class MoveState : NPCState
{
    public MoveState(NPC _NPC, NPCStateMachine _stateMachine, string _animBoolName) : base(_NPC, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (!NPC.inyectedState)
            NPC.SetNextTarget();
    }

    public override void Exit()
    {
        base.Exit();

        NPC.inyectedState = false;
    }

    public override void Update()
    {
        base.Update();

        NPC.MoveToTarget();

        if (Vector2.Distance(NPC.transform.position, NPC.nextState.targetTransform.position) <= NPC.agent.stoppingDistance + 0.5f)
            stateMachine.ChangeState(NPC.nextState);
    }
}
