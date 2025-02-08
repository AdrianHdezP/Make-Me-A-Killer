using System.Collections.Generic;
using UnityEngine;

public class CasterSmoke : Caster
{
    [SerializeField] Animator SmokeAnim;
    [SerializeField] ParticleSystem smokeParticles;
    [SerializeField] Collider2D smokeZone;
    [SerializeField] Collider2D hitBox;
    [SerializeField] Transform granade;

    bool formed;
    List<NPC> npcs = new();

    [SerializeField] float smokeTimer;
    float t = 0;


    private void Start()
    {
        canBePlaced = true;
        placed = false;
        renderer_.color = castingColor;

        smokeZone.enabled = false;
        hitBox.enabled = true;
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

        granade.rotation = Quaternion.identity;


        if (placed)
        {
            if (t < smokeTimer)
            {
                if (t > smokeTimer - 3.5f && smokeParticles.isPlaying) smokeParticles.Stop();
                else if (t > 2.5f && !formed) formed = true;

                t += Time.deltaTime;
            }
            else
            {
                formed = false;

                for (int i = 0; i < npcs.Count;)   
                {
                    npcs[i].hidden--;
                    npcs.RemoveAt(i);
                }

                Destroy(gameObject, 3f);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out NPC npc) && !npcs.Contains(npc) && formed)
        {
            npcs.Add(npc);
            npc.hidden++;
        }

        if (!placed) canBePlaced = false;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out NPC npc) && npcs.Contains(npc))
        {
            npcs.Remove(npc);
            npc.hidden--;
        }

        if (!placed) canBePlaced = true;
    }

    public override void Place()
    {
        smokeZone.enabled = true;
        hitBox.enabled = false;

        placed = true;
        renderer_.color = placedColor; 
        
        SmokeAnim.SetTrigger("Deploy");
        smokeParticles.Play();
    }
}
