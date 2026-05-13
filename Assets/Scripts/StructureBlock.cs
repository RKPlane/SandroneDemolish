using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class StructureBlock : MonoBehaviour
{
    [Header("Fisica y Densidad")]
    [SerializeField] float density = 800f;

    [Header("Demolicion y Umbral de activacion")]
    [SerializeField] bool autoThreshold = true;


    [Header("Multiplicador")]
    [SerializeField] float thresholdMultiplier = 0.4f;

    Rigidbody rb;
    Vector3 originPos;
    bool demolished;
    float computedThreshold;
    Renderer rend;
    Color originalColor;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        if (rend != null)
            originalColor = rend.material.color;
    }

    void Start()
    {
        originPos = transform.position;

        Vector3 size = GetComponent<Collider>().bounds.size;
        float volume = size.x * size.y * size.z;
        rb.mass = Mathf.Max(0.1f, volume * density);

        if (autoThreshold)
        {
            float maxDim = Mathf.Max(size.x, size.y, size.z);
            computedThreshold = maxDim * thresholdMultiplier;
        }
 

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
            DemolitionTracker.Instance.Unregister(this);
    }
}
