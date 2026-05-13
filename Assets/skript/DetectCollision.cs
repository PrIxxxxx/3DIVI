using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with " + collision.gameObject.name);
    }
    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("Collision detected with " + collision.gameObject.name);
    }
    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Collision detected with " + collision.gameObject.name);
    }
}
