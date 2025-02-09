using UnityEngine;

public class ExaminateBodyState : NPCState
{
    public ExaminateBodyState(NPC _NPC, NPCStateMachine _stateMachine, string _animBoolName) : base(_NPC, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        NPC.agent.isStopped = true;
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
