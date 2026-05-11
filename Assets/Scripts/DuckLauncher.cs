using UnityEngine;
using UnityEngine.InputSystem;

public class DuckLauncher : MonoBehaviour
{
    [Header("Pato")]
    [SerializeField] GameObject duckPrefab;
    [SerializeField] Transform launchPoint;

    [Header("Lanzamiento")]
    [SerializeField] float launchForce = 25f;
    [SerializeField] int duckCount = 5;

    [Header("Trayectoria")]
    [SerializeField] LineRenderer trajectoryLine;
    [SerializeField] int trajectorySteps = 45;
    [SerializeField] float trajectoryTimeStep = 0.06f;
    [SerializeField] Gradient trajectoryGradient;

    public int DucksRemaining => duckCount;

    public event System.Action<int> OnDuckCountChanged;

    Camera mainCam;

    void Awake()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        if (GameManager.Instance.CurrentState != GameManager.State.Playing) return;

        UpdateTrajectory();

        if (Mouse.current.leftButton.wasPressedThisFrame && duckCount > 0)
            Fire();
    }

    void Fire()
    {
        Vector3 dir = GetAimDirection();

        GameObject duckGO = Instantiate(
            duckPrefab,
            launchPoint.position,
            Quaternion.LookRotation(dir)
        );

        duckGO.GetComponent<Duck>().Launch(dir * launchForce);

        duckCount--;

        if (duckCount <= 0)
        {
            if (trajectoryLine != null)
                trajectoryLine.positionCount = 0;

            GameManager.Instance.OnDucksExhausted();
        }
    }

    void UpdateTrajectory()
    {
        if (trajectoryLine == null || duckCount <= 0)
        {
            if (trajectoryLine != null) trajectoryLine.positionCount = 0;
            return;
        }

        Vector3 dir = GetAimDirection();

        float mass = 1f;
        Rigidbody prefabRb = duckPrefab.GetComponent<Rigidbody>();
        if (prefabRb != null) mass = prefabRb.mass;

        Vector3 vel = dir * (launchForce / mass);
        Vector3 pos = launchPoint.position;

        trajectoryLine.positionCount = trajectorySteps;

        for (int i = 0; i < trajectorySteps; i++)
        {
            trajectoryLine.SetPosition(i, pos);
            vel += Physics.gravity * trajectoryTimeStep;
            pos += vel * trajectoryTimeStep;

            if (i > 0 && Physics.Linecast(trajectoryLine.GetPosition(i - 1), pos))
            {
                trajectoryLine.positionCount = i + 1;
                break;
            }
        }

        if (trajectoryGradient != null)
            trajectoryLine.colorGradient = trajectoryGradient;
    }

    Vector3 GetAimDirection()
    {
        Ray ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        float dist = Vector3.Distance(launchPoint.position, mainCam.transform.position) + 10f;
        Vector3 target = ray.GetPoint(dist);
        return (target - launchPoint.position).normalized;
    }
}
