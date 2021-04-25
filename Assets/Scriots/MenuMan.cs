using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMan : MonoBehaviour
{
    public static MenuMan Instance;

    [SerializeField] Menu[] menus;

    private void Awake()
    {
        Instance = this;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OpenMenu(string menu)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName == menu)
            {
                menus[i].open();
            }
            else if (menus[i].isOpen)
            {
                CloseMenu(menus[i]);
            }
        }
    }

    public void OpenMenu(Menu menu)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].isOpen)
            {
                CloseMenu(menus[i]);
            }
        }
        menu.open();
    }

    public void CloseMenu(Menu menu)
    {
        menu.close();
    }

}
