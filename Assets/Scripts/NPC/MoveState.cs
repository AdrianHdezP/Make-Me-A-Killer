using UnityEngine;

public class MoveState : NPCState
{
    public MoveState(NPC _NPC, NPCStateMachine _stateMachine, string _animBoolName) : base(_NPC, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (!npc.inyectedState)
            npc.SetNextTarget();

        if (npc.alert)
            npc.alertEmote.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();

        npc.inyectedState = false;
        npc.lastTargetPos = null;

        if (npc.alert)
            npc.alertEmote.SetActive(false);
    }

    public override void Update()
    {
        base.Update();

        npc.MoveToTarget();

        if (Vector2.Distance(npc.transform.position, npc.nextState.targetTransform.position) <= npc.agent.stoppingDistance + 0.5f)
        {
            stateMachine.ChangeState(npc.nextState);
        }          
    }
}

