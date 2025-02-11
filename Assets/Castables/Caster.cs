using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UI;

public class Caster : MonoBehaviour
{
    public SpriteRenderer renderer_;
    public Collider2D collider_;
    public LayerMask floorLayer;
    public LayerMask obstacleLayer;
    [HideInInspector] public bool placed;
    [HideInInspector] public bool canBePlaced = true;

    public Color castingColor;
    public Color blockedColor;
    public Color placedColor;

    public virtual void Place()
    {     
    }

    public bool SetPlaceState()
    {
        bool canCast = false;
        bool hitObstacle = false;

        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 0.2f, transform.forward, 0.01f);

        foreach (RaycastHit2D hit in hits)
        {
            if ((floorLayer & (1 << hit.collider.gameObject.layer)) != 0)
            {
                Debug.Log(hit.collider.gameObject.name);
                canCast = true;
            }

            if (hit.collider != collider_ && (obstacleLayer & (1 << hit.collider.gameObject.layer)) != 0)
            {
                Debug.Log(hit.collider.gameObject.name);

                hitObstacle = true;
            }
        }

        if (canCast && !hitObstacle) return true;
        else return false;
    }
}
