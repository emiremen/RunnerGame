using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameDataSO gameDataSO;
    [SerializeField] private Material[] curvedMaterials;
    [SerializeField] private int curveTime = 5;
    Tween curveTween;


    void OnEnable()
    {
        EventManager.getGameDataSO += GetGameDataSO;
    }

    void OnDisable()
    {
        EventManager.getGameDataSO -= GetGameDataSO;
    }

    void Start()
    {
        StartCoroutine(nameof(RandomizeWorldCurve));
    }

    IEnumerator RandomizeWorldCurve()
    {
        float randomCurveX = Random.Range(-0.003f, 0.003f);
        foreach (var material in curvedMaterials)
        {
            curveTween = material.DOFloat(randomCurveX, "_CurveX", curveTime).SetEase(Ease.Linear);
        }
        yield return curveTween.WaitForCompletion();
        StartCoroutine(nameof(RandomizeWorldCurve));
    }

    private GameDataSO GetGameDataSO()
    {
        return gameDataSO;
    }
}
