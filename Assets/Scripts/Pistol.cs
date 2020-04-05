using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Pistol : NetworkBehaviour
{
    public GameObject projectile;
    public ParticleSystem shotParticles;
    public float bulletForce = 50f;
    private Transform shotPoint;
    private float timeSinceShot;
    private float reload = 0.5f;

    [SerializeField]
    AudioSource gunShot = null;

    private bool _isLocalPlayer;

    private void Start()
    {
        _isLocalPlayer = transform.root.GetComponent<Player>().getIsLocalPlayer();
        shotPoint = shotParticles.transform;
    }
    // Update is called once per frame
    void Update()
    {
   

        Debug.Log(_isLocalPlayer);

        if (!_isLocalPlayer)
        {
            return;
        }

        if (timeSinceShot <= 0)
        {

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 direction = worldMousePos - transform.position;
                direction.Normalize();

                Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, rotZ);

                GameObject bullet = Instantiate(projectile, shotPoint.position, transform.rotation);
                bullet.GetComponent<Bullet>().setMaster(this.transform.root.gameObject);
                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
                Debug.Log(bullet.transform.up);
                bulletRb.velocity = direction *bulletForce ;
                Destroy(bullet, 1f);
                shotParticles.Play();
                timeSinceShot = reload;
                gunShot.Play();
            }
        }
        else
        {
            timeSinceShot -= Time.deltaTime;
        }


    }

}
