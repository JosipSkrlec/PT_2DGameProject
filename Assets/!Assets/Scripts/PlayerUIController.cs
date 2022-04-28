using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    public static PlayerUIController Instance;

    [SerializeField] private Image _healthFillImage;
    [SerializeField] private TMP_Text _livesTxt;


    // TODO - do health bar !
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void UpdatePlayerHealthUI(float playerMaxHealth, float playerCurrentHealth)
    {
        //Debug.Log("Update player health UI!");

        _healthFillImage.fillAmount = playerCurrentHealth / playerMaxHealth;

    }

    public void UpdatePlayerHealthInUI(int playerLives)
    {
        _livesTxt.text = "x" + playerLives;
    }

}
