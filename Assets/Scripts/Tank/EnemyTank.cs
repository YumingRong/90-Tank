using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : Tank
{
    // Start is called before the first frame update
    void Start()
    {
        m_PlayerNumber = -1;
        moveDirection = Vector2.down;
        m_ChargeTime = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        m_CurrentChargeTime += Time.deltaTime;
        if (m_CurrentChargeTime >= m_ChargeTime)
        {
            //Fire();
            m_CurrentChargeTime = 0f;
        }
    }
}
