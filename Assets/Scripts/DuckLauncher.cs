using UnityEngine;
using UnityEngine.InputSystem;

public class DuckLauncher : MonoBehaviour
{
    [SerializeField] GameObject duckPrefab;
    [SerializeField] Transform launchPoint;
    [SerializeField] float launchForce = 25f;
    [SerializeField] int duckCount = 5;

    //trajectoria NO TOCAR
    [SerializeField] LineRenderer trajectoryLine;
    [SerializeField] int trajectorySteps = 30;
    [SerializeField] float trajectoryTimeStep = 0.1f;

    Camera mainCam;

    void Awake() => mainCam = Camera.main;

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
        GameObject duck = Instantiate(duckPrefab, launchPoint.position, Quaternion.LookRotation(dir));
        duck.GetComponent<Duck>().Launch(dir * launchForce);

        duckCount--;
        if (duckCount <= 0)
            GameManager.Instance.OnDucksExhausted();
    }

    void UpdateTrajectory()
    {
        if (trajectoryLine == null) return;

        Vector3 dir = GetAimDirection();
        Vector3 vel = dir * launchForce;
        Vector3 pos = launchPoint.position;

        trajectoryLine.positionCount = trajectorySteps;
        for (int i = 0; i < trajectorySteps; i++)
        {
            trajectoryLine.SetPosition(i, pos);
            vel += Physics.gravity * trajectoryTimeStep;
            pos += vel * trajectoryTimeStep;
        }
    }

    Vector3 GetAimDirection()
    {
        // RAYCAST DEL DISPARO
        Ray ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        float dist = Vector3.Distance(launchPoint.position, mainCam.transform.position) + 10f;
        Vector3 target = ray.GetPoint(dist);
        return (target - launchPoint.position).normalized;
    }
}
