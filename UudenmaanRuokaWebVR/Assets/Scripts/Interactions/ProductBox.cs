using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @Author: Veli-Matti Vuoti
/// 
/// Handles the product box functionality, which is not much.
/// </summary>
public class ProductBox : MonoBehaviour
{
    public Product _product;
    public GameObject uiElementPrefab;
    public GameObject uiElement;

    public void AutoInit(Product product)
    {
        gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", product.pPicture);

        InitUIElement(product);
    }

    public void InitLocal()
    {
        if (_product == null)
        {
            Debug.LogWarning("Failed to initialize locally! -Product is null please setup product");
            return;
        }
        gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", _product.pPicture);
        InitUIElement(_product);
    }

    public void InitUIElement(Product product)
    {
        uiElement = Instantiate(uiElementPrefab, transform.position + Vector3.Scale(new Vector3(0.3f,0.3f,0.3f),transform.forward), transform.rotation);
        SetText(uiElement, product.pName, product.pDescription, product.pPrice);
    }

    public void SetText(GameObject ui, string header, string description, float price)
    {
        ui.GetComponent<ProductUI>().UpdateText(header, description, price);
        ui.SetActive(false);
    }

    public void ShowUIElement()
    {
        //Debug.Log("SHOW UI ELEMENT");
        if(!uiElement.activeSelf)
            uiElement.SetActive(true);
    }

    public void HideUIElement()
    {
        //Debug.Log("HIDE UI ELEMENT");
        uiElement.SetActive(false);
    }
}
