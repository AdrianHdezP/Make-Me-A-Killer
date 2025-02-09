using UnityEngine;

public class AlertState : NPCState
{
    public AlertState(NPC _NPC, NPCStateMachine _stateMachine, string _animBoolName) : base(_NPC, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        NPC.agent.isStopped = true;

        if (NPC.win)
        {
            NPC.manager.LoadScene("Win");
        }
        
        if (NPC.loose)
        {
            NPC.manager.LoadScene("Lose");
        }
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
