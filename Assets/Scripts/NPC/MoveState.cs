using UnityEngine;

public class MoveState : NPCState
{
    public MoveState(NPC _NPC, NPCStateMachine _stateMachine, string _animBoolName) : base(_NPC, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        NPC.SetNextTarget();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        NPC.MoveToTarget();

        if (Vector2.Distance(NPC.transform.position, NPC.nextState.targetTransform.position) <= 0.5f)
            stateMachine.ChangeState(NPC.nextState);
    }
}
