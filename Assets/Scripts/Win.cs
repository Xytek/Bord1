using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Win : NetworkBehaviour
{
    List<CaptureBox> captureBoxes = new List<CaptureBox>();

    [SyncVar]
    private int playerWon = 0;

    // Start is called before the first frame update
    void Start()
    {
        // find all "CaptureBox" scripts in the rooms
        GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
        foreach (var room in rooms)
        {
            captureBoxes.Add(room.GetComponent<CaptureBox>());
        }
    }

    public void decideWhoWon()
    {
        // the count of all total tiles a player owns
        int[] playerTilesCount = { 0, 0, 0 };

        // check if all boxes are captured by one player
        foreach (var captureBox in captureBoxes)
        {
            BoxCollider2D collider = captureBox.GetComponent<BoxCollider2D>();
            int roomValue = (int)(collider.size.x * collider.size.y);
            int capturedBy = captureBox.capturedBy;

            playerTilesCount[capturedBy] += roomValue;
        }

        // the player with most tiles/points win
        for(int i = 0; i < playerTilesCount.Length; i++)
        {
            if(playerTilesCount[i] > playerTilesCount[playerWon])
            {
                playerWon = i;
            }
        }

        Text text = GetComponent<Text>();

        switch ((PLAYERS)playerWon)
        {
            case PLAYERS.RED:
                text.text = "Player RED Won!";
                break;
            case PLAYERS.YELLOW:
                text.text = "Player YELLOW Won!";
                break;
            case PLAYERS.BLUE:
                text.text = "Player BLUE Won!";
                break;
            default:
                break;
        }
    }

    /*
    // Update is called once per frame
    void Update()
    {
        // the player we'll think will win
        int winningPlayer = (int)captureBoxes[0].capturedBy;

        // check if all boxes are captured by one player
        foreach (var captureBox in captureBoxes)
        {
            // if there is any box that is not captured by this player, nobody won yet
            if (captureBox.capturedBy != winningPlayer)
            {
                return;
            }
        }

        playerWon = winningPlayer;

        if (playerWon != (int)PLAYERS.NONE)
        {
            Text text = GetComponent<Text>();

            switch ((PLAYERS)playerWon)
            {
                case PLAYERS.RED:
                    text.text = "Player RED Won!";
                    break;
                case PLAYERS.YELLOW:
                    text.text = "Player YELLOW Won!";
                    break;
                case PLAYERS.BLUE:
                    text.text = "Player BLUE Won!";
                    break;
                default:
                    break;
            }
        }
    }
    */
}
