using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour, IGameManager
{
    public GameObject HealthBar;
    private Image _hb;

    public ManagerStatus status { get; private set; }

    public float health { get; private set; }
    public float maxHealth = 100;

    public void Startup()
    {
        Debug.Log("Player manager starting...");

        health = maxHealth;

        status = ManagerStatus.Started;
        _hb = HealthBar.GetComponent<Image>();
    }

    public void ChangeHealth(int value)
    {
        health -= value;
        _hb.fillAmount = health / maxHealth;
    }
}
