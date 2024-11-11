using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public Transform hull;
    public Transform minimap;

    public float maxSpeed = 5f;
    public float acceleration = 5f;
    public float deceleration = 5f;
    public float turnSpeed = 50f;

    public float currentSpeed = 0f;

    void Update()
    {
        // Forward and backward movement
        if (Input.GetKey(KeyCode.W))
        {
            currentSpeed += acceleration * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            currentSpeed -= acceleration * Time.deltaTime;
        }
        else
        {
            // Gradually decelerate to zero when no input
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.deltaTime);
        }

        // Clamp speed to max speed limits
        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);

        // Turning (left/right)
        float turnDirection = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            turnDirection = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            turnDirection = 1f;
        }

        // Apply movement and rotation
        transform.Translate(-hull.forward * currentSpeed * Time.deltaTime);
        
        Vector3 minimapPos = transform.position;
        minimapPos.y = 20;
        minimap.position = minimapPos;

        hull.Rotate(Vector3.up, turnDirection * turnSpeed * Time.deltaTime);
    }
}
