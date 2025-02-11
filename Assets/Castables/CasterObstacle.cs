using UnityEngine;
using UnityEngine.AI;

public class CasterObstacle : Caster
{
    public NavMeshObstacle osbtucle_;

    private void Start()
    {
        placed = false;
        renderer_.color = castingColor;
        osbtucle_.enabled = false;

        collider_.gameObject.layer = LayerMask.NameToLayer("Summon");
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
    }

  //  private void OnTriggerStay2D(Collider2D collision)
  //  {
  //      canBePlaced = false;
  //  }
  //  private void OnTriggerExit2D(Collider2D collision)
  //  {
  //      canBePlaced = true;
  //  }

    public override void Place()
    {
        collider_.gameObject.layer = LayerMask.NameToLayer("Obstacle");
        placed = true;
        renderer_.color = placedColor;
        osbtucle_.enabled = true;
        Destroy(GetComponent<Rigidbody2D>());  
    }
}
