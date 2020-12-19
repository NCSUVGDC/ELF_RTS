using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    private Vector3 movePosition;

    private int resourceAmount;

    private float time;

    private float pickUpTime = 2f;

    private float dropOffTime = 2f;

    void Awake()
    {
        this.movePosition = transform.position;
        this.time = 0f;
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
        if (GameManager.Instance().isPaused())
        {
            Time.timeScale = 0f;
        }
        else if (GameManager.Instance().isFastForward())
        {
            Time.timeScale = 2f;
        }
        else
        {
            Time.timeScale = 1f;
        }

        if (collision.gameObject.tag == "Resource")
        {
            //GameManager.fastForward()
            time += Time.fixedDeltaTime;
            //Debug.Log(Time.deltaTime);
            if (time >= pickUpTime)
            {
                time = time - pickUpTime;

                (collision.gameObject.GetComponent(typeof(Resource)) as Resource).GatherResource(1, this.gameObject);
            }
        }

        if (collision.gameObject.tag == "Builder")
        {
            //Debug.Log("Giving Resources");
            time += Time.fixedDeltaTime;
            if (time >= dropOffTime)
            {
                Debug.Log(time);
                time = time - dropOffTime;
                if (resourceAmount > 0)
                {
                    (collision.gameObject.GetComponent(typeof(Resource)) as Resource).GiveResource(1);
                    resourceAmount--;
                }
            }
        }
    }
}
