using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Coin : MonoBehaviour, ICollectable
{

    [SerializeField] private ParticleSystem collectEffect;
    [SerializeField] private Animator animator;

    private Collider coinCollider;


    IEnumerator Start()
    {
        coinCollider = GetComponent<Collider>();
        yield return new WaitForSeconds(Random.Range(0, 0.5f));
        animator.Play("CoinRotation");
    }

    public void Collect()
    {
        EventManager.updateCoinScore?.Invoke();
        collectEffect.Play();
        EventManager.playAudio?.Invoke(0);
        transform.DOScale(Vector3.zero, 0.01f).OnComplete(() =>
        {
            transform.DOScale(Vector3.one, 1f).SetDelay(5f).OnComplete(() =>
            {
                coinCollider.enabled = true;
                animator.Play("CoinRotation");
            });
        });
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            coinCollider.enabled = false;
            Collect();
        }
    }
}
