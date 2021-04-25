using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class NetEventController : MonoBehaviour, IOnEventCallback
{
    public static NetEventController netController;

    public enum EventType
    {
        EventRespawn = 1,
        EventIncrementScore,
        EventAddKillMessage,
        EventLeave,
        EVENT_MAX
    }

    /* EVENTS AND ARGUMENTS
    
        // EVENT:       EventRespawn (1)
        // DESC:        Simply respawn the targeted player.
        // ARGUMENTS:   0

        // EVENT:       EventIncrementScore (2)
        // DESC:        Increment the score for the selected player by 1.
        // ARGUMENTS:   1
        //              int player - The index of the player to update

        // EVENT:       EventAddKillMessage (3)
        // DESC:        Add a kill message at the top left
        // ARGUMENTS:   3
        //              int shooter - The index of the player who killed the other player
        //              int victim - The index of the player who died
        //              float distance - The distance of the kill

        // EVENT:       EventLeave (4)
        // DESC:        Orders target to leave the room
        // ARGUMENTS:   0
    */

    void Awake()
    {
        netController = this;
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void SendEvent(EventType type, ReceiverGroup recipients, object[] data = null)
    {
        PhotonNetwork.RaiseEvent((byte) type, data, new RaiseEventOptions { Receivers = recipients }, SendOptions.SendReliable);
    }

    public void OnEvent(EventData photonEvent)
    {
        if ((EventType)photonEvent.Code >= EventType.EVENT_MAX) return;

        EventType type = (EventType) photonEvent.Code;
        object[] data = (object[]) photonEvent.CustomData;
        switch (type) // get the type of event
        {
            case EventType.EventRespawn:
                RoomPlayerInfo.roomPlayerInfo.GetLocalPlayerManager().Respawn();
                break;

            case EventType.EventIncrementScore:
                if ((int) data[0] == RoomPlayerInfo.roomPlayerInfo.GetLocalPlayer().ActorNumber)
                {
                    RoomPlayerInfo.roomPlayerInfo.localScore++;
                }
                else
                {
                    RoomPlayerInfo.roomPlayerInfo.enemyScore++;
                }

                RoomPlayerInfo.roomPlayerInfo.GetLocalPlayerManager().GetController().GetComponent<PlayerController>().playerDeathCounter.text = RoomPlayerInfo.roomPlayerInfo.localScore + " : " + RoomPlayerInfo.roomPlayerInfo.enemyScore;

                break;

            case EventType.EventAddKillMessage:
                Player shooter = PhotonNetwork.CurrentRoom.GetPlayer((int) data[0]);
                Player victim = PhotonNetwork.CurrentRoom.GetPlayer((int) data[1]);
                float distance = (float) data[2];
                break;

            case EventType.EventLeave:
                if (PhotonNetwork.IsMasterClient) PhotonNetwork.DestroyAll();
                PhotonNetwork.Disconnect();
                SceneManager.LoadScene(0);
                break;
        }
    }
}
