// FILE:    PlayerListItem.cs
// DATE:    4/25/2021
// DESC:    This file handles player room interactions.

using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text text; // text to modify
    Player player; // player being referenced

    // FUNCTION:    setup
    // DESC:        Sets up necessary info
    // PARAMETERS:  1
    //              Player _player: The player to setup with
    public void setup(Player _player)
    {
        player = _player; // set player
        text.text = _player.NickName; // set the text accordingly
    }

    // FUNCTION:    OnPlayerLeftRoom
    // DESC:        Event when player leaves room; destroys player object.
    // PARAMETERS:  1
    //              Player otherPlayer: Player to destroy
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    // FUNCTION:    OnLeftRoom
    // DESC:        Event when player leaves room; destroys player object.
    // PARAMETERS:  0
    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
