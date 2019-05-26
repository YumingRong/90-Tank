using UnityEngine;

public partial class Tank : MonoBehaviour
{
    public Rigidbody2D m_Shell;                   // Prefab of the shell.
    public Transform fireTransform;

    protected float m_ChargeTime;
    protected float m_ShellSpeed = 3;
    protected int m_ShootDamage = 1;
    protected float m_CurrentChargeTime = 0;

    protected ObjectPool shellPool = ObjectPool.GetInstance();

    protected void Fire()
    {
        // Create an instance of the shell and store a reference to it's rigidbody.
        GameObject shellInstance = shellPool.GetObject(fireTransform.position, fireTransform.rotation);
        //Rigidbody2D shellInstance =
        //    Instantiate(m_Shell, fireTransform.position, fireTransform.rotation) as Rigidbody2D;

        // Set the shell's velocity to the launch force in the fire position's forward direction.
        Shell shell = shellInstance.GetComponent<Shell>();
        shell.GetComponent<Rigidbody2D>().velocity = m_ShellSpeed * moveDirection;
        shell.shooter = m_PlayerNumber;
        shell.damage = m_ShootDamage;

    }
}
