using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// @Author: Veli-Matti Vuoti
/// 
/// Updates the text on product ui popup.
/// </summary>
public class ProductUI : MonoBehaviour
{
    public TextMeshProUGUI pName;
    public TextMeshProUGUI pDescription;
    public TextMeshProUGUI pPrice;

    public void UpdateText(string header, string description, float price)
    {
        pName.text = header;
        pDescription.text = description;
        pPrice.text = price.ToString() + " €";
    }
}
