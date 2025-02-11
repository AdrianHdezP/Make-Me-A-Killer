using UnityEngine;

public class PeeState : NPCState
{
    public PeeState(NPC _NPC, NPCStateMachine _stateMachine, string _animBoolName, Transform _targetTransform, int _stateProbability, float _stateTimeDuration) : base(_NPC, _stateMachine, _animBoolName, _targetTransform, _stateProbability, _stateTimeDuration)
    {
    }

    public override void Enter()
    {
        base.Enter();

        npc.peeEmote.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();

        npc.peeEmote.SetActive(false);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0)
            stateMachine.ChangeState(npc.moveState);
    }
}
