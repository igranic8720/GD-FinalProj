// FILE:    MenuMan.cs
// DATE:    4/25/2021
// DESC:    This file facilitates managing the menus on the main menu.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMan : MonoBehaviour
{
    public static MenuMan Instance;

    [SerializeField] Menu[] menus;
    // FUNCTION:    Awake
    // DESC:        sets the instance of the MenuMan.  Cursor settings are set to default.
    // PARAMETERS:  void
    private void Awake()
    {
        Instance = this;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    // FUNCTION:    OpenMenu
    // DESC:        opens the menu given in the string param. closes all others.
    // PARAMETERS:  string menu
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
    // FUNCTION:    OpenMenu
    // DESC:        opens the menu given in the Menu param. closes all others.
    // PARAMETERS:  Menu menu
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
    // FUNCTION:    CloseMenu
    // DESC:        closes the menu specified in the Menu param
    // PARAMETERS:  Menu menu
    public void CloseMenu(Menu menu)
    {
        menu.close();
    }

}
