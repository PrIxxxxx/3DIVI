using UnityEngine;
using System.Collections;
public class Gun : MonoBehaviour
{
    public Transform firePoint; // no kurienes lodes vizu?li par?d?s
    public GameObject bulletTrailPrefab; //
    public Camera playerCamera;
    public float damage = 25f;
    public float range = 100f;
    public float fireRate = 0.25f;

    public int currentAmmo = 0;
    public int magSize = 10;
    public float reloadTime = 1.5f;

    private Vector3 startPos;
    public Vector3 reloadOffset = new Vector3(0, -0.2f, 0);
    public float reloadMoveSpeed = 6f;

    private bool isReloading = false;

    private float nextFireTime;

    void Start()
    {
        playerCamera = Camera.main;

        if (playerCamera == null)
        {
            Debug.LogError("No Main Camera found! Make sure your camera tag is MainCamera.");
        }
        startPos = transform.localPosition;
    }
    void Update()
    {
        if (isReloading) return;

        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            if (currentAmmo <= 0)
            {
                StartCoroutine(Reload());
                return;
            }

            currentAmmo--;

            Shoot();
            nextFireTime = Time.time + fireRate;

            Debug.Log("Gun ammo: " + currentAmmo);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        if (isReloading) yield break;
        if (currentAmmo == magSize) yield break;

        isReloading = true;

        Debug.Log("Reloading...");

        
        yield return StartCoroutine(
            MoveGun(startPos + reloadOffset)
        );

        
        yield return new WaitForSeconds(0.5f);

        
        int loadedAmmo = InventorySystem.Instance.TakeAmmoStack();

        if (loadedAmmo > 0)
        {
            currentAmmo = loadedAmmo;
            Debug.Log("Reloaded: " + currentAmmo);
        }
        else
        {
            Debug.Log("No ammo stacks in inventory!");
        }

        
        yield return StartCoroutine(
            MoveGun(startPos)
        );

        isReloading = false;
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

    IEnumerator MoveGun(Vector3 target)
    {
        while (Vector3.Distance(transform.localPosition, target) > 0.01f)
        {
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                target,
                Time.deltaTime * reloadMoveSpeed
            );

            yield return null;
        }

        transform.localPosition = target;
    }
}