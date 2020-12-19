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
        if (GameManager.Instance().isPaused())
        {
            Time.timeScale = 0f;
        } else if (GameManager.Instance().isFastForward())
        {
            Time.timeScale = 2f;
        } else
        {
            Time.timeScale = 1f;
        }
        transform.position += velocityVector * moveSpeed * Time.deltaTime;

    }
}
