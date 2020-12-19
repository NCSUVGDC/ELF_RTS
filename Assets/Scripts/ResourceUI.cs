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
        Debug.Log("MouseEntered");
        popUp.SetActive(true);
    }

    void OnMouseExit()
    {
        Debug.Log("MouseExited");
        popUp.SetActive(false);
    }

    public void OnResourceQuantityChange()
    {
        resourceQuantityText.text = resource.quantity.ToString();
    }
}
