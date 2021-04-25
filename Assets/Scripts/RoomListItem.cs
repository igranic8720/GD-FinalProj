using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    public RoomInfo info;
    public void setup(RoomInfo _info)
    {
        info = _info;
        text.text = _info.Name + " : " + info.PlayerCount + "/" + info.MaxPlayers;
    }

    public void OnClick()
    {
        Launcher.Instance.JoinRoom(info);
    }
}
