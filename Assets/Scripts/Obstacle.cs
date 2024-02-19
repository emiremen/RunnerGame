using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Obstacle : MonoBehaviour, IObstacle
{
    Tween shakeTween;


    void Start()
    {
        shakeTween = transform.DOShakeScale(0.5f, 0.5f).SetAutoKill(false).Pause();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Shake()
    {
        shakeTween.Restart();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Shake();
            PlayerManager player = EventManager.getPlayer?.Invoke();
            player.PlayerDeath();
        }
    }
}
