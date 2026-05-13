using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public Transform playerBody;  // assign the Player object here
    public Camera playerCamera;   // assign the Camera here
    public float mouseSensitivity = 100f;

    public static bool freezeMouseForFrame;

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        xRotation = playerCamera.transform.localEulerAngles.x;
        if (xRotation > 180f) xRotation -= 360f;
    }

    void Update()
    {
        if (InventorySystem.Instance.isOpen == false)
        {

            if (freezeMouseForFrame)
            {
                freezeMouseForFrame = false;
                return;
            }

            float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}
