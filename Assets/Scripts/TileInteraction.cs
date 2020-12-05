﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class TileInteraction : MonoBehaviour
{

    private Grid grid;
    [SerializeField] private Tilemap interactiveMap = null;
    [SerializeField] private Tilemap pathMap = null;
    [SerializeField] private Tile hoverTile = null;
    [SerializeField] private Tile pathTile = null;
    [SerializeField] private Tile backGround = null;

    //[SerializeField] private Tile[] blocks = null;
    //[SerializeField] private int[] blockNums = null;


    private Vector3Int previousMousePos = new Vector3Int();

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
        

        if (interactiveMap.GetTile(mousePos) != backGround && interactiveMap.GetTile(mousePos) != hoverTile)
        {
            if (interactiveMap.GetTile(previousMousePos) == backGround || interactiveMap.GetTile(previousMousePos) == hoverTile)
            {
                interactiveMap.SetTile(previousMousePos, backGround);
            }
                
                     
        }
        else
        {
            if (!mousePos.Equals(previousMousePos))
            {
                if (interactiveMap.GetTile(mousePos) == backGround)
                {
                    if(interactiveMap.GetTile(previousMousePos) == backGround || interactiveMap.GetTile(previousMousePos) == hoverTile)
                    {
                        interactiveMap.SetTile(previousMousePos, backGround); // Remove old hoverTile
                        
                    }
                    interactiveMap.SetTile(mousePos, hoverTile);
                }                
                
            }

            // Left mouse click -> add path tile
            if (Input.GetMouseButton(0))
            {
                pathMap.SetTile(mousePos, pathTile);
            }

            // Right mouse click -> remove path tile
            if (Input.GetMouseButton(1))
            {
                pathMap.SetTile(mousePos, null);
            }
        }
        previousMousePos = mousePos;
    }

    Vector3Int GetMousePosition()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return grid.WorldToCell(mouseWorldPos);
    }
}

