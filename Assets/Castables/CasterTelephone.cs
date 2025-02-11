using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CasterTelephone : Caster
{
    [SerializeField] Animator phoneAnim;
    [SerializeField] AudioSource audioSource;
    [SerializeField] Vector2 callLimits;
    List<NPC> npcs = new();

    bool called;

    [SerializeField] float callTimer;
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
            canBePlaced = SetPlaceState();

            if (!canBePlaced) renderer_.color = blockedColor;
            else renderer_.color = castingColor;
        }
        else if (renderer_.color != placedColor)
        {
            renderer_.color = placedColor;
        }


        if (placed)
        {
            if (t < callTimer)
            {
                t += Time.deltaTime;
            }
            else if (!called)
            {
                called = true;

                List<NPC> allNPCs = new();

                foreach (NPC npc in FindObjectsByType<NPC>(FindObjectsSortMode.None))
                {
                    if (!npc.CompareTag("Special")) allNPCs.Add(npc);
                }
    
                int amount = (int)Random.Range(callLimits.x, callLimits.y);

                while (npcs.Count < amount && allNPCs.Count > 0)
                {
                    NPC npc = allNPCs [Random.Range(0, allNPCs.Count)];

                    if (!npcs.Contains(npc))
                    {
                        npcs.Add(npc);
                        allNPCs.Remove(npc);
                    }
                }

                for (int i = 0; i < npcs.Count; i++)
                {
                    npcs[i].InyectedState(npcs[i].phoneState);
                }

                Destroy(gameObject, 2.5f);
            }
        }
    }

    public override void Place()
    {
        phoneAnim.SetTrigger("Call");
        audioSource.Play();

        placed = true;
        renderer_.color = placedColor;

    }
}
