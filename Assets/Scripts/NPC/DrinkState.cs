using UnityEngine;

public class DrinkState : NPCState
{
    public DrinkState(NPC _NPC, NPCStateMachine _stateMachine, string _animBoolName, Transform _targetTransform, int _stateProbability, float _stateTimeDuration) : base(_NPC, _stateMachine, _animBoolName, _targetTransform, _stateProbability, _stateTimeDuration)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = NPC.drinkStateTimeDuration;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0)
            stateMachine.ChangeState(NPC.moveState);
    }
}
