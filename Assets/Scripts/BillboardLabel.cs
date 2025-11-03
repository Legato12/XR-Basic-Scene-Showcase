// BillboardLabel.cs — rotate label to face player camera each frame
using UnityEngine;

[ExecuteAlways]
public class BillboardLabel : MonoBehaviour
{
    public Camera targetCamera;
    void LateUpdate()
    {
        var cam = targetCamera ? targetCamera : Camera.main;
        if (!cam) return;
        Vector3 dir = transform.position - cam.transform.position;
        if (dir.sqrMagnitude < 0.0001f) return;
        transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }
}
