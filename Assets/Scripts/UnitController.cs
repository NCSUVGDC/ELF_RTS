using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;


// Uses code from this: https://www.youtube.com/watch?v=LoKNYlWWeSM&ab_channel=Omnirift
public class UnitController : MonoBehaviour
{

    // Don't name it camera, it overrides Component.camera
    Camera camGirl;

    // Level of the ground that the raycast will intersect if it does not hit an object(ELF)

    //public NavMeshAgent agent;
    public LayerMask groundLayer;
    public LayerMask unitLayer;
    private Vector3 leftClickPos;

    private List<GameObject> selectedObjects;
    // Start is called before the first frame update
    void Awake()
    {
        selectedObjects = new List<GameObject>();
        camGirl = Camera.main;
    }


    // Update is called once per frame 
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //leftClickPos = GetMousePosition();
            RaycastHit2D hit = Physics2D.Raycast(GetMousePosition(), camGirl.transform.forward, 100);
            if (hit)
            {
                GameObject obj = hit.transform.gameObject;
                if (!selectedObjects.Contains(obj) && obj.tag == "Unit")
                {
                    selectedObjects.Add(obj);
                }
            }
        }

        //if (Input.GetMouseButtonUp(0))
        //{
        //    //Collider2D[] colliderArray = Physics2D.OverlapAreaAll(leftClickPos, GetMousePosition());
        //    foreach (Collider2D collide in colliderArray)
        //    {
        //        Debug.Log(collide);
        //        selectedObjects.Add(collide.gameObject);
        //       // collide.gameObject.transform.Translate(1, 1, 1);
        //    }
        //}
        if (Input.GetMouseButtonUp(1))
        {
            foreach(GameObject obj in selectedObjects)
            {
               // NavMeshAgent agent = obj.GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
                
                    //agent.Warp(obj.transform.position);
                    //agent.SetDestination(GetMousePosition());
               UnitMovement uM = obj.GetComponent(typeof(UnitMovement)) as UnitMovement;
               uM.SetMovePosition(GetMousePosition());
                
            }
            selectedObjects.Clear();
        }
    }

    private Vector3 GetMousePosition()
    {
        Vector2 screenPosition = Input.mousePosition;

        Vector3 worldPosition = camGirl.ScreenToWorldPoint(screenPosition);
        worldPosition.z = -2f;

        return worldPosition;

       // RayCastHit position;
       // Physics.Raycast(worldPosition, camGirl.transform.forward, out position, 100, groundLayer);

       // return position.point;
    }
}
