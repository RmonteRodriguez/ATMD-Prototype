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

    public bool matchReadyToStart;
    public bool matchStarted;

    //Timer
    public float timer;
    public float initialTime;
    public float timeDecreasedPerSecond;
    public Text timerText;

    //Team Scores
    public float redTeamScore;
    public float blueTeamScore;
    public Text redText;
    public Text blueText;

    private void Start()
    {
        ValidateConnection();
        Spawn();

        matchReadyToStart = false;

        timer = initialTime;
        timeDecreasedPerSecond = 1f;
    }

    void Update()
    {

        if (matchStarted == true)
        {
            redText.text = (int)redTeamScore + " Pts";
            blueText.text = (int)blueTeamScore + " Pts";
        }
        else
        {
            redText.text = " ";
            blueText.text = " ";
        }

        if(PhotonNetwork.IsMasterClient)
        {
            if (matchReadyToStart == true && timer > 0)
            {
                photonView.RPC("TimerCountdown", RpcTarget.All);
            }
            else
            {
                timerText.text = " ";
            }
        }
        else
        {
            timerText.text = " ";
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
    
    [PunRPC]
    public void TimerCountdown()
    {
        timerText.text = "Match Start: " + (int)timer;
        timer -= timeDecreasedPerSecond * Time.deltaTime;
    }
}
