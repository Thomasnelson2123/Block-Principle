﻿using System.Collections;
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
    [SerializeField] private Tile backGround = null;

    [SerializeField] private Tile[] blocks = null;      // an array that corresponds to the block types
    [SerializeField] private int[] blockNums = null;    // an array that corresponds to the number of each type of block for a particular level

    [SerializeField] UserInterface ui;

    [SerializeField] private Rigidbody2D rbPlayer;
    [SerializeField] private Collider2D colPlayer;
    [SerializeField] private Tile skullBlock;



    private Vector3Int previousMousePos = new Vector3Int();

    private int block;

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

    // Start is called before the first frame update
    void Start()
    {
        grid = gameObject.GetComponent<Grid>();
        
    }

    // Update is called once per frame
    void Update()
    {
        GetPlayerLocations();
        // Mouse over -> highlight tile
        Vector3Int mousePos = GetMousePosition();
        AssignBlock();
        // for dealing with placing the hover selection icon
        if (UIMap.GetTile(mousePos) == hoverTile && GetPlayerLocations().Contains(mousePos))
        {
            UIMap.SetTile(previousMousePos, null);
        }
        if (backMap.GetTile(mousePos) == backGround && !GetPlayerLocations().Contains(mousePos))
        {
            UIMap.SetTile(mousePos, hoverTile);
        }
        if (backMap.GetTile(mousePos) != backGround && UIMap.GetTile(mousePos) != hoverTile)
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
                    if (!GetPlayerLocations().Contains(mousePos))
                        UIMap.SetTile(mousePos, hoverTile);
                }                
                
            }
            // block is initially set as 100 until otherwise. This if is to avoid errors.
            if (block != 100)
            {
                // Left mouse click -> add block tile
                if (Input.GetMouseButton(0) && (frontMap.GetTile(mousePos) == null) && blockNums[block] > 0 && !GetPlayerLocations().Contains(mousePos))
                {
                    frontMap.SetTile(mousePos, blocks[block]);

                    // subtract one from block selection
                    ManageBlockSelection(block, -1);

                    // update UI



                }

                // Right mouse click -> remove block tile
                if (Input.GetMouseButton(1) && frontMap.GetTile(mousePos) == blocks[block])
                {
                    frontMap.SetTile(mousePos, null);

                    // add one to block selection
                    ManageBlockSelection(block, 1);

                    // update UI

                }
            }
        }
        previousMousePos = mousePos;
        
    }

    Vector3Int GetMousePosition()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return grid.WorldToCell(mouseWorldPos);
    }


    // gets the current block being used from whatever button was pressed
    private void AssignBlock()
    {
        block = ui.GetBlock();
        
    }

    // called after the removal or placement of every block. increments by one the number 
    // of that block appropriately 
    private void ManageBlockSelection(int blockType, int db)
    {
        blockNums[blockType] += db;
    }

    // will return the number of a specified block type remaining
    public int GetBlockNum(int blockType)
    {
        return blockNums[blockType];
    }

    // this method will find the tile spaces that the player is occupying, put those coords into a list, and return it
    private List<Vector3Int> GetPlayerLocations()
    {       

        List<Vector3Int> occupiedTiles = new List<Vector3Int>();
        Vector3 center = colPlayer.bounds.center;
        Vector3 extents = colPlayer.bounds.extents;

        // find the 4 corners of the players collider; these are the coords that the player occupies
        Vector3 topLeft = new Vector3(center.x - extents.x, center.y + extents.y); 
        Vector3 topRight = new Vector3(center.x + extents.x, center.y + extents.y);
        Vector3 bottomLeft = new Vector3(center.x - extents.x, center.y - extents.y);
        Vector3 bottomRight = new Vector3(center.x + extents.x, center.y - extents.y);

        // add those coords to a list and return
        occupiedTiles.Add(UIMap.WorldToCell(topLeft));
        occupiedTiles.Add(UIMap.WorldToCell(topRight));
        occupiedTiles.Add(UIMap.WorldToCell(bottomLeft));
        occupiedTiles.Add(UIMap.WorldToCell(bottomRight));

        return occupiedTiles;
    }


   

}

