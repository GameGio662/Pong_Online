using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkManagerPong : NetworkManager
{
    public Transform lSpawn;
    public Transform rSpawn;
    GameObject ball;


    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Transform start = numPlayers == 0 ? lSpawn : rSpawn;
        GameObject player = Instantiate(playerPrefab, start.position, start.rotation);
        NetworkServer.AddPlayerForConnection(conn, player);


        if (numPlayers == 2)
        {
            ball = Instantiate(spawnPrefabs.Find(Prefab => Prefab.name == "Ball"));
            NetworkServer.Spawn(ball);
        }

        base.OnServerAddPlayer(conn);
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {

        if (ball != null)
            NetworkServer.Destroy(ball);


        base.OnServerDisconnect(conn);
    }
}
