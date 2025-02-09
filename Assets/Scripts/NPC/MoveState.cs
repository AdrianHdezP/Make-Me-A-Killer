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

        if (NPC.alert)
            NPC.alertEmote.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();

        NPC.inyectedState = false;

        if (NPC.alert)
            NPC.alertEmote.SetActive(false);
    }

    public override void Update()
    {
        base.Update();

        if (NPC.examinate && !NPC.alert)
        {
            NPC.MoveToTarget(NPC.bodyTarget);

            if (Vector2.Distance(NPC.transform.position, NPC.bodyTarget.position) <= NPC.agent.stoppingDistance + 0.5f)
                stateMachine.ChangeState(NPC.examinateBodyState);
        }
        else if (!NPC.examinate && NPC.alert)
        {
            NPC.MoveToTarget(NPC.door);

            if (Vector2.Distance(NPC.transform.position, NPC.door.position) <= NPC.agent.stoppingDistance + 0.5f)
                stateMachine.ChangeState(NPC.alertState);
        }
        else if (!NPC.examinate && !NPC.alert)
        {
            NPC.MoveToTarget();

            if (Vector2.Distance(NPC.transform.position, NPC.nextState.targetTransform.position) <= NPC.agent.stoppingDistance + 0.5f)
                stateMachine.ChangeState(NPC.nextState);
        }
    }
}
