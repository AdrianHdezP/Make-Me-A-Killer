using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetsHolder : MonoBehaviour
{
    public List<Targets> targets;
    public List<NPC> npc;

    private void Awake()
    {
        targets = FindObjectsByType<Targets>(FindObjectsSortMode.None).ToList();
        npc = FindObjectsByType<NPC>(FindObjectsSortMode.None).ToList();

        AssignTargets();
    }

    private void AssignTargets()
    {
        for (int i = 0; i < npc.Count; i++)
        {
            Targets randomTargets = targets[Random.Range(0, targets.Count)];
            npc[i].targets = randomTargets;
        }
    }
}
