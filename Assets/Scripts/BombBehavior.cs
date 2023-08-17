using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BombBehavior : MonoBehaviour
{
    public TMP_Text multiplierTxt;
    [SerializeField] ParticleSystem sparksVFX;
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void LightUp()
    {
        sparksVFX.Play();
    }
    public void StartGameplay()
    {
        animator.SetBool("IsPlaying", true);
    }
    public void Explote()
    {
        multiplierTxt.color = Color.red;
        sparksVFX.Stop();
        animator.SetBool("IsPlaying", false);
    }
    public void ResetBomb()
    {
        animator.SetTrigger("ResetBomb");
        multiplierTxt.color = Color.white;
    }
}
