using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterBomb : Caster
{
    [SerializeField] GameObject bomb;
    [SerializeField] Animator bombAnim;
    [SerializeField] GameObject effect;

    List<Collider2D> colliders = new();
    List<NPC> npcs = new();

    [SerializeField] float explosionTimer;
    float t = 0;

    Vector3 scale;

    private void Start()
    {
        canBePlaced = false;
        placed = false;
        renderer_.color = castingColor;
    }

    private void Update()
    {
        canBePlaced = colliders.Count > 0 || npcs.Count > 0;

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
            if (t < explosionTimer)
            {
                t += Time.deltaTime;

                transform.localScale = scale * t/explosionTimer;
            }
            else
            {
                for (int i = 0; i < colliders.Count;)
                {
                    if (colliders[0].gameObject)
                    {
                        GameObject instance = colliders[0].gameObject;
                        colliders.RemoveAt(0);
                        Destroy(instance);
                    }
                }

                for (int i = 0; i < npcs.Count; i++)
                {
                    npcs[i].KillNPC();
                }

                Instantiate(effect, bomb.transform.position, Quaternion.identity);

                Destroy(gameObject);
                Destroy(bomb);

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CasterObstacle>() && !colliders.Contains(collision)) colliders.Add(collision);
        else if (collision.TryGetComponent(out NPC npc) && !npcs.Contains(npc)) npcs.Add(npc);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<CasterObstacle>() && colliders.Contains(collision)) colliders.Remove(collision);
        else if (collision.TryGetComponent(out NPC npc) && npcs.Contains(npc)) npcs.Remove(npc);
    }


    public override void Place()
    {
        placed = true;
        renderer_.color = placedColor;
        bomb.transform.SetParent(null, true);
        scale = transform.localScale;

        bombAnim.SetFloat("Speed", 1 / explosionTimer);
        bombAnim.SetTrigger("Explode");

    }
}
