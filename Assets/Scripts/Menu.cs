// FILE:    Menu.cs
// DATE:    4/25/2021
// DESC:    This file facilitates the opening and closing of different segments of the main menu.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public string menuName;

    public bool isOpen;

    // FUNCTION:    open
    // DESC:        sets the setactive() to true for the gameObject, also sets the flag.
    // PARAMETERS:  void
    public void open()
    {
        isOpen = true;
        gameObject.SetActive(true);
    }
    // FUNCTION:    close
    // DESC:        sets the setactive() to false for the gameObject, also sets the flag
    // PARAMETERS:  void
    public void close()
    {
        isOpen = false;
        gameObject.SetActive(false);
    }
}
