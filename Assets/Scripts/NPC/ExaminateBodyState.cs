using UnityEngine;

public class ExaminateBodyState : NPCState
{
    float t = 0;
    int dir = 0;

    Vector3 enterDir;
    NPCView view;

    bool redVisible = false;
    bool someoneElse = false;
    bool blueVisibleAndDead = false;

    float frozenTime = 1.5f;
    bool frozen = false;
    bool determinated = false;

    public ExaminateBodyState(NPC _NPC, NPCStateMachine _stateMachine, string _animBoolName, Transform _targetTransform, int _stateProbability, float _stateTimeDuration) : base(_NPC, _stateMachine, _animBoolName, _targetTransform, _stateProbability, _stateTimeDuration)
    {

    }

    public override void Enter()
    {
        base.Enter();

        npc.examinate = true;

        enterDir = npc.anim.transform.up;
        npc.examinateEmote.SetActive(true);
        npc.agent.isStopped = true;
        view = npc.view;
    }

    public override void Update()
    {
        base.Update();

        foreach (NPC seenNPC in view.seenNPC)
        {
            // Si yo soy normal y estoy viendo al rojo
            if (npc.type == NPCType.normal && seenNPC.type == NPCType.red)
            {
                redVisible = true;
            }

            // Si yo soy normal y estoy viendo al azul muerto
            if (npc.type == NPCType.normal && seenNPC.type == NPCType.blue && seenNPC.dead)
            {
                blueVisibleAndDead = true;
            }

            if (npc.type == NPCType.normal && seenNPC.type == NPCType.normal && seenNPC != npc)
            {
                someoneElse = true;
            }

            // Si yo soy normal y estoy viendo al azul muerto, lo estoy examinando, lo estoy determinando y no estoy alerta
            if (seenNPC.dead && npc.type == NPCType.red)
            {
                npc.bloqMovement = true;
            }
        }

        if (!determinated) //SI AUN NO SE HA DETERMINADO LA WIN-LOSE CONDITION
        {
            if (blueVisibleAndDead && redVisible && !someoneElse) //WIN CONDITION
            {
                determinated = true;
                frozen = true;

                npc.win = true;
            }
            else if (someoneElse) //LOSE CONDITION 1#
            {
                determinated = true;
                frozen = true;

                npc.loose = true;
            }
            else if (stateTimer <= 0) //LOSE CONDITION 2#
            {
                determinated = true;

                npc.loose = true;
            }
        }
        else 
        {
            if (frozen && frozenTime > 0)
            {
                frozenTime -= Time.deltaTime;
            }
            else if (frozen && frozenTime <= 0) 
            {
                frozen = false;
            }
            

            if (!frozen) //GOES TO ALERT IF DETERMINATED & !FROZEN
            {             
                AlertState alertState = new AlertState(npc, npc.stateMachine, "Idle", npc.door, 100, 4);
                npc.InyectedState(alertState);
            }
        }


        //LOOK AROUND
        if (t > 0.6f)
        {
            switch (dir)
            {
                case 0:
                    dir++;
                    npc.anim.transform.up = Vector3.right;
                    break;

                case 1:
                    dir++;
                    npc.anim.transform.up = -Vector3.right;
                    break;

                case 2:
                    dir++;
                    npc.anim.transform.up = Vector3.up;
                    break;

                case 3:
                    dir++;
                    npc.anim.transform.up = -Vector3.up;
                    break;

                default:
                    dir = 0;
                    npc.anim.transform.up = enterDir;
                    break;

            }

            t = 0;
        }
        else
        {
            if(!frozen) t += Time.deltaTime;
        }     
    }

    public override void Exit()
    {
        base.Exit();

        npc.examinate = false;
        npc.alert = true;

        npc.examinateEmote.SetActive(false);
        npc.agent.isStopped = false;
    }
}
