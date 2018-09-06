using UnityEngine;

public class PlayerLook : MonoBehaviour {

    [SerializeField] private string mouseXInputName, mouseYInputName;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private Transform playerBody;

    private float xAxisClamp;

    private void Awake()
    {
        LockCursor();
        xAxisClamp = 0.0f;
    }

    private void Update()
    {
        CameraRotation();
    }

    private void LockCursor() //Locking cursor.
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void CameraRotation() //Calculating and applying rotation. / Clamping max-min rotation.
    {
        float mouseX = Input.GetAxis(mouseXInputName) * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis(mouseYInputName) * mouseSensitivity * Time.deltaTime;

        xAxisClamp += mouseY;

        if (xAxisClamp > 90.0f) //Checking and applying clamp.
        {
            xAxisClamp = 90.0f;
            mouseY = 0.0f;

            ClampXAxisRotationToValue(270.0f); //Value "(270.0f)" = X axis rotation clamp. 
        }
        else if (xAxisClamp < -90.0f)
        {
            xAxisClamp = -90.0f;
            mouseY = 0.0f;

            ClampXAxisRotationToValue(90.0f); //Value "(90.0f)" = Y axis rotation clamp. 
        }

        transform.Rotate(Vector3.left * mouseY);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void ClampXAxisRotationToValue(float value)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;
    }
}
