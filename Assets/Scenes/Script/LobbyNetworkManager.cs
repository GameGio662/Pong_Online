using UnityEngine.SceneManagement;
using Mirror;
using UnityEngine;
using System.Linq;
using System;

public class LobbyNetworkManager : NetworkManager
{
    [Scene] [SerializeField] private string menuScene = string.Empty;


    [Header ("Room")]
    [SerializeField] private NetworkRoomPlayerLobby roomPlayer = null;

    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;

    [Header("Spanw")]
    public Transform lSpawn;
    public Transform rSpawn;
    GameObject ball;

  


    public override void OnStartServer()
    {
       spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

        foreach(var prefab in spawnablePrefabs)
        {
          NetworkClient.RegisterPrefab(prefab);
        }
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();

        OnClientConnected?.Invoke();
    }

    

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        
        OnClientDisconnected?.Invoke();
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);

        if(numPlayers >= maxConnections)
        {
            conn.Disconnect();
            return;
        }



        //if(SceneManager.GetActiveScene().name != menuScene)
        //{
        //    conn.Disconnect();
        //    return;
        //}

    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        if (ball != null)
            NetworkServer.Destroy(ball);

        base.OnServerDisconnect(conn);
    }

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

        if(SceneManager.GetActiveScene().name == menuScene)
        {
            NetworkRoomPlayerLobby roomPlayerInstance = Instantiate(roomPlayer);

            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
        }


    }
}
