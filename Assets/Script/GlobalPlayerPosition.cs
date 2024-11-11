using UnityEngine;

public class GlobalPlayerPosition : MonoBehaviour
{
    public static GlobalPlayerPosition Instance { get; private set; }
    private Rigidbody rb;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        // Get the Rigidbody component for velocity tracking
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component missing from GlobalPlayerPosition object.");
        }
    }

    public Vector3 GetPlayerPosition()
    {
        return transform.position;
    }

    public Transform GetPlayerTransform()
    {
        return transform;
    }

    // Method to return the magnitude of the velocity
    public float GetVelocityMagnitude()
    {
        return GetComponent<Rigidbody>().velocity.magnitude;
    }

    public float GetMaxSpeed()
    {
        return GetComponent<TankMovement>().maxSpeed;
    }

    public float GetSpeedRatio()
    {
        return GetVelocityMagnitude() / GetMaxSpeed();
    }
}
