// FILE:    PlayerController.cs
// DATE:    4/25/2021
// DESC:    This file houses the main player interaction.

using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Gui;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerController : MonoBehaviourPunCallbacks, IDamageable
{
    [SerializeField] private Image healthbarImage; // image of the healthbar.
    [SerializeField] private GameObject ui; // ui canvas
    [SerializeField] private GameObject cameraHolder; // the camera
    [SerializeField] private float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime; // used for movement
    [SerializeField] public Item[] items; // item holding array
    [SerializeField] public Text playerDeathCounter; // score counter
    
    public int itemIndex; // currently held item
    private int previousItemIndex = -1; // previously held item

    private float verticalLookRotation; // used with movement controller
    private bool grounded; // if the player is on the ground (uses playergroundcheck.cs)
    private Vector3 smoothMoveVelocity; // used with movement controller
    private Vector3 moveAmount; // used with movement controller
    private Rigidbody rb; // this player's rigidbody

    private PhotonView PV; // this player's photonview

    private const float maxHealth = 100f; // the highest possible health
    private float currentHealth = maxHealth; // current player health

    private PlayerManager playerManager; // manager managing this player

    private float lastFired; // used for firing weps

    // FUNCTION:    Awake
    // DESC:        Sets required vars
    // PARAMETERS:  0
    void Awake()
    {
        rb = GetComponent<Rigidbody>(); // sets rigidbody
        PV = GetComponent<PhotonView>(); // sets photonview

        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>(); // gets the playermanager
    }

    // FUNCTION:    Start
    // DESC:        Sets required player components, deletes if not necessary
    // PARAMETERS:  0
    void Start()
    {
        if (PV.IsMine)
        {
            EquipItem(0); // if mine, start with weapon
            Cursor.visible = false; // cursor FPS mode
            Cursor.lockState = CursorLockMode.Locked;
            playerDeathCounter.text = RoomPlayerInfo.roomPlayerInfo.localScore + " : " + RoomPlayerInfo.roomPlayerInfo.enemyScore;
        }
        else
        {
            // if not me, destroy anything not needed
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
            Destroy(ui);

            // reposition guns on enemy player
            foreach (Item gun in items)
            {
                gun.itemGameObject.transform.localPosition = new Vector3(0.3f, -0.12f, 0.8f);
            }
        }
    }

    // FUNCTION:    Update
    // DESC:        Manages things that happen every frame.
    // PARAMETERS:  0
    void Update()
    {
        if (!PV.IsMine) return;
        if (GameObject.FindGameObjectWithTag("Pause").GetComponent<LeanWindow>().On == false)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            ui.SetActive(true);
            Look();
            Move();
            Jump();
            WeaponFiring();

            for (int i = 0; i < items.Length; i++) // switch weapons if corresponding key is pressed
            {
                if (Input.GetKeyDown((i + 1).ToString()))
                {
                    EquipItem(i);
                    break;
                }
            }

            // mouse wheel scrolling change weps
            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
            {
                if (itemIndex >= items.Length - 1)
                {
                    EquipItem(0);
                }
                else
                {
                    EquipItem(itemIndex + 1);
                }
            }

            // mouse wheel scrolling change weps
            if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
            {
                if (itemIndex <= 0)
                {
                    EquipItem(items.Length - 1);
                }
                else
                {
                    EquipItem(itemIndex - 1);
                }
            }
        }
        
        // respawn if fell into void
        if (transform.position.y < -10f)
        {
            playerManager.Respawn();
        }

        // pause game on tab
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ui.SetActive(false);
            GameObject.FindGameObjectWithTag("Pause").GetComponent<LeanWindow>().TurnOn();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    // FUNCTION:    FixedUpdate
    // DESC:        Manages movement
    // PARAMETERS:  0
    void FixedUpdate()
    {
        if (!PV.IsMine) return;
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

    // FUNCTION:    Move
    // DESC:        Manages movement
    // PARAMETERS:  0
    void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
    }

    // FUNCTION:    Jump
    // DESC:        Handles jumping
    // PARAMETERS:  0
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(transform.up * jumpForce);
        }
    }

    // FUNCTION:    Look
    // DESC:        Handles mouse movement
    // PARAMETERS:  0
    void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    // FUNCTION:    WeaponFiring
    // DESC:        Handles firing weapons
    // PARAMETERS:  0
    void WeaponFiring()
    {
        if (items[itemIndex] == null) return;
        if (items[itemIndex].IsMultiFire()) // automatic weapon
        {
            if (!Input.GetMouseButton(0)) return;
            if (Time.time - lastFired > 1f / 8f) // 8f is fire rate
            {
                lastFired = Time.time;
                items[itemIndex].Use();
            }
        }
        else
        {
            if (!Input.GetMouseButtonDown(0)) return;
            items[itemIndex].Use();
        }
    }

    // FUNCTION:    EquipItem
    // DESC:        Switches weapons
    // PARAMETERS:  1
    //              int _index: The new weapon index
    void EquipItem(int _index)
    {
        if (_index == previousItemIndex) return; // switching to same wep?
        itemIndex = _index; // set new index
        items[itemIndex].itemGameObject.SetActive(true); // see the new eep
        if (previousItemIndex != -1)
        {
            items[previousItemIndex].itemGameObject.SetActive(false);
        }
        previousItemIndex = itemIndex;

        if (PV.IsMine) // sync new wep switch over pun
        {
            Hashtable hash = new Hashtable
            {
                { "itemIndex", itemIndex }
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    // FUNCTION:    OnPlayerPropertiesUpdate
    // DESC:        Called on hashtable properties change; used to update player weapons.
    // PARAMETERS:  2
    //              Player targetPlayer: Target player
    //              Hashtable changedProps: all the changed properties
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!PV.IsMine && targetPlayer == PV.Owner)
        {
            EquipItem((int)changedProps["itemIndex"]);
        }
    }

    // FUNCTION:    SetGroundedState
    // DESC:        Sets the ground state
    // PARAMETERS:  1
    //              bool _grounded: new ground state
    public void SetGroundedState(bool _grounded)
    {
        grounded = _grounded;
    }

    // FUNCTION:    TakeDamage
    // DESC:        Handles taking damage
    // PARAMETERS:  1
    //              float damage: how much damage to take
    public void TakeDamage(float damage)
    {
        PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }

    // FUNCTION:    RPC_TakeDamage
    // DESC:        Handles taking damage
    // PARAMETERS:  1
    //              float damage: how much damage to take
    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        if (!PV.IsMine) return;

        currentHealth -= damage; // decrement health
        healthbarImage.fillAmount = currentHealth / maxHealth; // adjust hp bar

        if (currentHealth <= 0)
        {
            // player just died, increment the other score and respawn everyone
            NetEventController.netController.SendEvent(
                NetEventController.EventType.EventIncrementScore, ReceiverGroup.All, 
                new object[]
                {
                    RoomPlayerInfo.roomPlayerInfo.GetEnemyPlayer().ActorNumber
                });

            NetEventController.netController.SendEvent(
                NetEventController.EventType.EventAddKillMessage, ReceiverGroup.All, 
                new object[]
                {
                    RoomPlayerInfo.roomPlayerInfo.GetEnemyPlayer().ActorNumber, 
                    RoomPlayerInfo.roomPlayerInfo.GetLocalPlayer().ActorNumber,
                    Vector3.Distance(gameObject.transform.position, RoomPlayerInfo.roomPlayerInfo.GetEnemyPlayerGO().transform.position)
                });

            NetEventController.netController.SendEvent(
                NetEventController.EventType.EventRespawn, ReceiverGroup.All);
        }
    }
    
    
}
