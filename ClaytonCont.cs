using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ClaytonCont : MonoBehaviour
{
    public StaminaBar sBar;
    public HealthBar HPBar;
    public Manabar MPBar;
   
     
    public float jumpHeight = 50f;

    public static float walkSpeedV = 3f ;
    public static float walkSpeedH = 1f;

    public float runSpeedV = walkSpeedV * 2 ;
    public float runSpeedH = walkSpeedH * 2;

    public bool drainingStam;

    public int stamDrain = 5;
    
    Animator anim;
   
    CharacterController controller;

    public Transform lookatme;

    public int rollRecovery = 3;

    public bool IsDodging;

    public int rollCost = 5;




    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    
    }



    void Update()
    {
        Movement();
        Jump();
        MouseLook();
        DodgeRoll();
        DamageStats();

    }
    
    void Jump()
    {
       if(Input.GetAxis("Jump") != 0 && controller.isGrounded)
       
        {
                controller.Move(Vector3.up*jumpHeight*Time.deltaTime);
        }
    }
    void Movement()
    {
        Vector2 anymove = new Vector2 (Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (!Input.GetKey(KeyCode.LeftShift) )
        {

            if (Input.GetAxisRaw("Vertical") > 0.1)
            {
                controller.Move(transform.forward * walkSpeedV* Time.deltaTime);
            }
            if (Input.GetAxisRaw("Vertical") < -0.1)
            {
                controller.Move(transform.forward * -walkSpeedV * Time.deltaTime);
                anim.SetFloat("StrafeDir", 0.2f);
            }
            if (Input.GetAxisRaw("Horizontal") < -0.1)
            {
                controller.Move(transform.right * -walkSpeedH * Time.deltaTime);
                anim.SetFloat("StrafeDir", 0.6f); 
            }
            if (Input.GetAxisRaw("Horizontal") > 0.1)
            {
                controller.Move(transform.right * walkSpeedH * Time.deltaTime);
                anim.SetFloat("StrafeDir", 0.4f);
            }

        }
        if (Input.GetKey(KeyCode.LeftShift) && sBar.getCurrentStamValue < stamDrain)
        {

            if (Input.GetAxisRaw("Vertical") > 0.1)
            {
                controller.Move(transform.forward * walkSpeedV * Time.deltaTime);
            }
            if (Input.GetAxisRaw("Vertical") < -0.1)
            {
                controller.Move(transform.forward * -walkSpeedV * Time.deltaTime);
                anim.SetFloat("StrafeDir", 0.2f);
            }
            if (Input.GetAxisRaw("Horizontal") < -0.1)
            {
                controller.Move(transform.right * -walkSpeedH * Time.deltaTime);
                anim.SetFloat("StrafeDir", 0.6f);
            }
            if (Input.GetAxisRaw("Horizontal") > 0.1)
            {
                controller.Move(transform.right * walkSpeedH * Time.deltaTime);
                anim.SetFloat("StrafeDir", 0.4f);
            }

        }
       else if (Input.GetKey(KeyCode.LeftShift) && sBar.getCurrentStamValue >= stamDrain )
        {
          
            if (Input.GetAxisRaw("Vertical") > 0.1)
            {
                controller.Move(transform.forward * runSpeedV * Time.deltaTime);
            }
            if (Input.GetAxisRaw("Vertical") < -0.1)
            {
                controller.Move(transform.forward * -runSpeedV * Time.deltaTime);
                anim.SetFloat("StrafeDir", 0.2f);
            }
            if (Input.GetAxisRaw("Horizontal") < -0.1)
            {
                controller.Move(transform.right * -runSpeedH * Time.deltaTime);
                anim.SetFloat("StrafeDir", 0.6f);
            }
            if (Input.GetAxisRaw("Horizontal") > 0.1)
            {
                controller.Move(transform.right * runSpeedH * Time.deltaTime);
                anim.SetFloat("StrafeDir", 0.4f);
            }
           
            //drain stam on run


            if (anymove != Vector2.zero && Input.GetKey(KeyCode.LeftShift) && !drainingStam  )
            {
                StartCoroutine(RunningDrain());
            }
            IEnumerator RunningDrain()
            {
                drainingStam = true;
                sBar.SetStam(sBar.getCurrentStamValue -  stamDrain);
                yield return new WaitForSeconds(1);
                drainingStam = false;

            }
        }
       


        Vector2 inputMove = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
        if (inputMove.x > 0.1)
        {
            anim.SetFloat("SpeedPercent", 0.5f);

        }
        if(Input.GetKey(KeyCode.LeftShift) && sBar.getCurrentStamValue >= stamDrain && inputMove.x > 0.1)
        {
            anim.SetFloat("SpeedPercent", 1f);
        }

        if (inputMove.x == 0 )
        {
            anim.SetFloat("SpeedPercent", 0);

        }
        if(inputMove.y == 0 && inputMove.x >= 0)
        {
            anim.SetFloat("StrafeDir", 0f);
        }
    }
    void MouseLook()
    {
        Vector3 targetPosition = new Vector3(lookatme.transform.position.x,
                                                 transform.position.y,
                                                 lookatme.transform.position.z);
        transform.LookAt(targetPosition);
    }

    //combined all the roll directions
    void DodgeRoll()
    {

        RollF();
        RollB();
        RollL();
        RollR();


    }
    //calling enums for each input
    public void RollF()
    {
        if (Input.GetKey(KeyCode.LeftControl) & sBar.getCurrentStamValue > 11 & Input.GetAxis("Vertical") > 0 & !IsDodging)
        {
            StartCoroutine(Rollf());

        }



        else
            anim.SetBool("RollF", false);

        return;
    }
    public void RollB()
    {
        if (Input.GetKey(KeyCode.LeftControl) & sBar.getCurrentStamValue > 11 & Input.GetAxis("Vertical") < 0 & !IsDodging)
        {
            StartCoroutine(Rollb());

        }



        else

            anim.SetBool("RollB", false);

        return;
    }
    public void RollL()
    {
        if (Input.GetKey(KeyCode.LeftControl) & sBar.getCurrentStamValue > 11 & Input.GetAxis("Horizontal") < 0 & !IsDodging)
        {
            StartCoroutine(Rolll());

        }



        else

            anim.SetBool("RollL", false);

        return;
    }
    public void RollR()
    {
        if (Input.GetKey(KeyCode.LeftControl) & sBar.getCurrentStamValue > 11 & Input.GetAxis("Horizontal") > 0 & !IsDodging)
        {
            StartCoroutine(Rollr());

        }



        else

            anim.SetBool("RollR", false);

        return;
    }
    //enums to stop spam loss of stam and rolling anim
    IEnumerator Rollf()
    {
        IsDodging = true;
        anim.SetBool("RollF", true);
        sBar.SetStam(sBar.getCurrentStamValue - rollCost);
        yield return new WaitForSeconds(rollRecovery);
        IsDodging = false;
        anim.SetBool("RollF", false);
    }
    IEnumerator Rollb()
    {
        IsDodging = true;
        anim.SetBool("RollB", true);
        sBar.SetStam(sBar.getCurrentStamValue - rollCost);
        yield return new WaitForSeconds(rollRecovery);
        IsDodging = false;
        anim.SetBool("RollB", false);
    }
    IEnumerator Rolll()
    {
        IsDodging = true;
        anim.SetBool("RollL", true);
        sBar.SetStam(sBar.getCurrentStamValue - rollCost);
        yield return new WaitForSeconds(rollRecovery);
        IsDodging = false;
        anim.SetBool("RollL", false);
    }
    IEnumerator Rollr()
    {
        IsDodging = true;
        anim.SetBool("RollR", true);
        sBar.SetStam(sBar.getCurrentStamValue - rollCost);
        yield return new WaitForSeconds(rollRecovery);
        IsDodging = false;
        anim.SetBool("RollR", false);
    }
    void DamageStats()
    {
        if (Input.GetKey(KeyCode.L))
        {
            HPBar.SetHP(5);
            sBar.SetStam(5);
            MPBar.SetMP(5);
        }
        
    }








}
