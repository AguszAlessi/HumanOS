// CollisionVirus.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class CollisionVirus : MonoBehaviour
{
    public GameObject virus;
    public GameObject victoryPanel; // Panel con blur y mensaje
    public Slider virusHealthBar;   
    public float damageAmount = 10f;
    public float deathExtraDelay = 2.0f; // Segundos extra antes de desaparecer
    public float takeHitDelay = 0.5f; // Segundos de delay tras animación de golpe

    // Tiempo por defecto a esperar si no se encuentra el clip de muerte
    public float deathDelay = 1.0f;

    private Animator animator;
    private bool canTakeHit = true;
    private bool isDead = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }
    }

    // Cambia OnCollisionEnter por OnTriggerEnter para usar el trigger de la aguja
    private void OnTriggerEnter(Collider other)
    {
        // Solo la aguja (collider hijo) puede hacer daño
        // Recomendado: ponle un tag especial a la aguja, por ejemplo "NeedleTip"
        if (!isDead && other.CompareTag("NeedleTip"))
        {
            if (virusHealthBar != null && canTakeHit)
            {
                virusHealthBar.value -= damageAmount;

                if (virusHealthBar.value > 0)
                {
                    StartCoroutine(HandleTakeHit());
                }
                else
                {
                    isDead = true;
                    StartCoroutine(HandleDeath());
                }
            }
        }
    }

    private IEnumerator HandleTakeHit()
    {
        canTakeHit = false;
        TriggerAnimatorParameter("TakeHit");
        yield return new WaitForSeconds(takeHitDelay);
        canTakeHit = true;
    }

    private IEnumerator HandleDeath()
    {
        // Anula todas las animaciones activas excepto Death
        if (animator != null)
        {
            foreach (var p in animator.parameters)
            {
                if (p.name != "Death")
                {
                    if (p.type == AnimatorControllerParameterType.Bool)
                        animator.SetBool(p.name, false);
                    else if (p.type == AnimatorControllerParameterType.Int)
                        animator.SetInteger(p.name, 0);
                    else if (p.type == AnimatorControllerParameterType.Float)
                        animator.SetFloat(p.name, 0f);
                }
            }
        }
        // Dispara el parámetro de muerte
        TriggerAnimatorParameter("Death");

        // Intentar obtener la duración del clip de muerte si existe
        float wait = deathDelay;
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            foreach (var clip in animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name.ToLower().Contains("death"))
                {
                    wait = clip.length;
                    break;
                }
            }
        }

        // Espera la animación de muerte
        yield return new WaitForSeconds(wait);
        // Espera segundos extra antes de desaparecer
        yield return new WaitForSeconds(deathExtraDelay);

        // Si el panel está como hijo del virus, lo separo y lo activo
        Transform panel = transform.Find("CanvasPanel Variant");
        if (panel != null)
        {
            panel.SetParent(null);
            panel.gameObject.SetActive(true);
        }

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }

        Destroy(virus);
        Debug.Log("choque -> virus destruido tras animación de muerte");
    }

    // Método seguro que dispara un parámetro según su tipo real en el Animator
    private void TriggerAnimatorParameter(string paramName)
    {
        if (animator == null) return;

        foreach (var p in animator.parameters)
        {
            if (p.name == paramName)
            {
                if (p.type == AnimatorControllerParameterType.Trigger)
                {
                    animator.SetTrigger(paramName);
                }
                else if (p.type == AnimatorControllerParameterType.Bool)
                {
                    animator.SetBool(paramName, true);
                    // Resetear el bool para simular un trigger
                    StartCoroutine(ResetBoolParameter(paramName, 0.15f));
                }
                else if (p.type == AnimatorControllerParameterType.Int)
                {
                    animator.SetInteger(paramName, 1);
                }
                else if (p.type == AnimatorControllerParameterType.Float)
                {
                    animator.SetFloat(paramName, 1f);
                }
                break;
            }
        }
    }

    private IEnumerator ResetBoolParameter(string paramName, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (animator != null) animator.SetBool(paramName, false);
    }

    public void GoToMainMenu()
    {
        //SceneManager.LoadScene("MainMenu");
    }
}