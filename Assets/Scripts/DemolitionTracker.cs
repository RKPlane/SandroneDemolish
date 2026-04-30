using UnityEngine;
using System.Collections.Generic;

public class DemolitionTracker : MonoBehaviour
{
    public static DemolitionTracker Instance { get; private set; }

    [Range(0f, 1f)]
    [SerializeField] float winThreshold = 0.8f;

    int totalBlocks;
    int demolishedBlocks;
    readonly HashSet<StructureBlock> registered = new();

    public float DemolitionPercent
    {
        get
        {
            if (totalBlocks > 0)
            {
                return (float)demolishedBlocks / totalBlocks;
            }
            else
            {
                return 0f;
            }
        }
    }

    void Awake() => Instance = this;

    public void Register(StructureBlock block)
    {
        if (registered.Add(block))
            totalBlocks++;
    }

    public void Unregister(StructureBlock block)
    {
        if (registered.Remove(block))
            totalBlocks = Mathf.Max(0, totalBlocks - 1);
    }

    public void OnBlockDemolished()
    {
        demolishedBlocks++;
        if (DemolitionPercent >= winThreshold)
            GameManager.Instance.OnDemolitionComplete();
    }
}
