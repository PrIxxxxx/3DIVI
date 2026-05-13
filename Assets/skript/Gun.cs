using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletTrailPrefab;
    public Camera playerCamera;
    public float damage = 25f;
    public float range = 100f;
    public float fireRate = 0.25f;

    private float nextFireTime;

    void Start()
    {
        playerCamera = Camera.main;

        if (playerCamera == null)
        {
            Debug.LogError("No Main Camera found! Make sure your camera tag is MainCamera.");
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            if (!InventorySystem.Instance.UseOneAmmo())
            {
                Debug.Log("No ammo!");
                return;
            }

            Shoot();
            nextFireTime = Time.time + fireRate;

            Debug.Log("Ammo left: " + InventorySystem.Instance.pistolAmmo);
        }
    }

    void Shoot()
    {

        GameObject trail = Instantiate(bulletTrailPrefab, firePoint.position, Quaternion.identity);
        LineRenderer line = trail.GetComponent<LineRenderer>();

        Vector3 endPoint = playerCamera.transform.position + playerCamera.transform.forward * range;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, range))
        {
            endPoint = hit.point;

            EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        line.SetPosition(0, firePoint.position);
        line.SetPosition(1, endPoint);

        Destroy(trail, 0.05f);
    }
    }