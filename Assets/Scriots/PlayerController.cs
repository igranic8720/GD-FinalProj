using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerController : MonoBehaviourPunCallbacks, IDamageable
{
    [SerializeField] private Image healthbarImage;
    [SerializeField] private GameObject ui;
    [SerializeField] private GameObject cameraHolder;
    [SerializeField] private float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;
    [SerializeField] private Item[] items;
    [SerializeField] private Text playerDeathCounter;
    
    private int itemIndex;
    private int previousItemIndex = -1;

    private float verticalLookRotation;
    private bool grounded;
    private Vector3 smoothMoveVelocity;
    private Vector3 moveAmount;

    private Rigidbody rb;

    private PhotonView PV;

    private const float maxHealth = 100f;
    private float currentHealth = maxHealth;

    private PlayerManager playerManager;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();

        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    void Start()
    {
        if (PV.IsMine)
        {
            EquipItem(0);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            playerDeathCounter.text = "0 : 0";
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
            Destroy(ui);
        }
    }

    void Update()
    {
        if (!PV.IsMine) return;
        Look();
        Move();
        Jump();

        for (int i = 0; i < items.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                EquipItem(i);
                break;
            }
        }

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

        if (Input.GetMouseButtonDown(0))
        {
            items[itemIndex].Use();
        }

        if (transform.position.y < -10f)
        {
            Die();
        }

        if (Input.GetKeyDown(KeyCode.Q)) EndGame();
    }
    void FixedUpdate()
    {
        if (!PV.IsMine) return;
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

    void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(transform.up * jumpForce);
        }
    }

    void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    void EquipItem(int _index)
    {
        if (_index == previousItemIndex) return;

        itemIndex = _index;

        items[itemIndex].itemGameObject.SetActive(true);

        if (previousItemIndex != -1)
        {
            items[previousItemIndex].itemGameObject.SetActive(false);
        }

        previousItemIndex = itemIndex;

        if (PV.IsMine)
        {
            Hashtable hash = new Hashtable
            {
                { "itemIndex", itemIndex }
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!PV.IsMine && targetPlayer == PV.Owner)
        {
            EquipItem((int)changedProps["itemIndex"]);
        }
    }

    public void SetGroundedState(bool _grounded)
    {
        grounded = _grounded;
    }

    public void TakeDamage(float damage)
    {
        PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        if (!PV.IsMine) return;

        currentHealth -= damage;
        healthbarImage.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void EndGame()
    {
        if (PhotonNetwork.IsMasterClient) PhotonNetwork.DestroyAll();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }

    void Die()
    {
        playerManager.playerDeaths++;

        Hashtable hash = new Hashtable
        {
            { "playerDeaths", playerManager.playerDeaths }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

        PV.RPC("RPC_Respawn", RpcTarget.Others);

        playerManager.Die();
        Player[] players = PhotonNetwork.PlayerList;
        foreach (Player pl in players)
        {
            if (!pl.IsLocal) playerDeathCounter.text = playerManager.playerDeaths + " : " + (int)pl.CustomProperties["playerDeaths"];
        }
    }

    [PunRPC]
    void RPC_Respawn()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("Player");
        foreach (var v in go)
        {
            if (v.GetComponent<PhotonView>().IsMine)
            {
                PhotonNetwork.Destroy(v);
                playerManager.CreateController();
                GameObject[] go2 = GameObject.FindGameObjectsWithTag("Player");
                foreach (var v2 in go2)
                {
                    Player[] players = PhotonNetwork.PlayerList;
                    foreach (Player pl in players)
                    {
                        if (!pl.IsLocal && v2.GetComponent<PhotonView>().IsMine)
                        {
                            v2.GetComponent<PlayerController>().playerDeathCounter.text = playerManager.playerDeaths + " : " + (int)pl.CustomProperties["playerDeaths"];
                        }  
                    }
                }

                break;
            }
        }
        
    }

}
