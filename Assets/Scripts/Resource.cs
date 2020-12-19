using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ResourceType
{
    wood
}

public class Resource : MonoBehaviour
{
    public ResourceType type;
    public int quantity;

    public UnityEvent onQuantityChange;

    public void GatherResource (int amount, GameObject unit)
    {
        quantity -= amount;

        int amountToGive = amount;
        if (quantity < 0)
        {
            amountToGive = amount + quantity;
        }
        (unit.GetComponent(typeof(UnitMovement)) as UnitMovement).AddResource(amountToGive);

        if (quantity <= 0)
        {
            Destroy(gameObject);
        }
        if (onQuantityChange != null)
        {
            onQuantityChange.Invoke();
        }
    }
   
}
