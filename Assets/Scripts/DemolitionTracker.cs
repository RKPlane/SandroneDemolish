using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DemolitionTracker : MonoBehaviour
{
    public static DemolitionTracker Instance { get; private set; }

    [Range(0f, 1f)]
    [SerializeField] float winThreshold = 0.8f;

    [SerializeField] float winDelay = 2f;

    int totalBlocks;
    int demolishedBlocks;
    bool winPending;

    readonly HashSet<StructureBlock> registered = new();

    public float DemolitionPercent
    {
        get
        {
            if (totalBlocks > 0)
                return (float)demolishedBlocks / totalBlocks;
            else
                return 0f;
        }
    }

    public int TotalBlocks => totalBlocks;
    public int DemolishedBlocks => demolishedBlocks;

    void Awake()
    {
        Instance = this;
    }

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
        GameManager.Instance.OnDemolitionUpdated();

        if (!winPending && DemolitionPercent >= winThreshold)
        {
            winPending = true;
            StartCoroutine(WinAfterDelay());
        }
    }

    IEnumerator WinAfterDelay()
    {
        yield return new WaitForSeconds(winDelay);
        if (GameManager.Instance.CurrentState == GameManager.State.Playing)
            GameManager.Instance.OnDemolitionComplete();
    }
}
