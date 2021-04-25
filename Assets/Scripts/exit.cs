using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exit : MonoBehaviour
{
    public void Exit()
    {
        NetEventController.netController.SendEvent(NetEventController.EventType.EventLeave, ReceiverGroup.All);
    }
}
