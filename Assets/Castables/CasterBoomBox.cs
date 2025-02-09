using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class CasterBoomBox : Caster
{
    [SerializeField] Animator boomBox;
    [SerializeField] AudioSource audioSource;

    List<NPC> npcs = new();

    [SerializeField] float musicThreshHold;
    [SerializeField] float musicTimer;
    float t = 0;


    private void Start()
    {
        canBePlaced = true;
        placed = false;
        renderer_.color = castingColor;
    }

    private void Update()
    {
        if (!placed)
        {
            if (!canBePlaced) renderer_.color = blockedColor;
            else renderer_.color = castingColor;
        }
        else if (renderer_.color != placedColor)
        {
            renderer_.color = placedColor;
        }


        if (placed)
        {
            if (t < musicTimer)
            {
                t += Time.deltaTime;


                List<NPC> allNPCs = new();

                foreach (NPC npc in FindObjectsByType<NPC>(FindObjectsSortMode.None))
                {
                   if (!npc.dead) allNPCs.Add(npc);
                }

                for (int i = 0; i < allNPCs.Count; i++)
                {
                    if (Vector3.Distance(transform.position, allNPCs[i].transform.position) < musicThreshHold && !npcs.Contains(allNPCs[i])) //
                    {
                        npcs.Add(allNPCs[i]);

                        DanceState danceState = new DanceState(allNPCs[i], allNPCs[i].stateMachine, "Idle", transform, 100, musicTimer - t);
                        allNPCs[i].InyectedState(danceState);
                    }
                }

            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public override void Place()
    {
        boomBox.SetTrigger("Call");
        audioSource.Play();

        placed = true;
        renderer_.color = placedColor;
    }
}
