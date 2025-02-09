using System.Collections.Generic;
using UnityEngine;

public class NPCView : MonoBehaviour
{
    private NPC npc;

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject NPCCollider;

    private Mesh mesh;
    //private Vector3 origin;
    private float startingAngle;
    private float fov;
    private List<NPC> seenNPC = new List<NPC>();


    private void Start()
    {
        npc = GetComponentInParent<NPC>();
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        fov = 120f;
    }

    private void Update()
    {
        bool redVisible = false;
        bool blueVisibleAndDead = false;    
        bool someoneElse = false;

        // Para todos los NPC que estoy viendo
        for (int i = 0; i < seenNPC.Count; i++)
        {
            // Si yo soy normal y estoy viendo al rojo
            if (npc.type == NPCType.normal && seenNPC[i].type == NPCType.red)
            {
                redVisible = true;
            }

            // Si yo soy normal y estoy viendo al azul muerto
            if (npc.type == NPCType.normal && seenNPC[i].type == NPCType.blue && seenNPC[i].dead)
            {
                blueVisibleAndDead = true;
            }

            if (npc.type == NPCType.normal && seenNPC[i].type == NPCType.normal)
            {
                someoneElse = true;
            }

            // Si yo soy el rojo y estoy viendo al azul muerto
            if (npc.type == NPCType.red && seenNPC[i].type == NPCType.blue && seenNPC[i].dead && !npc.examinate)
            {
                npc.bodyTarget = seenNPC[i].transform;
                npc.examinate = true;
                npc.InyectedState(npc.examinateBodyState);
            }

            // Si yo soy normal o el rojo y estoy viendo al azul muerto y aun no lo estoy examinando ni estoy alerta
            if (seenNPC[i].dead && !npc.examinate && !npc.alert)
            {
                npc.bodyTarget = seenNPC[i].transform;
                npc.examinate = true;
                npc.InyectedState(npc.examinateBodyState);
            }

            // Si yo soy normal y estoy viendo al azul muerto, lo estoy examinando, lo estoy determinando y no estoy alerta
            if (seenNPC[i].dead && npc.examinate && npc.determine && !npc.alert)
            {
                if (seenNPC[i].type == NPCType.blue && npc.type == NPCType.red)
                    npc.bloqMovement = true;

                npc.alert = true;
                npc.InyectedState(npc.alertState);
                npc.determine = false;

                if (blueVisibleAndDead && redVisible && !someoneElse)
                {
                    npc.win = true;
                }
                else
                {
                    npc.loose = true;
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (npc.dead || npc.type == NPCType.red && npc.examinate || npc.alert)
            Destroy(mesh);

        seenNPC.Clear();
        int rayCount = 50;
        float angle = GetAngleFromVectorFloat(transform.up) - fov / 2f;
        float angleIncrease = fov / rayCount;
        float viewDistance = 5f;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = Vector3.zero;
        int vertexIndex = 1;
        int triangleIndex = 0;

        for (int i = 1; i <= rayCount; i++)
        {
            Vector3 vertex;

            RaycastHit2D[] raycastHit2D = Physics2D.RaycastAll(transform.position, GetVectorFromAngle(angle), viewDistance, layerMask);
            //Debug.DrawRay(transform.position, GetVectorFromAngle(angle) * viewDistance);

            //RaycastHit2D myRaycastHit2D = new RaycastHit2D();

            RaycastHit2D collisionHit =  new RaycastHit2D();

            foreach (var ray in raycastHit2D)
            {
                if (ray.collider.gameObject != NPCCollider)
                {
                    NPC npc = ray.collider.gameObject.GetComponent<NPC>();

                    if (npc && !seenNPC.Contains(npc) && npc.hidden == 0 && this.npc.hidden == 0)
                    {
                        seenNPC.Add(npc);
                    }
                    else if (!npc)
                    {
                        collisionHit = ray;
                        break;
                    }                   
                }
            }

            if (collisionHit.collider == null)
            {
                vertex = transform.InverseTransformDirection(GetVectorFromAngle(angle) * viewDistance);
            }
            else
            {
                vertex = transform.InverseTransformPoint(collisionHit.point);
            }

            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        if (mesh != null)
        {
            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;
        }     
    }

    private Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    private float GetAngleFromVectorFloat(Vector3 direction)
    {
        direction = direction.normalized;
        float n = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (n < 0)
            n += 360;

        return n;
    }
}
