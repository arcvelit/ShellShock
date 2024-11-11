using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float followSpeed = 5f;
    public float rotationSmoothTime = 0.2f;

    private Vector3 currentVelocity;

    void LateUpdate()
    {
        Vector3 targetPosition = target.position + target.TransformDirection(offset);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, followSpeed * Time.deltaTime);

        Quaternion targetRotation = Quaternion.LookRotation(-target.forward, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothTime);
    }
}
