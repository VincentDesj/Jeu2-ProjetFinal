using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    [Header("Damage Assesment")]
    public bool isDead = false;
    public bool hasEscaped = false;
    public bool isTakingDamage = false;
    public int hp = 3;
    private float colorSmoothing = 1f;


    [SerializeField] TextMeshProUGUI hpAmount;
    [SerializeField] TextMeshProUGUI finalMessage;
    public Color damageColor;
    public Image damage;


    // Start is called before the first frame update
    void Start()
    {
        UpdateHpUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (isTakingDamage)
        {
            damage.color = damageColor;
            isTakingDamage = false;
        }
        else
        {
            damage.color = Color.Lerp(damage.color, Color.clear, colorSmoothing * Time.deltaTime);
        }
    }

    private void UpdateHpUI()
    {
        char square = '\u25A0';
        string hpLeft = "";
        if (hp != 0)
        {
            for (int i = 0; i < hp; i++)
            {
                hpLeft += square.ToString();
            }
        }
        else 
        {
            isDead = true;
        }

        hpAmount.text = hpLeft;
    }

    internal void UpdateFinalMessage(string message)
    {
        finalMessage.text = message;
        finalMessage.gameObject.active = true;
    }

    private void AddToHp(int amount)
    {
        hp += amount;
        UpdateHpUI();
    }

    private void SubstactToHp(int amount)
    {
        hp -= amount;
        UpdateHpUI();
        isTakingDamage = true;
    }
}
