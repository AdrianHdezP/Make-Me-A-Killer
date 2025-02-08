using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Caster : MonoBehaviour
{
    public SpriteRenderer renderer_;
    public Collider2D collider_;
    [HideInInspector] public bool placed;
    [HideInInspector] public bool canBePlaced = true;

    public Color castingColor;
    public Color blockedColor;
    public Color placedColor;


    public virtual void Place()
    {

    }
}
