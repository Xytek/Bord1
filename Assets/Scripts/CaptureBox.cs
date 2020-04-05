using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class CaptureBox : NetworkBehaviour
{
    Collider2D collider;
    Slider slider;
    GameObject sliderGO;

    [SerializeField]
    public const float TIME_TO_CAPTURE = 2.0f;

    [SerializeField]
    private Vector3Int tilePosition;
    [SerializeField]
    private Tilemap tilemap;
    private float timeLeftToCapture = TIME_TO_CAPTURE;

    // who has captured this room
    [SyncVar]
    public int capturedBy = (int) PLAYERS.NONE;

    // who is currently capturing it
    [SyncVar]
    private int playerCapturing = (int)PLAYERS.NONE;

    //[SyncVar]
    //private SyncListInt _playersInside = new SyncListInt();
    [SyncVar]
    private SyncListInt _playersInside = new SyncListInt();

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();
        sliderGO = transform.Find("Canvas").Find("Slider").gameObject;
        slider = sliderGO.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        // no players inside, do nothing, really
        if (_playersInside.Count == 0 || (_playersInside.Count == 1 && _playersInside[0] == capturedBy))
        {
            timeLeftToCapture = TIME_TO_CAPTURE;
            sliderGO.SetActive(false);
            slider.value = 0;
        }

        // if only one player is inside
        else if (_playersInside.Count == 1)
        {
            // another player has started capturing it
            if(playerCapturing != _playersInside[0])
            {
                timeLeftToCapture = TIME_TO_CAPTURE;
                slider.value = 0;
            }

            // if opponent is in room
            if (_playersInside[0] != capturedBy)
            {
                playerCapturing = _playersInside[0];
                timeLeftToCapture -= Time.deltaTime;
                sliderGO.SetActive(true);
                slider.value = (1.0f / TIME_TO_CAPTURE) * (TIME_TO_CAPTURE - timeLeftToCapture);                

                // opponent has successfully captured
                if (timeLeftToCapture <= 0)
                {
                    //sliderGO.SetActive(false);
                    //slider.value = 0;
                    captureRoom((PLAYERS)_playersInside[0]);
                }
            }
        }
    }

    void captureRoom(PLAYERS capturer)
    {
        Vector3Int tilePos = tilePosition;
        tilemap.SetTileFlags(tilePos, TileFlags.None);

        switch (capturer)
        {
            case PLAYERS.RED:
                tilemap.SetColor(tilePos, Color.red);
                capturedBy = (int)PLAYERS.RED;
                break;
            case PLAYERS.YELLOW:
                tilemap.SetColor(tilePos, Color.yellow);
                capturedBy = (int)PLAYERS.YELLOW;
                break;
            case PLAYERS.BLUE:
                tilemap.SetColor(tilePos, Color.blue);
                capturedBy = (int)PLAYERS.BLUE;
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _playersInside.Add((int) other.GetComponent<Player>().playerColor);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _playersInside.Remove((int) other.GetComponent<Player>().playerColor);
        }
    }

    /*
    PLAYERS[] GetPlayersInside()
    {
        Vector2 center = collider.transform.position;
        Vector2 radius = new Vector2(transform.localScale.x / 2, transform.localScale.y / 2);
        List<PLAYERS> playersInside = new List<PLAYERS>();

        Collider2D[] getObjectsInside = Physics2D.OverlapBoxAll(center, radius, 0);

        for (int i = 0; i < getObjectsInside.Length; i++)
        {
            if (getObjectsInside[i].tag == "Player")
            {
                playersInside.Add(getObjectsInside[i].GetComponent<Player>().playerColor);
            }
        }

        return playersInside.ToArray();
    }
    */

    /*
    private void OnTriggerStay2D(Collider2D other)
    {
        Vector2 center = collider.transform.position;
        Vector2 radius = new Vector2(transform.localScale.x / 2, transform.localScale.y / 2);
        List<PLAYERS> playersInside = new List<PLAYERS>();

        //Collider2D[] getObjectsInside = Physics2D.OverlapBoxAll(center, radius, 0);

        for (int i = 0; i < getObjectsInside.Length; i++)
        {
            if (getObjectsInside[i].tag == "Player")
            {
                playersInside.Add(getObjectsInside[i].GetComponent<Player>().playerColor);
            }
        }

        _playersInside = playersInside.ToArray();
    }
    */
}
