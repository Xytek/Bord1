using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlatformBehaviour : NetworkBehaviour
{
    Collider2D collider;
    Rigidbody2D rigidbody;
    private bool canDisable = false;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                gameObject.layer = 9;       // "Going Down"
                Invoke("minWaitForGoingDown", 0.5f);
            }

        }
        else //Not local player
        {
            gameObject.layer = 9;
            Invoke("minWaitForGoingDown", 0.5f);
        }
    }

    void minWaitForGoingDown()
    {
        canDisable = true;
        gameObject.layer = 0;       // "Default"
    }

    /*
    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 validDirection = Vector3.up;  // What you consider to be upwards
        float contactThreshold = 30;          // Acceptable difference in degrees

        if (collision.gameObject.tag == "Platform")
        {
            for (int k = 0; k < collision.contacts.Length; k++)
            {
                // Colliding with a surface facing mostly upwards?
                if (Vector3.Angle(collision.contacts[k].normal, validDirection) >= contactThreshold)
                {
                    Debug.Log("enter");
                    // make collider trigger for a bit so he can pass through
                    rigidbody.isKinematic = true;
                    break;
                }
            }
        }
    }
    */
    /*
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("yes");
        if(collision.gameObject.tag == "Platform" && canDisable)
        {
            Debug.Log("Going down");
            gameObject.layer = 0;
            canDisable = false;
        }
    }
    */
}
