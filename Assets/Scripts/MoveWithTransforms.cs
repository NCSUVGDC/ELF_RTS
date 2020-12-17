using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithTransforms : MonoBehaviour, IMoveVelocity
{
    public float moveSpeed;

    private Vector3 velocityVector;
    
    // Start is called before the first frame update
    void Awake()
    {
        velocityVector = new Vector3(0, 0, 0);
    }

    public void SetVelocity(Vector3 velocityVector)
    {
        this.velocityVector = velocityVector;
    }

    // Update is called once per frame
    private void Update()
    {
        //Debug.Log(velocityVector);
        transform.position += velocityVector * moveSpeed * Time.deltaTime;

    }
}
