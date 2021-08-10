using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class Manager : MonoBehaviourPunCallbacks
{
    public string playerPrefab;
    public Transform spawnPoint;

    //Bomb Gamemode
    public string diffuserPrefab;
    public Transform diffuserSpawnPoint;

    private void Start()
    {
        ValidateConnection();
        Spawn();

        if (PhotonNetwork.CurrentRoom.PlayerCount >= 0)
        {
            SpawnDiffuser();
        }
    }

    public void Spawn()
    {
        PhotonNetwork.Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    [PunRPC]
    private void ValidateConnection()
    {
        if (PhotonNetwork.IsConnected) return;
        SceneManager.LoadScene(0);
    }

    public void SpawnDiffuser()
    {
        PhotonNetwork.Instantiate(diffuserPrefab, diffuserSpawnPoint.position, diffuserSpawnPoint.rotation);
    }
}
