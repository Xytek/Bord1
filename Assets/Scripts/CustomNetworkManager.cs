using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.Networking;
public class CustomNetworkManager : NetworkManager
{

    [SerializeField]
    GameObject redPlayer = null;

    [SerializeField]
    GameObject bluePlayer = null;

    [SerializeField]
    GameObject yellowPlayer = null;

    int playerSpawned = 0;

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject player = null;

        switch (playerSpawned)
        {
            case 0:
                player = (GameObject)Instantiate(bluePlayer, Vector3.zero, Quaternion.identity);
                player.GetComponent<Player>().playerColor = PLAYERS.BLUE;
                break;
            case 1:
                player = (GameObject)Instantiate(redPlayer, Vector3.zero, Quaternion.identity);
                player.GetComponent<Player>().playerColor = PLAYERS.RED;
                break;
            case 2:
                player = (GameObject)Instantiate(yellowPlayer, Vector3.zero, Quaternion.identity);
                player.GetComponent<Player>().playerColor = PLAYERS.YELLOW;
                break;
            default:
                playerSpawned = 0;
                player = (GameObject)Instantiate(bluePlayer, Vector3.zero, Quaternion.identity);
                player.GetComponent<Player>().playerColor = PLAYERS.BLUE;
                break;
        }
        player.tag = "Player";

        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        playerSpawned++;

        if (playerSpawned < 3)
        {

        }
    }
}
