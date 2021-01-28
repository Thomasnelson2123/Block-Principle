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
    [SerializeField] private Tilemap outlineMap = null;
    [SerializeField] private Tile hoverTile = null;
    [SerializeField] private Tile backGround = null;

    [SerializeField] private Tile[] blocks = null;      // an array that corresponds to the block types
    [SerializeField] private int[] blockNums = null;    // an array that corresponds to the number of each type of block for a particular level

    [SerializeField] UserInterface ui;

    [SerializeField] private Rigidbody2D rbPlayer;
    [SerializeField] private Collider2D colPlayer;
    

    private List<Vector3Int> blueBlocks; // list containing coords of any blue blocks
    [SerializeField] GameController gameControl;


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
        Brown,   //8
        EmptyBlue   //9
    }

    // Start is called before the first frame update
    void Start()
    {
        grid = gameObject.GetComponent<Grid>();
        blueBlocks = new List<Vector3Int>();
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
        // if player just jumped, we will toggle any blue platforms
        if (gameControl.InitiateJump == true)
        {
            ToggleBlue();
            gameControl.InitiateJump = false;
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
                if (Input.GetMouseButton(0) && (frontMap.GetTile(mousePos) == null) && blockNums[block] > 0 && !GetPlayerLocations().Contains(mousePos) && outlineMap.GetTile(mousePos) == null)
                {
                    frontMap.SetTile(mousePos, blocks[block]);
                    // want to keep track of the blue blocks
                    if (block == 3)
                    {
                        blueBlocks.Add(mousePos);
                    }

                    // subtract one from block selection
                    ManageBlockSelection(block, -1);

                    // update UI



                }

                // Right mouse click -> remove block tile
                if (Input.GetMouseButton(1) && (frontMap.GetTile(mousePos) == blocks[block] || 
                    (block == 3 && outlineMap.GetTile(mousePos) == blocks[9]))) // this is a special check so we can pick up inactive blue blocks
                {
                    if (outlineMap.GetTile(mousePos) == blocks[9]) // in special case above, we will delete inactive blue block from outline map
                    {
                        outlineMap.SetTile(mousePos, null);
                    }
                    else
                        frontMap.SetTile(mousePos, null);
                    // we want to keep track of blue blocks 
                    if (block == 3)
                    {
                        blueBlocks.Remove(mousePos);
                    }
                    // add one to block selection
                    ManageBlockSelection(block, 1);

                    // update UI

                }
            }
        }
        previousMousePos = mousePos; // update previous mouse position to current

        
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
        float offset = 7 / 10f; // offset is used so that the player's bounds isn't nearly as drastic
        List<Vector3Int> occupiedTiles = new List<Vector3Int>();
        Vector3 center = colPlayer.bounds.center;
        Vector3 extents = colPlayer.bounds.extents;

        // find the 4 corners of the players collider; these are the coords that the player occupies
        Vector3 topLeft = new Vector3(center.x - (extents.x * offset), center.y + (extents.y * offset)); 
        Vector3 topRight = new Vector3(center.x + (extents.x * offset), center.y + (extents.y * offset));
        Vector3 bottomLeft = new Vector3(center.x - (extents.x * offset), center.y - (extents.y * offset));
        Vector3 bottomRight = new Vector3(center.x + (extents.x * offset), center.y - (extents.y * offset));

        // add those coords to a list and return
        occupiedTiles.Add(UIMap.WorldToCell(topLeft));
        occupiedTiles.Add(UIMap.WorldToCell(topRight));
        occupiedTiles.Add(UIMap.WorldToCell(bottomLeft));
        occupiedTiles.Add(UIMap.WorldToCell(bottomRight));

        return occupiedTiles;
    }

    // blue tiles have the properity of toggling on and off when the player jumps
    // so this method, when given input that the player has jumped, will swap all blue blocks
    // with its counterpart (either active or inactive)
    private bool toggle = true;
    public void ToggleBlue()
    {
        foreach(Vector3Int w in GetPlayerLocations())
        {
            if (blueBlocks.Contains(w))
            {
                return;
            }
        }
        foreach(Vector3Int v in blueBlocks)
        {
            
            if (frontMap.GetTile(v) == blocks[3])
            {
                frontMap.SetTile(v, null);
                outlineMap.SetTile(v, blocks[9]);
            }
            else
            {
                frontMap.SetTile(v, blocks[3]);
                outlineMap.SetTile(v, null);
            }
        }
        
    }
   

}

