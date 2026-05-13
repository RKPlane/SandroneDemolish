using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BuildingAnchor : MonoBehaviour
{

    //Bases de los edificios
    [SerializeField] float anchorBreakForce = Mathf.Infinity;
    [SerializeField] float anchorBreakTorque = Mathf.Infinity;

    void Start()
    {
        FixedJoint anchor = gameObject.AddComponent<FixedJoint>();
        anchor.connectedBody = null;
        anchor.breakForce    = anchorBreakForce;
        anchor.breakTorque   = anchorBreakTorque;
    }
}
