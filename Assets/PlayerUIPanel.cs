using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIPanel : MonoBehaviour {
    public Text playerName;
    public Text playerScore;

    PlayerController player;

    public void AssignPlayer(int index) {
        StartCoroutine(AssignPlayerDelay(index));
    }

    IEnumerator AssignPlayerDelay(int index) {
        yield return new WaitForSeconds(.01f);
        player = GameManager.instance.playerList[index].GetComponent<PlayerInputHandler>().playerController;

        SetUpInfoPanel();
    }

    void SetUpInfoPanel() {
        if (player != null) {
            player.OnScoreChanged += UpdateScore;
            playerName.text = player.thisPlayersName.ToString();
        }
    }

    private void UpdateScore(int score) {
        playerScore.text = score.ToString();
    }
}
