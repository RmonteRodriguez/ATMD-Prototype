using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class Manager : MonoBehaviourPunCallbacks
{
    public string playerPrefab;
    public Transform spawnPoint;

    public string timerPrefab;

    //Timer
    //public float timer;
    //public float timeDecreasedPerSecond;
    //public Text timerText;

    private void Start()
    {
        ValidateConnection();
        Spawn();

        //timer = 60f;
        //timeDecreasedPerSecond = 1f;
    }

    void Update()
    {
        //photonView.RPC("TimerCountdown", RpcTarget.All);
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
    /*
    [PunRPC]
    public void TimerCountdown()
    {
        timerText.text = (int)timer + " ";
        timer -= timeDecreasedPerSecond * Time.deltaTime;
    }

    /*

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {

            stream.SendNext(timer);
        }
        else
        {
            timer = (float)stream.ReceiveNext();

        }

    }
    */
}
