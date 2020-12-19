using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointTowardsCenter : MonoBehaviour
{

    public TileMapGenerator tilegen;

    Vector3 center;

    Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        center = new Vector3(tilegen.rows / 2, tilegen.cols / 2, 0);
        camera = Camera.main;
        transform.localPosition = new Vector3(5, -4, 1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*
        Vector3 targetDirection = center - transform.position;

        Vector3 newDirection = Vector3.RotateTowards(, targetDirection, Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
        */
        Vector3 moveDirection = gameObject.transform.position - center;
        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
