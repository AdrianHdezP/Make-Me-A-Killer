using UnityEngine;

public class NPCView : MonoBehaviour
{
    private NPC npc;

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject NPCCollider;

    private Mesh mesh;
    private Vector3 origin;
    private float startingAngle;
    private float fov;



    private void Start()
    {
        npc = GetComponentInParent<NPC>();
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
<<<<<<< Updated upstream
        origin = Vector3.zero;
        fov = 90f;
=======
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
            if (npc.type == NPCType.red && seenNPC[i].type == NPCType.blue && seenNPC[i].dead)
            {
                npc.bodyTarget = seenNPC[i].transform.position;
                npc.examinate = true;
                npc.InyectedState(npc.examinateBodyState);
            }

            // Si yo soy normal y estoy viendo al azul muerto y aun no lo estoy examinando
            if (npc.type == NPCType.normal && seenNPC[i].dead && !npc.examinate)
            {
                npc.bodyTarget = seenNPC[i].transform.position;
                npc.examinate = true;
                npc.InyectedState(npc.examinateBodyState);
            }

            // Si yo soy normal y estoy viendo al azul muerto y si lo estoy examinando
            if (npc.type == NPCType.normal && seenNPC[i].dead && npc.examinate)
            {
                Debug.Log("Corre");

                if (blueVisibleAndDead && redVisible && !someoneElse)
                {
                    Debug.Log("Ganas");
                }
                else
                {
                    Debug.Log("Pierdes");
                }
            }
        }
>>>>>>> Stashed changes
    }

    private void Update()
    {   
<<<<<<< Updated upstream
=======
        if (npc.dead)
            return;

        seenNPC.Clear();
>>>>>>> Stashed changes
        int rayCount = 50;
        float angle = startingAngle;
        float angleIncrease = fov / rayCount;
        float viewDistance = 5f;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;
        int vertexIndex = 1;
        int triangleIndex = 0;

        for (int i = 1; i <= rayCount; i++)
        {
            Vector3 vertex;

            RaycastHit2D[] raycastHit2D = Physics2D.RaycastAll(origin, GetVectorFromAngle(angle), viewDistance, layerMask);
            RaycastHit2D myRaycastHit2D = new RaycastHit2D();

            foreach (var ray in raycastHit2D)
            {
                if (ray.collider.gameObject != NPCCollider)
                {
                    myRaycastHit2D = ray;
<<<<<<< Updated upstream
=======
                    
                    if (myRaycastHit2D.collider.gameObject.GetComponent<NPC>() && !seenNPC.Contains(myRaycastHit2D.collider.gameObject.GetComponent<NPC>()))
                        seenNPC.Add(myRaycastHit2D.collider.gameObject.GetComponent<NPC>());

>>>>>>> Stashed changes
                    break;
                }
            }

            if (myRaycastHit2D.collider == null)
            {
                vertex = origin + GetVectorFromAngle(angle) * viewDistance;
            }
            else
            {
                vertex = myRaycastHit2D.point;
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

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    private void SetOrigin(Vector3 origin) => this.origin = origin;

    private void SetAimDirection(Vector3 aimDirection)
    {
        startingAngle = GetAngleFromVectorFloat(aimDirection) - fov / 2f;
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
