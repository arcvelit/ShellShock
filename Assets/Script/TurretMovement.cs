using UnityEngine;

public class TurretMovement : MonoBehaviour
{
    public float sensitivity;
    private float currentRotation = 0f;

    public Transform minimap;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        currentRotation += mouseX * sensitivity;

        if (currentRotation > 360) currentRotation -= 360;
        else if (currentRotation < -360) currentRotation += 360;
        
        // Rotate minimap
        minimap.localEulerAngles = new Vector3(90, -currentRotation, 0);

        transform.localEulerAngles = new Vector3(0, currentRotation, 0);
    }
}
