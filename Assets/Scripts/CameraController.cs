using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float MinTargetDistance = 3f;
    public float MaxTargetDistance = 30f;

    public float RotationSpeed = 0.25f;
    public float ScrollSpeed = 1f;
    public float VerticalAngleDeg = 30f;

    private float Distance = 10f;
    private float AngleRad = 0f;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position += transform.forward * Input.mouseScrollDelta.y;

        if (Input.GetMouseButton(sail.MouseButton.Middle) == true)
        {
            var pixelOffset = Input.GetAxis("Mouse X") * 40f; // Sorcery
            AngleRad -= Mathf.Deg2Rad * pixelOffset * RotationSpeed;
        }

        // Wheel
        Distance -= Input.mouseScrollDelta.y;
        Distance = Mathf.Clamp(Distance, MinTargetDistance, MaxTargetDistance);

        // Update Position
        var target = GlobalManagers.mapManager.MapCenterPosition;

        var hozizontalAxis = (Vector3.right * Mathf.Sin(AngleRad) + Vector3.back * Mathf.Cos(AngleRad)).normalized;

        var verticalAngleRad = VerticalAngleDeg * Mathf.Deg2Rad;
        var horizontalLength = Mathf.Cos(verticalAngleRad) * Distance;
        var verticalLength = Mathf.Sin(verticalAngleRad) * Distance;

        transform.position = target + horizontalLength * hozizontalAxis + Vector3.up * verticalLength;

        transform.LookAt(target);
    }

    private void OnDrawGizmos()
    {
        if (GlobalManagers.mapManager == null)
        {
            return;
        }
        
        var target = GlobalManagers.mapManager.MapCenterPosition + Vector3.up;
        var cameraPosition = target + (Vector3.right * Mathf.Sin(AngleRad) + Vector3.back * Mathf.Cos(AngleRad)) * 2f;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(target, cameraPosition);
    }
}
