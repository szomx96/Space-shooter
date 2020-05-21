using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Stats : MonoBehaviour
{

    public Texture2D hpText;
    public Texture2D staminaText;
    public FirstPersonController characterController;

    public float staminaRegen = 0.05f;
    public float staminaUsage = 0.5f;

    private float maxHp = 100;
    private float hp = 100;
    private float maxStamina = 100;
    private float stamina = 100;

    private float width;
    private float height;


    private void Awake()
    {
        characterController = GetComponent<FirstPersonController>();
        height = Screen.height * 0.03f;
        width = height * 10.0f;
    }

    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(10,
                          Screen.height - height - 10,
                          hp * width / maxHp,
                          height),
                        hpText);

        GUI.DrawTexture(new Rect(10,
                                 Screen.height - height * 2 - 20,
                                 stamina * width / maxStamina,
                                 height),
                        staminaText);        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        IncreaseStamina(staminaRegen);

        if (Input.GetKey(KeyCode.LeftShift) && (Input.GetKey(KeyCode.W)
            || Input.GetKey(KeyCode.A)
            || Input.GetKey(KeyCode.D)
            || Input.GetKey(KeyCode.S)))
        {
            DecreaseStamina(staminaUsage);

            if (stamina <= 0)
            {
                characterController.m_RunSpeed = characterController.m_WalkSpeed;
            }                  
        }
    }

    private void GetDamage(float damage)
    {
        hp -= damage;
        if (hp < 0) hp = 0;
    }

    private void Heal(float amout)
    {
        hp += amout;
        if (hp > maxHp) hp = maxHp;
    }

    private void DecreaseStamina(float amount)
    {
        stamina -= amount;
        if (stamina < 0) stamina = 0;
    }

    private void IncreaseStamina(float amount)
    {
        stamina += amount;
        if (stamina > maxStamina) stamina = maxStamina;
    }
}
