using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class Panels : MonoBehaviour
{
    [SerializeField] Image tensPlace;
    [SerializeField] Image onesPlace;

    [SerializeField] Tile[] numbers;

    [SerializeField] TileInteraction grid = new TileInteraction();

    [SerializeField] private int blockType;
    private int number;

    void Update()
    {
        number = grid.GetBlockNum(blockType);
        ConvertNumbertoImage();
    }

    private void ConvertNumbertoImage()
    {
        int tens = number / 10;
        int ones = number - (tens * 10);

        tensPlace.sprite = numbers[tens].sprite;
        onesPlace.sprite = numbers[ones].sprite;

    }

}
