using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceUI : MonoBehaviour
{
    public GameObject popUp;
    public TextMeshProUGUI resourceQuantityText;
    public Resource resource;

    void OnMouseEnter()
    {
        popUp.SetActive(true);
    }

    void OnMouseExit()
    {
        popUp.SetActive(false);
    }

    public void OnResourceQuantityChange()
    {
        resourceQuantityText.text = resource.quantity.ToString();
    }
}
