using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Sword : NetworkBehaviour
{
    private Animator anim;
    private float damage = 100f;
    // Update is called once per frame
    void Start()
    {
        anim = transform.root.GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Player enemy = collision.transform.GetComponent<Player>();
        if (anim.GetBool("isStriking"))
            if (enemy != null)
                enemy.DealDamage(damage);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player enemy = collision.transform.GetComponent<Player>();
        if (anim.GetBool("isStriking"))
            if (enemy != null)
                enemy.DealDamage(damage);
    }
}

