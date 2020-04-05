using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float damage = 100f;
    private GameObject myMaster;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "BulletStopper")
        {
            Debug.Log("Bullet stopped");
            Destroy(this.gameObject);

        }
        if (collision.tag != "Room" && collision.gameObject != myMaster)
        {
        Debug.Log(collision);

            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.DealDamage(damage);
            Destroy(this.gameObject);

            }
        }
    }

    public void setMaster(GameObject master)
    {
        myMaster = master;
    }
}
