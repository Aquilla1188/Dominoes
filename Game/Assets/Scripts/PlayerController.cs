﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private const int kNumberOfCardsToDraw = 7;

    public GameController gameController;
    public List<DominoController> dominoControllers;
    public TileController tileController;



    public HistoryController historyController;
    private DominoController chosenDomino;
    private DominoController chosenPlace;
    private bool readytoplay = false;

    public Text turnText;


    public int startPosition1 = -8;
    public int startPosition2 = 8;
    public string playerName;
    private int cnt;
    float dominoScale = 1.2f;
    private bool isFirstDeal = true;

    bool drawFlag=false;

	void Start()
    {
        dominoControllers = new List<DominoController>(28);
//        tileController.Shuffle();
//        dominoControllers.Add(tileController.Deal());
//        dominoControllers[0].GetComponent<Transform>().transform.position.x = new Vector3(0, 0, 0);

	}

    void Update()
    {



    }

    public void ResetHand()
    {
        foreach (DominoController domino in dominoControllers)
        {
            Destroy(domino.gameObject);
        }
        dominoControllers.Clear();
        chosenDomino = null;
        chosenPlace = null;
        readytoplay = false;
        isFirstDeal = true;
    }



    public void DominoOnClick(DominoController clickedDomino)
    {
        //Debug.Log(clickedDomino.upperValue);
        //Debug.Log(clickedDomino.leftValue);
        bool preflag = false;
        readytoplay = false;
        string playerturn = "";

        if (turnText.text.Equals("Player1's turn"))
        {
            playerturn = "player1";
        }
        else
        {
            playerturn = "player2";
        }

        if (!playerturn.Equals(playerName))
        {
            return;
        }

        //set clicked domino
        foreach (DominoController domino in dominoControllers)
        {
            if (domino == clickedDomino)
            {           
                preflag = true;    
                //move clicked card
                
                registerDomino();


                if (chosenDomino == null)
                {
                    chosenDomino = clickedDomino;
                    clickedDomino.isClicked = true;
                    Selecteffect(clickedDomino);
                }
                else if (clickedDomino == chosenDomino)
                {
                    chosenDomino = null;
                    clickedDomino.isClicked = false;
                    Selecteffect(clickedDomino);
                }
                else if (clickedDomino != chosenDomino)
                {
                    chosenDomino.isClicked = false;
                    Selecteffect(chosenDomino);
                    chosenDomino = clickedDomino;
                    clickedDomino.isClicked = true;
                    Selecteffect(clickedDomino);
                }
                break;
            }
        }
        if (!preflag && chosenDomino != null)
        {
            readytoplay = true;
            //Debug.Log(playerName);
        }

        if (chosenDomino != null)
        {
            int horizontalLen = historyController.horizontalDominoes.Count;
            int verticalLen = historyController.verticalDominoes.Count;
            if (horizontalLen == 0 && verticalLen == 0)
            {
                chosenPlace = null;
                readytoplay = true;
                if (chosenDomino.upperValue != chosenDomino.lowerValue)
                    chosenDomino.SetLeftRightValues(chosenDomino.upperValue, chosenDomino.lowerValue);
                PlayDomino();
                chosenDomino = null;
            }
            else if(readytoplay)
            {
                if (clickedDomino == historyController.horizontalDominoes[0])
                {
                    if (clickedDomino.leftValue == -1)
                    {
                        if (chosenDomino.upperValue == clickedDomino.upperValue || chosenDomino.lowerValue == clickedDomino.upperValue)
                        {
                            chosenPlace = clickedDomino;
                            if (chosenDomino.upperValue == clickedDomino.upperValue)
                                chosenDomino.SetLeftRightValues(chosenDomino.lowerValue, chosenDomino.upperValue);
                            else
                                chosenDomino.SetLeftRightValues(chosenDomino.upperValue, chosenDomino.lowerValue);
                            PlayDomino();
                            Debug.Log("excute h_left,h_vertical,p_normal");
                            return;
                        }
                    }
                    else
                    {
                        if (chosenDomino.upperValue == clickedDomino.leftValue && chosenDomino.upperValue == chosenDomino.lowerValue)
                        {
                            chosenPlace = clickedDomino;
                            PlayDomino();
                            Debug.Log("excute h_left,h_horizontal,p_special");
                            return;
                        }
                        else if (chosenDomino.upperValue == clickedDomino.leftValue || chosenDomino.lowerValue == clickedDomino.leftValue)
                        {
                            chosenPlace = clickedDomino;
                            if (chosenDomino.upperValue == clickedDomino.leftValue)
                                chosenDomino.SetLeftRightValues(chosenDomino.lowerValue, chosenDomino.upperValue);
                            else
                                chosenDomino.SetLeftRightValues(chosenDomino.upperValue, chosenDomino.lowerValue);
                            PlayDomino();
                            Debug.Log("excute h_left,h_horizontal,p_normal");
                            return;
                        }
                    }
                }
                if (clickedDomino == historyController.horizontalDominoes[horizontalLen-1])
                {
                    if (clickedDomino.leftValue == -1)
                    {
                        if (chosenDomino.upperValue == clickedDomino.upperValue || chosenDomino.lowerValue == clickedDomino.upperValue)
                        {
                            chosenPlace = clickedDomino;
                            if (chosenDomino.upperValue == clickedDomino.upperValue)
                                chosenDomino.SetLeftRightValues(chosenDomino.upperValue, chosenDomino.lowerValue);
                            else
                                chosenDomino.SetLeftRightValues(chosenDomino.lowerValue, chosenDomino.upperValue);
                            PlayDomino();
                            Debug.Log("excute h_right,h_vertical,p_normal");
                            return;
                        }
                    }
                    else
                    {
                        if (chosenDomino.upperValue == clickedDomino.rightValue && chosenDomino.upperValue == chosenDomino.lowerValue)
                        {
                            chosenPlace = clickedDomino;
                            PlayDomino();
                            Debug.Log("excute h_right,h_horizontal,p_special");
                            return;
                        }
                        else if (chosenDomino.upperValue == clickedDomino.rightValue || chosenDomino.lowerValue == clickedDomino.rightValue)
                        {
                            chosenPlace = clickedDomino;
                            if (chosenDomino.upperValue == clickedDomino.rightValue)
                                chosenDomino.SetLeftRightValues(chosenDomino.upperValue, chosenDomino.lowerValue);
                            else
                                chosenDomino.SetLeftRightValues(chosenDomino.lowerValue, chosenDomino.upperValue);
                            PlayDomino();
                            Debug.Log("excute h_right,h_horizontal,p_normal");
                            return;
                        }
                    }
                }
                if (clickedDomino == historyController.verticalDominoes[0])
                {
                    if (clickedDomino.leftValue == -1)
                    {
                        if(chosenDomino.upperValue == clickedDomino.upperValue && chosenDomino.upperValue == chosenDomino.lowerValue)
                        {
                            chosenPlace = clickedDomino;
                            chosenDomino.SetLeftRightValues(chosenDomino.upperValue, chosenDomino.lowerValue);
                            PlayDomino();
                            Debug.Log("excute h_top,h_vertical,p_special");
                            return;
                        }
                        else if (chosenDomino.upperValue == clickedDomino.upperValue || chosenDomino.lowerValue == clickedDomino.upperValue)
                        {
                            chosenPlace = clickedDomino;
                            if (chosenDomino.upperValue == clickedDomino.upperValue)
                                chosenDomino.SetUpperLowerValues(chosenDomino.lowerValue, chosenDomino.upperValue);
                            PlayDomino();
                            Debug.Log("excute h_top,h_vertical,p_normal");
                            return;
                        }
                    }
                    else
                    {
                        if (chosenDomino.upperValue == clickedDomino.leftValue || chosenDomino.lowerValue == clickedDomino.leftValue)
                        {
                            chosenPlace = clickedDomino;
                            if (chosenDomino.upperValue == clickedDomino.leftValue)
                                chosenDomino.SetUpperLowerValues(chosenDomino.lowerValue, chosenDomino.upperValue);
                            PlayDomino();
                            Debug.Log("excute h_top,h_horizontal,p_normal");
                            return;
                        }
                    }
                }
                if (clickedDomino == historyController.verticalDominoes[verticalLen-1])
                {
                    if (clickedDomino.leftValue == -1)
                    {
                        if(chosenDomino.upperValue == clickedDomino.lowerValue && chosenDomino.upperValue == chosenDomino.lowerValue)
                        {
                            chosenPlace = clickedDomino;
                            chosenDomino.SetLeftRightValues(chosenDomino.upperValue, chosenDomino.lowerValue);
                            PlayDomino();
                            Debug.Log("excute h_bottom,h_vertical,p_special");
                            return;
                        }
                        else if (chosenDomino.upperValue == clickedDomino.lowerValue || chosenDomino.lowerValue == clickedDomino.lowerValue)
                        {
                            chosenPlace = clickedDomino;
                            if (chosenDomino.lowerValue == clickedDomino.lowerValue)
                                chosenDomino.SetUpperLowerValues(chosenDomino.lowerValue, chosenDomino.upperValue);
                            PlayDomino();
                            Debug.Log("excute h_bottom,h_vertical,p_normal");
                            return;
                        }
                    }
                    else
                    {
                        if (chosenDomino.upperValue == clickedDomino.leftValue || chosenDomino.lowerValue == clickedDomino.leftValue)
                        {
                            chosenPlace = clickedDomino;
                            if (chosenDomino.lowerValue == clickedDomino.leftValue)
                                chosenDomino.SetUpperLowerValues(chosenDomino.lowerValue, chosenDomino.upperValue);
                            PlayDomino();
                            Debug.Log("excute h_bottom,h_horizontal,p_normal");
                            return;
                        }
                    }
                }
            }
                

        }

    }

    public bool HasCardToPlay()
    {
        if (dominoControllers.Count == 0)
        {
            return false;
        }
        else
        {
            foreach (DominoController playingDomino in dominoControllers)
            {
                if (ListOfValidPlaces(playingDomino) != null)
                    return true;
            }

        }
        return false;     
    }

    public List<DominoController> ListOfValidPlaces(DominoController playingDomino)
    {
        int horizontalLen = historyController.horizontalDominoes.Count;
        int verticalLen = historyController.verticalDominoes.Count;
        List<DominoController> listOfValidPlaces = new List<DominoController>();
        //there is no cards on play zone(the first card to play)
        if (horizontalLen == 0 && verticalLen == 0)
        {
            return null;
        }
        else
        {
            List<DominoController> fourCorners = new List<DominoController>();
            if (horizontalLen != 0)
            {
                fourCorners.Add(historyController.horizontalDominoes[0]);
                fourCorners.Add(historyController.horizontalDominoes[horizontalLen-1]);
            }
            if (verticalLen != 0)
            {
                fourCorners.Add(historyController.verticalDominoes[0]);
                fourCorners.Add(historyController.verticalDominoes[verticalLen-1]);
            }
            if (fourCorners.Count != 0)
            {
                foreach (DominoController toplaceDomino in fourCorners)
                {
                    if (horizontalLen != 0)
                    {
                        if (toplaceDomino == historyController.horizontalDominoes[0])
                        {
                            //vertical toplaceDomino
                            if (toplaceDomino.leftValue == -1)
                            {
                                if (playingDomino.upperValue == toplaceDomino.upperValue || playingDomino.lowerValue == toplaceDomino.upperValue)
                                {
                                    listOfValidPlaces.Add(toplaceDomino);
                                }
                            }
                            //horizontal toplaceDomino
                            else
                            {
                                if (playingDomino.upperValue == toplaceDomino.leftValue || playingDomino.lowerValue == toplaceDomino.leftValue)
                                {

                                    listOfValidPlaces.Add(toplaceDomino);
                                }

                            }
                        }
                        if (toplaceDomino == historyController.horizontalDominoes[horizontalLen-1])
                        {
                            //vertival topalceDomino
                            if (toplaceDomino.leftValue == -1)
                            {
                                if (toplaceDomino.upperValue == playingDomino.upperValue || playingDomino.lowerValue == toplaceDomino.upperValue)
                                {
                                    listOfValidPlaces.Add(toplaceDomino);
                                }
                            }
                            //horizontal topalceDomino
                            else
                            {
                                if (playingDomino.upperValue == toplaceDomino.rightValue || playingDomino.lowerValue == toplaceDomino.rightValue)
                                {
                                    listOfValidPlaces.Add(toplaceDomino);
                                }

                            }
                        } 
                    }

                    if (verticalLen != 0)
                    {
                        if (toplaceDomino == historyController.verticalDominoes[0])
                        {
                            //vertical topalceDomino
                            if (toplaceDomino.leftValue == -1)
                            {

                                if (playingDomino.upperValue == toplaceDomino.upperValue || playingDomino.lowerValue == toplaceDomino.upperValue)
                                {
                                    listOfValidPlaces.Add(toplaceDomino);
                                }
                            }
                            //horizontal toplaceDomino
                            else
                            {
                                if (playingDomino.upperValue == toplaceDomino.leftValue || playingDomino.lowerValue == toplaceDomino.leftValue)
                                {
                                    listOfValidPlaces.Add(toplaceDomino);
                                }
                            }
                        }
                        if (toplaceDomino == historyController.verticalDominoes[verticalLen-1])
                        {
                            //vertical toplaceDomino
                            if (toplaceDomino.leftValue == -1)
                            {
                                if(playingDomino.upperValue == toplaceDomino.lowerValue || playingDomino.lowerValue == toplaceDomino.lowerValue)
                                {

                                    listOfValidPlaces.Add(toplaceDomino);
                                }

                            }
                            //horizontal toplaceDomino
                            else
                            {
                                if (playingDomino.upperValue == toplaceDomino.leftValue || playingDomino.lowerValue == toplaceDomino.leftValue)
                                {

                                    listOfValidPlaces.Add(toplaceDomino);
                                }
                            }
                        }
                    }

                }
            }

        }
        if (listOfValidPlaces.Count != 0)
            return listOfValidPlaces;
        else
            return null;
    }

    public void PlaceDomino(DominoController AIchosenDomino, DominoController AIchosenplace)
    {
        DominoController clickedDomino = AIchosenplace;
        int horizontalLen = historyController.horizontalDominoes.Count;
        int verticalLen = historyController.verticalDominoes.Count;

        if (AIchosenDomino != null)
        {
            if (clickedDomino != null)
            {
                if (clickedDomino == historyController.horizontalDominoes[0])
                {
                    if (clickedDomino.leftValue == -1)
                    {
                        if (AIchosenDomino.upperValue == clickedDomino.upperValue || AIchosenDomino.lowerValue == clickedDomino.upperValue)
                        {
                            AIchosenplace = clickedDomino;
                            if (AIchosenDomino.upperValue == clickedDomino.upperValue)
                                AIchosenDomino.SetLeftRightValues(AIchosenDomino.lowerValue, AIchosenDomino.upperValue);
                            else
                                AIchosenDomino.SetLeftRightValues(AIchosenDomino.upperValue, AIchosenDomino.lowerValue);
                            //                            PlayDomino();
                            //                            AIchosenDomino = null;
                            //                            AIchosenplace = null;
                            //                            Debug.Log("excute h_left,h_vertical,p_normal");
                            return;
                        }
                    }
                    else
                    {
                        if (AIchosenDomino.upperValue == clickedDomino.leftValue && AIchosenDomino.upperValue == AIchosenDomino.lowerValue)
                        {
                            AIchosenplace = clickedDomino;
                            //                            PlayDomino();
                            //                            AIchosenDomino = null;
                            //                            AIchosenplace = null;
                            //                            Debug.Log("excute h_left,h_horizontal,p_special");
                            return;
                        }
                        else if (AIchosenDomino.upperValue == clickedDomino.leftValue || AIchosenDomino.lowerValue == clickedDomino.leftValue)
                        {
                            AIchosenplace = clickedDomino;
                            if (AIchosenDomino.upperValue == clickedDomino.leftValue)
                                AIchosenDomino.SetLeftRightValues(AIchosenDomino.lowerValue, AIchosenDomino.upperValue);
                            else
                                AIchosenDomino.SetLeftRightValues(AIchosenDomino.upperValue, AIchosenDomino.lowerValue);
                            //                            PlayDomino();
                            //                            AIchosenDomino = null;
                            //                            AIchosenplace = null;
                            //                            Debug.Log("excute h_left,h_horizontal,p_normal");
                            return;
                        }
                    }
                }
                if (clickedDomino == historyController.horizontalDominoes[horizontalLen - 1])
                {
                    if (clickedDomino.leftValue == -1)
                    {
                        if (AIchosenDomino.upperValue == clickedDomino.upperValue || AIchosenDomino.lowerValue == clickedDomino.upperValue)
                        {
                            AIchosenplace = clickedDomino;
                            if (AIchosenDomino.upperValue == clickedDomino.upperValue)
                                AIchosenDomino.SetLeftRightValues(AIchosenDomino.upperValue, AIchosenDomino.lowerValue);
                            else
                                AIchosenDomino.SetLeftRightValues(AIchosenDomino.lowerValue, AIchosenDomino.upperValue);
                            //                            PlayDomino();
                            //                            AIchosenDomino = null;
                            //                            AIchosenplace = null;
                            //                            Debug.Log("excute h_right,h_vertical,p_normal");
                            return;
                        }
                    }
                    else
                    {
                        if (AIchosenDomino.upperValue == clickedDomino.rightValue && AIchosenDomino.upperValue == AIchosenDomino.lowerValue)
                        {
                            AIchosenplace = clickedDomino;
                            //                            PlayDomino();
                            //                            AIchosenDomino = null;
                            //                            AIchosenplace = null;
                            //                            Debug.Log("excute h_right,h_horizontal,p_special");
                            return;
                        }
                        else if (AIchosenDomino.upperValue == clickedDomino.rightValue || AIchosenDomino.lowerValue == clickedDomino.rightValue)
                        {
                            AIchosenplace = clickedDomino;
                            if (AIchosenDomino.upperValue == clickedDomino.rightValue)
                                AIchosenDomino.SetLeftRightValues(AIchosenDomino.upperValue, AIchosenDomino.lowerValue);
                            else
                                AIchosenDomino.SetLeftRightValues(AIchosenDomino.lowerValue, AIchosenDomino.upperValue);
                            //                            PlayDomino();
                            //                            AIchosenDomino = null;
                            //                            AIchosenplace = null;
                            //                            Debug.Log("excute h_right,h_horizontal,p_normal");
                            return;
                        }
                    }
                }
                if (clickedDomino == historyController.verticalDominoes[0])
                {
                    if (clickedDomino.leftValue == -1)
                    {
                        if (AIchosenDomino.upperValue == clickedDomino.upperValue && AIchosenDomino.upperValue == AIchosenDomino.lowerValue)
                        {
                            AIchosenplace = clickedDomino;
                            AIchosenDomino.SetLeftRightValues(AIchosenDomino.upperValue, AIchosenDomino.lowerValue);
                            //                            PlayDomino();
                            //                            AIchosenDomino = null;
                            //                            AIchosenplace = null;
                            //                            Debug.Log("excute h_top,h_vertical,p_special");
                            return;
                        }
                        else if (AIchosenDomino.upperValue == clickedDomino.upperValue || AIchosenDomino.lowerValue == clickedDomino.upperValue)
                        {
                            AIchosenplace = clickedDomino;
                            if (AIchosenDomino.upperValue == clickedDomino.upperValue)
                                AIchosenDomino.SetUpperLowerValues(AIchosenDomino.lowerValue, AIchosenDomino.upperValue);
                            //                            PlayDomino();
                            //                            AIchosenDomino = null;
                            //                            AIchosenplace = null;
                            //                            Debug.Log("excute h_top,h_vertical,p_normal");
                            return;
                        }
                    }
                    else
                    {
                        if (AIchosenDomino.upperValue == clickedDomino.leftValue || AIchosenDomino.lowerValue == clickedDomino.leftValue)
                        {
                            AIchosenplace = clickedDomino;
                            if (AIchosenDomino.upperValue == clickedDomino.leftValue)
                                AIchosenDomino.SetUpperLowerValues(AIchosenDomino.lowerValue, AIchosenDomino.upperValue);
                            //                            PlayDomino();
                            //                            AIchosenDomino = null;
                            //                            AIchosenplace = null;
                            //                            Debug.Log("excute h_top,h_horizontal,p_normal");
                            return;
                        }
                    }
                }
                if (clickedDomino == historyController.verticalDominoes[verticalLen - 1])
                {
                    if (clickedDomino.leftValue == -1)
                    {
                        if (AIchosenDomino.upperValue == clickedDomino.lowerValue && AIchosenDomino.upperValue == AIchosenDomino.lowerValue)
                        {
                            AIchosenplace = clickedDomino;
                            AIchosenDomino.SetLeftRightValues(AIchosenDomino.upperValue, AIchosenDomino.lowerValue);
                            //                            PlayDomino();
                            //                            AIchosenDomino = null;
                            //                            AIchosenplace = null;
                            //                            Debug.Log("excute h_bottom,h_vertical,p_special");
                            return;
                        }
                        else if (AIchosenDomino.upperValue == clickedDomino.lowerValue || AIchosenDomino.lowerValue == clickedDomino.lowerValue)
                        {
                            AIchosenplace = clickedDomino;
                            if (AIchosenDomino.lowerValue == clickedDomino.lowerValue)
                                AIchosenDomino.SetUpperLowerValues(AIchosenDomino.lowerValue, AIchosenDomino.upperValue);
                            //                            PlayDomino();
                            //                            AIchosenDomino = null;
                            //                            AIchosenplace = null;
                            //                            Debug.Log("excute h_bottom,h_vertical,p_normal");
                            return;
                        }
                    }
                    else
                    {
                        if (AIchosenDomino.upperValue == clickedDomino.leftValue || AIchosenDomino.lowerValue == clickedDomino.leftValue)
                        {
                            AIchosenplace = clickedDomino;
                            if (AIchosenDomino.lowerValue == clickedDomino.leftValue)
                                AIchosenDomino.SetUpperLowerValues(AIchosenDomino.lowerValue, AIchosenDomino.upperValue);
                            //                            PlayDomino();
                            //                            AIchosenDomino = null;
                            //                            AIchosenplace = null;
                            //                            Debug.Log("excute h_bottom,h_horizontal,p_normal");
                            return;
                        }
                    }
                }


            }
            else
            {
                if (AIchosenDomino.upperValue != AIchosenDomino.lowerValue)
                    AIchosenDomino.SetLeftRightValues(AIchosenDomino.upperValue, AIchosenDomino.lowerValue);
            }
        }
    }


    public void Selecteffect(DominoController selected)
    {
        Vector3 temp = selected.transform.position;
        if(playerName=="player1")
        {

            if (selected.isClicked)
            {
                temp.y = temp.y + (float)0.50;
                selected.transform.position = temp;
            }
            else
            {
                temp.y = temp.y - (float)0.50;
                selected.transform.position = temp;
            }
        }
        else
        {
            if (selected.isClicked)
            {
                temp.y = temp.y - (float)0.50;
                selected.transform.position = temp;
            }
            else
            {
                temp.y = temp.y + (float)0.50;
                selected.transform.position = temp;
            }
        }
    }



    // For Tile
    public void AddDomino()
    {
        // TOFIX
//        dominoControllers.AddRange(tileController.Deal());
        if (isFirstDeal)
        {
            for (int i = 0; i < kNumberOfCardsToDraw; i++)
            {
                dominoControllers.Add(tileController.DrawCard());
            }
            isFirstDeal = false;
        }
        if (playerName == "player1")
        {
            cnt = -dominoControllers.Count / 2;
            foreach (DominoController domino in dominoControllers)
            {
                // TOFIX
                domino.transform.position = new Vector3(cnt++, startPosition1, 0);
                domino.transform.localScale = new Vector3(dominoScale, dominoScale, 0);
                domino.onClick = DominoOnClick;
            }
        }
        else if(playerName == "player2")
        {
            cnt = -dominoControllers.Count / 2; ;
            foreach (DominoController domino in dominoControllers)
            {
                // TOFIX
                domino.transform.position = new Vector3(cnt++, startPosition2, 0);
                domino.transform.localScale = new Vector3(dominoScale, dominoScale, 0);
                domino.onClick = DominoOnClick;
            }
        }
           

    }

    // For Game
    public void PlayDomino()
    {
        if (!HasCardToPlay())
        {
            //not first deal
            if (historyController.verticalDominoes.Count != 0 || historyController.horizontalDominoes.Count != 0)
            {
                DrawDomino();
            }

        }
        if (chosenDomino != null && readytoplay == true)
        {
            dominoControllers.Remove(chosenDomino);
            AddDomino();
            if (playerName == "player1")
            {
                DominoController tcd, tcp;
                tcd = chosenDomino;
                tcp = chosenPlace;
                chosenDomino = null;
                chosenPlace = null;
                gameController.PlayerPlayDomino(this, tcd, tcp);
                //                AddDomino();

            }
            else if (playerName == "player2")
            {
                DominoController tcd, tcp;
                tcd = chosenDomino;
                tcp = chosenPlace;
                chosenDomino = null;
                chosenPlace = null;
                gameController.PlayerPlayDomino(this, tcd, tcp);
                //                AddDomino();
            }
        }   
                 
    }
       
    public void registerDomino()
    {
        int horizontalLen = historyController.horizontalDominoes.Count;
        int verticalLen = historyController.verticalDominoes.Count;
        if (horizontalLen != 0)
        {
            historyController.horizontalDominoes[0].onClick = DominoOnClick;
            historyController.horizontalDominoes[horizontalLen-1].onClick = DominoOnClick;
        }
        if (verticalLen != 0)
        {
            historyController.verticalDominoes[0].onClick = DominoOnClick;
            historyController.verticalDominoes[verticalLen-1].onClick = DominoOnClick;
        }
    }

    public void DrawDomino()
    {
        
        if (HasCardToPlay())
        {
            return;
        }
        else
        {
            while (!HasCardToPlay())
            {
                if (tileController.IsDrawable())
                {
                    DominoController addedDomino = tileController.DrawCard();
                    if (addedDomino != null)
                    {
                        dominoControllers.Add(addedDomino);
                        AddDomino(); 
                    }
                }
                else
                {
                    gameController.PlayerIsBlocked(this);
                    return;
                }
            }
        }

    }
}
