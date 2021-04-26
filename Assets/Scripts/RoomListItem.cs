// FILE:    RoomListItem.cs
// DATE:    4/25/2021
// DESC:    This file assists in listing rooms.

using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text text; // text used to display room info

    public RoomInfo info; // this instance

    // FUNCTION:    setup
    // DESC:        Sets up a button.
    // PARAMETERS:  1
    //              RoomInfo _info: The info of this room
    public void setup(RoomInfo _info)
    {
        info = _info; // set the info
        text.text = _info.Name + " : " + info.PlayerCount + "/" + info.MaxPlayers; // set str accordingly
    }

    // FUNCTION:    OnClick
    // DESC:        When a click happens, a room is joined.
    // PARAMETERS:  0
    public void OnClick()
    {
        Launcher.Instance.JoinRoom(info);
    }
}
