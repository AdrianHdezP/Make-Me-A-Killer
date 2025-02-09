using UnityEngine;

public class KillTarget : MonoBehaviour
{
    [SerializeField] private NPC blue;
    [SerializeField] private GameObject killTarget;
    [SerializeField] private GameObject alreadyKillTarget;

    private void Update()
    {
        if (blue.dead && killTarget.activeSelf == true)
        {
            killTarget.SetActive(false);
            alreadyKillTarget.SetActive(true);
        }
    }
}
