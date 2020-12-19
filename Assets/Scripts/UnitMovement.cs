using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    private Vector3 movePosition;

    private int resourceAmount;

    private float time = 0f;

    private float pickUpTime = 2f;

    void Awake()
    {
        this.movePosition = transform.position;
    }

    public void SetMovePosition(Vector3 movePosition)
    {
        this.movePosition = movePosition;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDir = (movePosition - transform.position);
       // Debug.Log(moveDir);
        GetComponent<IMoveVelocity>().SetVelocity(moveDir);
    }

    public void AddResource(int amount)
    {
        resourceAmount += amount;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Resource")
        {
            Debug.Log("Getting Resource");
            time += Time.deltaTime;
            if (time >= pickUpTime)
            {
                time = 0f;

                (collision.gameObject.GetComponent(typeof(Resource)) as Resource).GatherResource(1, this.gameObject);
            }
        }
    }
}
