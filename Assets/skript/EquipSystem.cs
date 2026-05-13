using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipSystem : MonoBehaviour
{
    public static EquipSystem Instance { get; set; }

    // -- UI -- //
    public GameObject quickSlotsPanel;

    public List<GameObject> quickSlotsList = new List<GameObject>();
    public List<string> itemList = new List<string>();
    


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    private void Start()
    {
        PopulateSlotList();
    }


    public Transform weaponHolder;
    private GameObject currentWeapon;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) EquipFromSlot(0);

        CheckEquippedItemStillInQuickSlots();
    }

    void CheckEquippedItemStillInQuickSlots()
    {
        if (currentWeapon == null) return;

        bool itemStillInQuickSlots = false;

        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount > 0)
            {
                GameObject item = slot.transform.GetChild(0).gameObject;
                string cleanName = item.name.Replace("(Clone)", "");

                if (currentWeapon.name.Contains(cleanName))
                {
                    itemStillInQuickSlots = true;
                    break;
                }
            }
        }

        if (!itemStillInQuickSlots)
        {
            Destroy(currentWeapon);
            currentWeapon = null;
        }
    }

    void EquipFromSlot(int slotIndex)
    {
        if (slotIndex >= quickSlotsList.Count) return;
        if (quickSlotsList[slotIndex].transform.childCount == 0) return;

        GameObject uiItem = quickSlotsList[slotIndex].transform.GetChild(0).gameObject;

        string cleanName = uiItem.name.Replace("(Clone)", "");
        string handPrefabName = cleanName + "_Hand";

        GameObject prefab = Resources.Load<GameObject>(handPrefabName);

        if (prefab == null)
        {
            Debug.LogError("Missing hand prefab in Resources: " + handPrefabName);
            return;
        }

        if (currentWeapon != null)
            Destroy(currentWeapon);

        currentWeapon = Instantiate(prefab, weaponHolder);
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.identity;
        currentWeapon.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    private void PopulateSlotList()
    {
        foreach (Transform child in quickSlotsPanel.transform)
        {
            if (child.CompareTag("QuickSlot"))
            {
                quickSlotsList.Add(child.gameObject);
            }
        }
    }

    public void AddToQuickSlots(GameObject itemToEquip)
    {
        // Find next free slot
        GameObject availableSlot = FindNextEmptySlot();
        // Set transform of our object
        itemToEquip.transform.SetParent(availableSlot.transform, false);
        // Getting clean name
        string cleanName = itemToEquip.name.Replace("(Clone)", "");
        // Adding item to list
        itemList.Add(cleanName);

        //InventorySystem.Instance.ReCalculateList();

    }


    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }

    public bool CheckIfFull()
    {

        int counter = 0;

        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount > 0)
            {
                counter += 1;
            }
        }

        if (counter == 7)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}
