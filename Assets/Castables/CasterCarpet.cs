
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterCarpet : Caster
{
    [SerializeField] Animator carpetAnim;
    [SerializeField] Card carpetCardPrefab;
    [SerializeField] Sprite placedCarpet;
    List<NPC> npcs = new();


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

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out NPC npc) && !npcs.Contains(npc) && npc.dead && !placed)
        {
            npcs.Add(npc);
        }
        else if (!placed && !collision.GetComponent<NPC>()) canBePlaced = false;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out NPC npc) && npcs.Contains(npc) && !placed)
        {
            npcs.Remove(npc);
        }

        if (!placed) canBePlaced = true;
    }

    public override void Place()
    {
        placed = true;
        renderer_.color = placedColor;       
        carpetAnim.SetTrigger("Deploy");

        foreach (NPC npc in npcs)
        {
            npc.gameObject.SetActive(false);
        }

        if (npcs.Count > 0) renderer_.sprite = placedCarpet;

        StartCoroutine(TimeOutCollider());
    }

    IEnumerator TimeOutCollider()
    {
        float t = 0;
        collider_.enabled = false;

        while (t < 0.5f)
        {
            t += Time.deltaTime;
            yield return null;
        }

        collider_.enabled = true;
    }

    public void LiftCarpet()
    {
        foreach (NPC npc in npcs)
        {
            npc.gameObject.SetActive(true);
        }

        Card card =  Instantiate(carpetCardPrefab, InvokeController.i.cardHolder.transform);
        Destroy(gameObject);
    }

}
