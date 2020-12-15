using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    
    

    private int block = 100;
    
    public int GetBlock()
    {
        return block;
    }

    public void SetBlock(int block)
    {
        this.block = block;
    }

   

    

}