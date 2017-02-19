﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameController gameController;
    public DominoController exampleDomino;
    public string playerName;
    public List<DominoController> dominoControllers;
    public TileController tileController;
    private int cnt;



	
	void Start()
    {
        dominoControllers = new List<DominoController>(28);
//        tileController.Shuffle();
//        dominoControllers.Add(tileController.Deal());
//        dominoControllers[0].GetComponent<Transform>().transform.position.x = new Vector3(0, 0, 0);

	}

    void Update()
    {
        //Domino animation
        //dominoControllers[0].GetComponent<Transform>().transform.position;
        foreach(DominoController domino in dominoControllers)
        {
            domino.GetComponent<Transform>().transform.position = new Vector3(cnt++, -9,0);
        }

    }




    public void DominoOnClick(DominoController dominoController)
    {
        dominoControllers.Add(dominoController);


    }

    // For Tile
    public void AddDomino(DominoController dominoController)
    {
        dominoControllers.AddRange(tileController.Deal());
        
//        dominoControllers.Add(dominoController);
//        dominoController.onClick = DominoOnClick;


    }

    // For Game
    public void PlayDomino()
    {
         
    }

    public void DrawDomino(DominoController dominoController)
    {
//        tileController = new TileController();
//        if (tileController.IsDrawable())
//        {
//            
//        }
    }
}
