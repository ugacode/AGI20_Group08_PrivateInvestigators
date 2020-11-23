using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public CityRatPlayer player;
    public Text speedText;
    public Text cluesText;

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            speedText.text = $"Player speed - {player.speed:0.#} m\\s";
            cluesText.text = $"Clues - {player.collectedClues}/10";
        }
    }
}
