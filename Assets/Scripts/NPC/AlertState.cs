using UnityEngine;

public class AlertState : NPCState
{
    public AlertState(NPC _NPC, NPCStateMachine _stateMachine, string _animBoolName, Transform _targetTransform, int _stateProbability, float _stateTimeDuration) : base(_NPC, _stateMachine, _animBoolName, _targetTransform, _stateProbability, _stateTimeDuration)
    {

    }

    public override void Enter()
    {
        base.Enter();

        npc.agent.isStopped = true;

        if (npc.win)
        {
            npc.manager.LoadScene("Win");
        }
        
        if (npc.loose)
        {
            npc.manager.LoadScene("Lose");
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
