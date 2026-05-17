using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public string requiredKey = "key";
    public Vector3 openOffset = new Vector3(0, 3f, 0);
    public float openSpeed = 3f;

    private Vector3 closedPos;
    private bool isOpen;

    void Start()
    {
        closedPos = transform.position;
    }

    void OnMouseDown()
    {
        if (isOpen) return;

        if (InventorySystem.Instance.HasItem(requiredKey))
        {
            StartCoroutine(OpenDoor());
        }
        else
        {
            Debug.Log("You need a key!");
        }
    }

    IEnumerator OpenDoor()
    {
        isOpen = true;

        Vector3 openPos = closedPos + openOffset;

        while (Vector3.Distance(transform.position, openPos) > 0.01f)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                openPos,
                Time.deltaTime * openSpeed
            );

            yield return null;
        }

        transform.position = openPos;
    }
}