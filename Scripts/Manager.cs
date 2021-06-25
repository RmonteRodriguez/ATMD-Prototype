using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Manager : MonoBehaviourPunCallbacks
{
    public string playerPrefab;
    public Transform spawnPoint;

    private void Start()
    {
        ValidateConnection();
        Spawn();
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
}
