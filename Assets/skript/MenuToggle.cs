using UnityEngine;

public class MenuToggle : MonoBehaviour
{
    public GameObject menuPanel;
    private bool isOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleMenu();
        }
    }

    void ToggleMenu()
    {
        isOpen = !isOpen;
        menuPanel.SetActive(isOpen);

        // Optional: pause the game when menu is open
        Time.timeScale = isOpen ? 0f : 1f;

        // Optional: lock/unlock mouse cursor
        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isOpen;
    }
}
