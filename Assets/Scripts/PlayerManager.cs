using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour, IGameManager
{
    public GameObject HealthBar;
    
    public GameObject EndGameImg;
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
        if (health <= 0)
        {
            StartCoroutine(EndGame());
        }
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(1f);
        EndGameImg.SetActive(true);
        yield return new WaitForSeconds(2f);
        Application.Quit();
    }
}
