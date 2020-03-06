using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimPlay : MonoBehaviour
{
    public Animator Asel_Run_Anim;
    
    public void Start()
    {
        Asel_Run_Anim = gameObject.GetComponent<Animator>();
        //Asel_Animation = gameObject.GetComponent<Animation>();
    }

    public void RunStart()
    {
        //Asel_Animation.wrapMode = WrapMode.Loop;
        Asel_Run_Anim.SetBool("walk", true);
    }
    public void JumpStart()
    {
        Asel_Run_Anim.SetBool("jump", true);
    }
    public void Stop()
    {
        Asel_Run_Anim.SetBool("jump", false);
        Asel_Run_Anim.SetBool("walk", false);
        Asel_Run_Anim.SetBool("Fall", false);
        Asel_Run_Anim.SetBool("Teleport", false);
        
    }

    public void teleport()
    {
        Asel_Run_Anim.SetBool("Teleport", true);
    }

    public void fall()
    {
        Asel_Run_Anim.SetBool("Fall", true);
    }
}
