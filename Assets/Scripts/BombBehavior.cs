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
        animator.SetInteger("BombState", 1);
    }
    public void Explote()
    {
        multiplierTxt.color = Color.red;
        sparksVFX.Stop();
        sparksVFX.Clear();
        animator.SetInteger("BombState", 2);
    }
    public void ResetBomb()
    {
        animator.SetInteger("BombState", 0);
        sparksVFX.Stop();
        sparksVFX.Clear();
        multiplierTxt.color = Color.white;
    }
}
