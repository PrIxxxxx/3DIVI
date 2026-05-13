using TMPro;
using UnityEngine;

public class AmmoStack : MonoBehaviour
{
    public int amount = 10;
    public TMP_Text amountText;

    void Start()
    {
        UpdateText();
    }

    public void UseOne()
    {
        amount--;
        UpdateText();

        if (amount <= 0)
        {
            Destroy(gameObject);
        }
    }

    void UpdateText()
    {
        if (amountText != null)
            amountText.text = amount.ToString();
    }
}