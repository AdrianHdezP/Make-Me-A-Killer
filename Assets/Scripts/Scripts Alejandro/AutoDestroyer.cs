using UnityEngine;

public class AutoDestroyer : MonoBehaviour
{
    [SerializeField] float t;

    private void Start()
    {
        if (t != 0) Destroy(gameObject, t);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
