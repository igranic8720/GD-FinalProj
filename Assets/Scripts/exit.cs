// FILE:    exit.cs
// DATE:    4/25/2021
// DESC:    This file facilitates the exiting of the of the match over the network.
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exit : MonoBehaviour
{
    // FUNCTION:    Exit
    // DESC:        Exits game for both players by calling event over photon network.
    // PARAMETERS:  void
    public void Exit()
    {
        NetEventController.netController.SendEvent(NetEventController.EventType.EventLeave, ReceiverGroup.All);
    }
}
