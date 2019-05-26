using UnityEngine;

public partial class Tank : MonoBehaviour
{
    public Rigidbody2D m_Shell;                   // Prefab of the shell.
    public AudioSource m_ShootingAudio;         // Reference to the audio source used to play the shooting audio. NB: different to the movement audio source.
    public AudioClip m_FireClip;                // Audio that plays when each shot is fired.
    public float m_ChargeTime;
    public Transform fireTransform;

    protected float m_ShellSpeed = 3;
    protected int m_ShootDamage = 1;
    protected float m_CurrentChargeTime = 0;
    protected Vector2 moveDirection = new Vector2(0, 1);

    protected void Fire()
    {
        // Create an instance of the shell and store a reference to it's rigidbody.
        Rigidbody2D shellInstance =
            Instantiate(m_Shell, fireTransform.position, fireTransform.rotation) as Rigidbody2D;

        // Set the shell's velocity to the launch force in the fire position's forward direction.
        shellInstance.velocity = m_ShellSpeed * moveDirection;

        Shell shell = shellInstance.GetComponent<Shell>();
        shell.shooter = m_PlayerNumber;
        shell.damage = m_ShootDamage;

        // Change the clip to the firing clip and play it.
        //m_ShootingAudio.clip = m_FireClip;
        //m_ShootingAudio.Play();

    }
}
