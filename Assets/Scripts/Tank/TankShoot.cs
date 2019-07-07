using UnityEngine;

public partial class Tank : MonoBehaviour
{
    public Rigidbody2D m_Shell;                   // Prefab of the shell.
    public Transform fireTransform;

    protected float m_ChargeTime;
    protected float m_ShellSpeed = 3;
    protected int m_ShootDamage = 1;
    protected float m_CurrentChargeTime = 0;

    protected void Fire(int damage)
    {
        GameObject shellInstance = ObjectPool.GetInstance().GetObject("Shell");
        shellInstance.transform.position = fireTransform.position;
        shellInstance.transform.rotation = fireTransform.rotation;
        Shell shell = shellInstance.GetComponent<Shell>();
        shell.GetComponent<Rigidbody2D>().velocity = m_ShellSpeed * moveDirection;
        shell.shooter = m_PlayerNumber;
        shell.damage = damage;

    }
}
