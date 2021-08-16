using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Timer : MonoBehaviour
{
    bool startTimer = false;
    double timerIncrementValue;
    double startTime;
    [SerializeField] double timer = 20;
    ExitGames.Client.Photon.Hashtable CustomeValue;

    void Start()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            CustomeValue = new ExitGames.Client.Photon.Hashtable();
            //startTime = PhotonNetwork.time;
            startTimer = true;
            CustomeValue.Add("StartTime", startTime);
            PhotonNetwork.CurrentRoom.SetCustomProperties(CustomeValue);
        }
        else
        {
            startTime = double.Parse(PhotonNetwork.CurrentRoom.CustomProperties["StartTime"].ToString());
            startTimer = true;
        }
    }

    void Update()
    {

        if (!startTimer) return;

        //timerIncrementValue = PhotonNetwork.time - startTime;

        if (timerIncrementValue >= timer)
        {
            //Timer Completed
            //Do What Ever You What to Do Here
        }
    }
}
