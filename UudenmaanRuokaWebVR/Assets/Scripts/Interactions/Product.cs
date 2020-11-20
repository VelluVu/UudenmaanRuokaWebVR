using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Product", menuName ="Products")]
public class Product : ScriptableObject
{
    public Texture2D pPicture;
    public string pName;
    public string pDescription;
    public float pPrice;
}
