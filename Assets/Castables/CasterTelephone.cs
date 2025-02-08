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
            else
            {
                List<NPC> allNPCs = new();

                foreach (NPC npc in FindObjectsByType<NPC>(FindObjectsSortMode.None))
                {
                    if (!npc.CompareTag("Special")) allNPCs.Add(npc);
                }

                int added = 0;
                int amount = (int)Random.Range(callLimits.x, callLimits.y);

                if (amount > allNPCs.Count) amount = allNPCs.Count - 1;

                while (added < amount)
                {
                    NPC npc = allNPCs[Random.Range(0, allNPCs.Count)];

                    if (!npcs.Contains(npc))
                    {
                        npcs.Add(npc);
                        added++;
                    }
                }

                for (int i = 0; i < npcs.Count;)
                {
                    npcs[i].InyectedState(npcs[i].phoneState);
                }

                Destroy(gameObject, 0.5f);
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
