using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour, IPlayer
{
    public abstract float Hp { get; set; }
    public abstract float MaxHp { get; set; }
    public abstract float Speed { get; set; }

    virtual public void GetDmg(float dmg)
    {
        if (Hp - dmg <= 0)
        {
            Hp = 0;
            GameOver();
        }
        else
        {
            Hp -= dmg;
        }
    }

    virtual public void GameOver()
    {
        if (gameObject.CompareTag("Player"))
            Debug.Log("GameOver");
        //w ramach debugu destroy jest w else, powinien byc niszczony obiekt niewazne czy to gracz czy nie
        if (gameObject.CompareTag("Enemy"))
            Destroy(gameObject);
    }
}
