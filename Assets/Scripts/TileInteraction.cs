using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class TileInteraction : MonoBehaviour
{

    private Grid grid;
    [SerializeField] private Tilemap backMap = null;
    [SerializeField] private Tilemap frontMap = null;
    [SerializeField] private Tilemap UIMap = null;
    [SerializeField] private Tile hoverTile = null;
    [SerializeField] private Tile block = null;
    [SerializeField] private Tile backGround = null;

    [SerializeField] private Tile[] blocks = null;      // an array that corresponds to the block types
    [SerializeField] private int[] blockNums = null;    // an array that corresponds to the number of each type of block for a particular level

    [SerializeField] UserInterface ui;


    private Vector3Int previousMousePos = new Vector3Int();

    // these enums will more than likely be unnecessary, TBD
    enum Block
    {
        Grey,   //0
        White,  //1
        Red,    //2
        Blue,   //3
        Yellow, //4
        Green,  //5
        Purple, //6
        Orange, //7
        Brown   //8
    }

    Block currentBlock = Block.Grey;

    // Start is called before the first frame update
    void Start()
    {
        grid = gameObject.GetComponent<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        
        // Mouse over -> highlight tile
        Vector3Int mousePos = GetMousePosition();
        
        // for dealing with placing the hover selection icon
        if (backMap.GetTile(mousePos) != backGround && backMap.GetTile(mousePos) != hoverTile)
        {
            if (backMap.GetTile(previousMousePos) == backGround || UIMap.GetTile(previousMousePos) == hoverTile)
            {
                UIMap.SetTile(previousMousePos, null);
            }
                
                     
        }
        else
        {
            if (!mousePos.Equals(previousMousePos))
            {
                if (backMap.GetTile(mousePos) == backGround)
                {
                    if(backMap.GetTile(previousMousePos) == backGround || UIMap.GetTile(previousMousePos) == hoverTile)
                    {
                        UIMap.SetTile(previousMousePos, null); // Remove old block
                        
                    }
                    UIMap.SetTile(mousePos, hoverTile);
                }                
                
            }

            // Left mouse click -> add path tile
            if (Input.GetMouseButton(0))
            {
                frontMap.SetTile(mousePos, blocks[AssignBlock()]);
            }

            // Right mouse click -> remove path tile
            if (Input.GetMouseButton(1))
            {
                frontMap.SetTile(mousePos, null);
            }
        }
        previousMousePos = mousePos;
    }

    Vector3Int GetMousePosition()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return grid.WorldToCell(mouseWorldPos);
    }

    // I will have a seperate script for button presses, which will tell us
    // what block we are currently using. This script is also going to communicate back to the 
    // UI / button script, telling it what to display
    // that is, this script will know what blocks are available per level, and the number of which as well

    
    private int AssignBlock()
    {
        currentBlock = (Block)ui.GetBlock();
        return (int)currentBlock;
    }
}

