using UnityEngine;

public class ExaminateBodyState : NPCState
{
    public ExaminateBodyState(NPC _NPC, NPCStateMachine _stateMachine, string _animBoolName) : base(_NPC, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        NPC.examinateEmote.SetActive(true);

        NPC.agent.isStopped = true;
        stateTimer = 2.5f;
    }

    public override void Exit()
    {
        base.Exit();

        NPC.examinateEmote.SetActive(false);

        NPC.agent.isStopped = false;
        NPC.examinate = false;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 1)
            NPC.determine = true;
    }
}
