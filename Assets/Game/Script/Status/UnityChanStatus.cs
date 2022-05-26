using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable][CreateAssetMenu(fileName = "UnityStatus", menuName = "UnityStatus")]
public class UnityChanStatus : ScriptableObject
{
    // Start is called before the first frame update

    [SerializeField][Tooltip("Å‘åHP")] private float MaxHp;
    [SerializeField][Tooltip("HP")] private float hp;
    [SerializeField][Tooltip("—Í")] private int power;
    [SerializeField][Tooltip("‘f‘‚³")] private int agility;
    [SerializeField][Tooltip("‚¨‹à")] private float money;
    //public int MaxHp1 { get => MaxHp; set => MaxHp = value; }

    public void SetMaxHp(int hp)
    {
        this.MaxHp = hp;
    }

    public float GetMaxHp() => MaxHp;

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

    public void DamageHp(int damage)
    {
        hp -= damage;
    }

    public void SetMoney(float money) 
    {
        this.money += money;
    }



    public float GetMoney() => money;
}
