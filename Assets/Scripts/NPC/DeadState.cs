using UnityEngine;

public class DeadState : NPCState
{
    public DeadState(NPC _NPC, NPCStateMachine _stateMachine, string _animBoolName) : base(_NPC, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        npc.dead = true;
        npc.agent.destination = npc.transform.position;
        npc.spriteRenderer.sortingOrder = 2;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
