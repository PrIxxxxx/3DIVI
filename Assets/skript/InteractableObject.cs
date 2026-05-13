using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;


public class InteractableObject : MonoBehaviour
{


    public bool playerRange;
    public string ItemName;
    public bool isAmmo;
    public int ammoAmount = 10;

    public string GetItemName()
    {
        return ItemName;
    }



    void Update()
    {
        if (playerRange && Input.GetKeyDown(KeyCode.E) && SelectionManager.Instance.onTarget)
        {
            if (isAmmo)
            {
                InventorySystem.Instance.AddToInventory(ItemName);
                Destroy(gameObject);
                return;
            }

            if (!InventorySystem.Instance.CheckIfFull())
            {
                Debug.Log("Interacted with " + gameObject.name);
                InventorySystem.Instance.AddToInventory(ItemName);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Inventory is full. Cannot pick up " + gameObject.name);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerRange = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerRange = false;
        }
    }
  
}

