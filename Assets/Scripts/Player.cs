using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public enum PLAYERS
{
    RED,
    YELLOW,
    BLUE,
    NONE
}

public class Player : NetworkBehaviour
{
    public Transform[] weapons;
    public PLAYERS playerColor;

    private float health = 100;
    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField]
    AudioSource SwordSwingSound = null;
    private bool IsSwordSwung = false;

    [SerializeField]
    AudioSource DrawGun = null;

    [SerializeField]
    AudioSource DrawSword = null;

    private bool networkSwingOnce = false;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
            UpdateWeapon();
        float moveInput = Input.GetAxis("Horizontal");

        if (moveInput != 0)
        {
            anim.SetBool("isIdle", false);
        }
        else
        {
            anim.SetBool("isIdle", true);
        }

        if (Input.GetMouseButton(0) && weapons[0].gameObject.activeSelf)
        {

            //Play sound
            if (!IsSwordSwung && isLocalPlayer)
            {
                anim.SetBool("isStriking", true);
                anim.SetTrigger("slash");
                SwordSwingSound.Play();
                IsSwordSwung = true;
            }

        }



        if (!SwordSwingSound.isPlaying)
        {
            anim.SetBool("isStriking", false);
            IsSwordSwung = false;
        }




        if (!isLocalPlayer && GetComponent<NetworkAnimator>().param4.Contains("True"))
        {
            if (!networkSwingOnce)
            {
                GetComponent<NetworkAnimator>().animator.SetBool("isStriking", true);
                GetComponent<NetworkAnimator>().animator.SetTrigger("slash");
                networkSwingOnce = true;
            }
        }
        else if (!isLocalPlayer && GetComponent<NetworkAnimator>().param4.Contains("False"))
        {
            networkSwingOnce = false;
        }


    }


    public void DealDamage(float dmg)
    {
        health -= dmg;
        
        if (health <= 0 && !transform.GetComponent<PlayerMovement>().isDead)
        {
            anim.SetTrigger("die");
           StartCoroutine( changeLocation());
            transform.GetComponent<PlayerMovement>().isDead = true;
            rb.AddForce(Vector2.up*5);
        }
    }

    private IEnumerator changeLocation()
    {
            GameObject spawn = GameObject.Find("Spawns");
        yield return new WaitForSeconds(0.5f);
        this.gameObject.transform.position = spawn.transform.position;
        transform.GetComponent<PlayerMovement>().isDead = false;
    }

    void UpdateWeapon()
    {
        for (int i = 0; i < weapons.Length; i++)
            if (Input.GetKeyDown(KeyCode.Alpha0 + 1 + i))
            {
                for (int j = 0; j < weapons.Length; j++)
                    if (j != i)
                        weapons[j].gameObject.SetActive(false);

                bool isActive = weapons[i].gameObject.activeSelf;
                switch (i)
                {
                    case 0:
                        anim.SetBool("gotSword", !isActive);
                        anim.SetBool("gotGun", false);
                        DrawSword.Play();
                        break;
                    case 1:
                        anim.SetBool("gotGun", !isActive);
                        anim.SetBool("gotSword", false);

                        DrawGun.Play();
                        break;
                    default:
                        break;
                }
            }
    }

    public bool getIsLocalPlayer()
    {
        return isLocalPlayer;
    }

}



