using UnityEngine;

public class PhoneState : NPCState
{
    public PhoneState(NPC _NPC, NPCStateMachine _stateMachine, string _animBoolName, Transform _targetTransform, int _stateProbability, float _stateTimeDuration) : base(_NPC, _stateMachine, _animBoolName, _targetTransform, _stateProbability, _stateTimeDuration)
    {
    }

    public override void Enter()
    {
        base.Enter();

        NPC.phoneEmote.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();

        NPC.phoneEmote.SetActive(false);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0)
            stateMachine.ChangeState(NPC.moveState);
    }
}
