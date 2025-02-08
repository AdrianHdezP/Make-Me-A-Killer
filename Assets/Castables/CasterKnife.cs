using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterKnife : Caster
{
    [SerializeField] Animator knifeAnim;
    List<NPC> npcs = new();


    private void Start()
    {
        canBePlaced = false;
        placed = false;
        renderer_.color = castingColor;
    }

    private void Update()
    {
        canBePlaced = npcs.Count == 1;

        if (!placed)
        {
            if (!canBePlaced) renderer_.color = blockedColor;
            else renderer_.color = castingColor;
        }
        else if (renderer_.color != placedColor)
        {
            renderer_.color = placedColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out NPC npc) && !npcs.Contains(npc)) npcs.Add(npc);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out NPC npc) && npcs.Contains(npc)) npcs.Remove(npc);
    }


    public override void Place()
    {
        knifeAnim.SetTrigger("Drop");

        placed = true;
        renderer_.color = placedColor;

        for (int i = 0; i < npcs.Count; i ++)
        {
            npcs[i].KillNPC();
        }

        Destroy(gameObject, 1f);
    }
}
