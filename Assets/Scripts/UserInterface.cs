using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{

    private int block = 100;
    [SerializeField] GameController gameState;
    
    public int GetBlock()
    {
        return block;
    }

    public void SetBlock(int block)
    {
        this.block = block;
    }

    public void OpenMenu(GameObject menu)
    {
        menu.SetActive(!menu.activeSelf);
        gameState.GetStateOfMenu(menu.activeSelf);
    }

    [SerializeField] private GameObject[] buttons = null;

    [SerializeField] private GameObject dropDown = null;
    [SerializeField] private Vector2 dropDownCoord;

    [SerializeField] RectTransform rectTransDropDown;



    Vector2 initialPos;

    // general process:
    // in unity, per each level, I want to designate which blocks are available in a stage in one array
    // in another array, designate the number of blocks available, out of the selection in that stage
    // based on which blocks are available, I want to populate the screen with the appropriate block buttons,
    // accompanied by the number corresponding to number available.
    // With each block placed, I need to keep track of how many have been used, and when a block is removed,
    // account for that as well.

    void Start()
    {
        WorldtoCanvasCoords();
        initialPos = rectTransDropDown.anchoredPosition;
        Debug.Log(rectTransDropDown.anchoredPosition);
    }


    // this method is pretty bad as of now, but it gets the job done
    // will probably improve later
    // places the buttons on screen in correct place
    private void WorldtoCanvasCoords()
    {
        float x = -15f;
        foreach (GameObject b in buttons)
        {
            b.transform.position = new Vector2(x, 9.4f);
            //b.SetActive(false);
            x += 6.5f;
            b.SetActive(false);

        }
    }

    // this method is called when the drop down button is pressed
    public void DropDownMenu()
    {
        foreach (GameObject b in buttons)
        {
            b.SetActive(!b.activeInHierarchy);
        }

        // move the drop down button
        if (rectTransDropDown.anchoredPosition == initialPos)
        {
            rectTransDropDown.anchoredPosition = dropDownCoord;
        }
        else
        {
            rectTransDropDown.anchoredPosition = initialPos;
        }

    }



}