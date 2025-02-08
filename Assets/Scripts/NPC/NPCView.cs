using System.Collections.Generic;
using UnityEngine;

public class NPCView : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject NPCCollider;

    private Mesh mesh;
    private float fov;
    private List<NPC> seenNPC = new List<NPC>();

    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        fov = 90f;
    }

    private void LateUpdate()
    {   
        if (NPCCollider.GetComponent<NPC>().dead)
            return;

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
            RaycastHit2D myRaycastHit2D = new RaycastHit2D();
            int raycastIndex = 0;

            foreach (var ray in raycastHit2D)
            {
                if (ray.collider.gameObject != NPCCollider)
                {
                    myRaycastHit2D = ray;
                    
                    if (myRaycastHit2D.collider.gameObject.GetComponent<NPC>())
                        seenNPC.Add(myRaycastHit2D.collider.gameObject.GetComponent<NPC>());

                    break;
                }
            }

            if (myRaycastHit2D.collider == null)
            {
                vertex = transform.InverseTransformDirection(GetVectorFromAngle(angle) * viewDistance);
            }
            else
            {
                vertex = transform.InverseTransformPoint(myRaycastHit2D.point);
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
