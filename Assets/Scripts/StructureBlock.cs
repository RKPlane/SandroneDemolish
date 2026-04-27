using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class StructureBlock : MonoBehaviour
{
    [SerializeField] float demolishThreshold = 0.5f;

    Vector3 originPos;
    bool demolished;

    void Start()
    {
        originPos = transform.position;
        DemolitionTracker.Instance.Register(this);
    }

    void FixedUpdate()
    {
        if (!demolished && Vector3.Distance(transform.position, originPos) > demolishThreshold)
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
