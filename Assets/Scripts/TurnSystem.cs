using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance { get; private set; }
    private int turnNumber = 1;
    private bool isPlayerTurn = true;
    public event EventHandler OnTurnChanged;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log($"Instance Error for {nameof(TurnSystem)}");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    public void NextTurn()
    {
        turnNumber++;
        isPlayerTurn = !isPlayerTurn;
        OnTurnChanged?.Invoke(this,EventArgs.Empty);
    }
    public int GetTurnNumber() => turnNumber;
    public bool IsPlayerTurn() => isPlayerTurn;
}
