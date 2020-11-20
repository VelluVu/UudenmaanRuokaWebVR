using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @Author : Veli-Matti Vuoti
/// 
/// Holds all the boxes and product list. 
/// Uses either product list or boxes use their own manually setup product
/// to initialize product to match the box "content".
/// </summary>
public class ProductHolder : MonoBehaviour
{
    public GameObject[] boxes;
    public Product[] products;
    public bool autoSetTextures = true;

    private void Start()
    {
        if (autoSetTextures)
        {
            for (int i = 0; i < boxes.Length; i++)
            {
                if (products[i] == null)
                    return;

                boxes[i].GetComponent<ProductBox>().AutoInit(products[i]);

            }
        }
        if(!autoSetTextures)
        {
            for (int i = 0; i < boxes.Length; i++)
            {
                boxes[i].GetComponent<ProductBox>().InitLocal();              
            }
        }
    }
}
