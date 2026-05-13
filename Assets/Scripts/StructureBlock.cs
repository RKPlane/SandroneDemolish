using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class StructureBlock : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] float density = 2400f;

    [Header("Demolition")]
    [SerializeField] bool  autoThreshold      = true;
    [SerializeField] float demolishThreshold  = 0.5f;
    [SerializeField] float thresholdMultiplier = 0.4f;

    public Rigidbody Rb { get; private set; }

    Vector3 originPos;
    bool demolished;
    float computedThreshold;
    Renderer rend;
    Color originalColor;

    void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        if (rend != null)
            originalColor = rend.material.color;
    }

    void Start()
    {
        originPos = transform.position;

        Vector3 size = GetComponent<Collider>().bounds.size;
        float volume = size.x * size.y * size.z;
        Rb.mass = Mathf.Max(0.1f, volume * density);

        computedThreshold = autoThreshold
            ? Mathf.Max(size.x, size.y, size.z) * thresholdMultiplier
            : demolishThreshold;

        DemolitionTracker.Instance.Register(this);
    }

    void FixedUpdate()
    {
        if (demolished) return;

        float dist = Vector3.Distance(transform.position, originPos);

        if (rend != null)
        {
            float t = Mathf.Clamp01(dist / computedThreshold);
            rend.material.color = Color.Lerp(originalColor, Color.red, t * 0.6f);
        }

        if (dist > computedThreshold)
        {
            demolished = true;
            DemolitionTracker.Instance.OnBlockDemolished();
        }
    }

    void OnDestroy()
    {
        if (!demolished)
            DemolitionTracker.Instance?.Unregister(this);
    }
}
