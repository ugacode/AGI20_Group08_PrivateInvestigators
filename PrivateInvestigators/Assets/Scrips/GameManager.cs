/*
Keeps check of where you are in the game.

Author: Karin Lagrelius
*/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject panelMenu;
    public GameObject locationGame;
    public enum State{ MENU, INIT, CITYRAT}
    State _state;

    public void InvestigateClicked(){
      SwitchState(State.CITYRAT);
    }

    // Start is called before the first frame update
    void Start()
    {
      SwitchState(State.MENU);
    }

    public void SwitchState(State newState){
      EndState();
      BeginState(newState);
    }

    void BeginState(State newState)
    {
      switch(newState)
      {
        case State.MENU:
          panelMenu.SetActive(true);
          break;
        case State.INIT:
          break;
        case State.CITYRAT:
          locationGame.SetActive(true);
          break;
      }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void EndState()
    {
      switch(_state)
      {
        case State.MENU:
          panelMenu.SetActive(false);
          break;
        case State.INIT:
          break;
        case State.CITYRAT:
          locationGame.SetActive(false);
          break;
      }
    }

}
