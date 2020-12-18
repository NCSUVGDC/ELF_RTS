using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    private Vector3 movePosition;

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
}
