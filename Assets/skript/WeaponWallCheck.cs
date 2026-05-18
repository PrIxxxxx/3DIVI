using UnityEngine;

public class WeaponWallCheck : MonoBehaviour
{
    public Transform weaponHolder;
    public Camera playerCamera;

    public Vector3 normalPos = new Vector3(0.3f, -0.25f, 0.5f);
    public Vector3 blockedPos = new Vector3(0.1f, -0.35f, 0.1f);

    public float checkDistance = 1f;
    public float smoothSpeed = 10f;

    void Start()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;
    }

    void Update()
    {
        bool nearWall = Physics.Raycast(
            playerCamera.transform.position,
            playerCamera.transform.forward,
            checkDistance
        );

        Vector3 targetPos = nearWall ? blockedPos : normalPos;

        weaponHolder.localPosition = Vector3.Lerp(
            weaponHolder.localPosition,
            targetPos,
            Time.deltaTime * smoothSpeed
        );
    }
}