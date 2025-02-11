using UnityEngine;

public class TVState : NPCState
{
    public TVState(NPC _NPC, NPCStateMachine _stateMachine, string _animBoolName, Transform _targetTransform, int _stateProbability, float _stateTimeDuration) : base(_NPC, _stateMachine, _animBoolName, _targetTransform, _stateProbability, _stateTimeDuration)
    {
    }

    public override void Enter()
    {
        base.Enter();

        npc.tvEmote.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();

        npc.tvEmote.SetActive(false);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0)
            stateMachine.ChangeState(npc.moveState);
    }
}
