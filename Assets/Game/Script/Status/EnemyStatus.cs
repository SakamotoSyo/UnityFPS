using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class EnemyStatus : MonoBehaviour
{
    
    [SerializeField][Tooltip("Å‘åHP")] private int MaxHp;
    [SerializeField][Tooltip("HP")]private float hp;
    [SerializeField][Tooltip("—Í")]private int power;
    [SerializeField][Tooltip("‘f‘‚³")]private int agility;
    [SerializeField][Tooltip("‚¨‹à")] private float money = 0;
    //public int MaxHp1 { get => MaxHp; set => MaxHp = value; }

    public void SetMaxHp(int hp)
    {
        this.MaxHp = hp;
    }

    public int GetMaxHp() => MaxHp;

    public void SetHp(float hp)
    {
        this.hp = Mathf.Max(0, Mathf.Min(GetMaxHp(), hp));
    }

    public float GetHp() => hp;

    public void SetPower(int power)
    {
        this.power = power; 
    }

    public int GetPower() => power;

    public void SetAgility(int agillity)
    {
        this.agility = agillity;
    }

    public int GetAgility() => agility;

    public void DamageHp(float damage)
    {
        hp -= damage;
    }

    public void Setmoney(float money) 
    {
        this.money = money;
    }

    public float GetMoney() => money;
}
