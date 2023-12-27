using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class PlayerController : MonoBehaviour
{
    Vector3 velocity;
    Rigidbody myRigidbody;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    public void LookAt (Vector3 lookPoint)
    {
        Vector3 hightCorrectedPoint = new Vector3 (lookPoint.x, transform.position.y, lookPoint.z);
        transform.LookAt(hightCorrectedPoint);
    }

    void FixedUpdate ()
    {
        myRigidbody.MovePosition(myRigidbody.position + velocity * Time.fixedDeltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(myRigidbody.position);
            Debug.Log(velocity * Time.fixedDeltaTime);
            Debug.Log(myRigidbody.position + velocity * Time.fixedDeltaTime);
        }
    }
}
