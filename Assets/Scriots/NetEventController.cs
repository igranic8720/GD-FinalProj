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
        EventQuit,
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
                GameObject localCanvas = GameObject.FindGameObjectWithTag("LocalCanvas");
                if ((int) data[0] == RoomPlayerInfo.roomPlayerInfo.GetLocalPlayer().ActorNumber)
                {
                    RoomPlayerInfo.roomPlayerInfo.localScore++;
                    if(RoomPlayerInfo.roomPlayerInfo.localScore >= 5)
                    {
                        localCanvas.GetComponent<Animator>().ResetTrigger("MatchWinTrig");
                        localCanvas.GetComponent<Animator>().SetTrigger("MatchWinTrig");
                        StartCoroutine(QuitCoroutine());
                    }
                    else
                    {
                        localCanvas.GetComponent<Animator>().ResetTrigger("RoundWinTrig");
                        localCanvas.GetComponent<Animator>().SetTrigger("RoundWinTrig");
                    }
                }
                else
                {
                    RoomPlayerInfo.roomPlayerInfo.enemyScore++;
                    if (RoomPlayerInfo.roomPlayerInfo.enemyScore >= 5)
                    {
                        localCanvas.GetComponent<Animator>().ResetTrigger("MatchLossTrig");
                        localCanvas.GetComponent<Animator>().SetTrigger("MatchLossTrig");
                        StartCoroutine(QuitCoroutine());
                    }
                    else
                    {
                        localCanvas.GetComponent<Animator>().ResetTrigger("RoundLossTrig");
                        localCanvas.GetComponent<Animator>().SetTrigger("RoundLossTrig");
                    }
                }



                RoomPlayerInfo.roomPlayerInfo.GetLocalPlayerManager().GetController().GetComponent<PlayerController>().playerDeathCounter.text = RoomPlayerInfo.roomPlayerInfo.localScore + " : " + RoomPlayerInfo.roomPlayerInfo.enemyScore;

                break;

            case EventType.EventAddKillMessage:
                Player shooter = PhotonNetwork.CurrentRoom.GetPlayer((int) data[0]);
                Player victim = PhotonNetwork.CurrentRoom.GetPlayer((int) data[1]);
                float distance = (float)data[2];
                GameObject KillFeed = GameObject.FindGameObjectWithTag("KillFeed");
                string shooterNick = "";
                string victimNick = "";
                if (PhotonNetwork.LocalPlayer.NickName == shooter.NickName)
                {
                    shooterNick = "You";
                }
                else
                {
                    shooterNick = shooter.NickName;
                }
                if (PhotonNetwork.LocalPlayer.NickName == victim.NickName)
                {
                    victimNick = "You";
                }
                else
                {
                    victimNick = victim.NickName;
                }
                KillFeed.GetComponent<Killfeed>().newKill(shooterNick, victimNick, distance);
                Debug.Log(shooter.NickName + " killed " + victim.NickName);
                break;

            case EventType.EventLeave:
                StartCoroutine(leaveCoroutine());
                break;
            case EventType.EventQuit:
                if (PhotonNetwork.IsMasterClient) PhotonNetwork.DestroyAll();
                PhotonNetwork.Disconnect();
                SceneManager.LoadScene(0);
                break;
        }

        IEnumerator leaveCoroutine()
        {
            GameObject localCanvas = GameObject.FindGameObjectWithTag("LocalCanvas");
            localCanvas.GetComponent<Animator>().ResetTrigger("PlayerLeft");
            localCanvas.GetComponent<Animator>().SetTrigger("PlayerLeft");
            yield return new WaitForSeconds(6);
            netController.SendEvent(NetEventController.EventType.EventQuit, ReceiverGroup.All);
        }

        IEnumerator QuitCoroutine()
        {
            yield return new WaitForSeconds(6);
            netController.SendEvent(NetEventController.EventType.EventLeave, ReceiverGroup.All);
        }
    }
}
