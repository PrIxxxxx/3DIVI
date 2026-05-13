using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; set; }

    public GameObject interaction_Info_UI;

    public bool onTarget;
    TMP_Text interaction_text;

    private void Start()
    {
        onTarget = false;
        interaction_text = interaction_Info_UI.GetComponent<TMP_Text>();
    }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    void Update()
    {
        //interaction_Info_UI.SetActive(false);
        Ray ray = new Ray(Camera.main.transform.position,
                  Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Player")) return;
            var selectionTransform = hit.transform;
            if (selectionTransform.GetComponent<InteractableObject>() && selectionTransform.GetComponent<InteractableObject>().playerRange)
            {
                interaction_text.text = selectionTransform.GetComponent<InteractableObject>().GetItemName();


                if (!interaction_Info_UI.activeSelf) //izlaiš vienu fraim lai camera neaizlektu prom kad pirmo reizi parāda ui
                {
                    MouseMovement.freezeMouseForFrame = true;
                    //Debug.Log("frozen");
                    onTarget=true;
                    interaction_Info_UI.SetActive(true);
                }

            }
            else
            {
                onTarget = false;
                interaction_Info_UI.SetActive(false);
            }
        }
    }

}
