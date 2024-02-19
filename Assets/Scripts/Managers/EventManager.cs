using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class EventManager
{
    public static Func<PlayerManager> getPlayer;
    public static Func<Vector3> getPosition;
    public static Action increasePlayerSpeed;
    public static Func<CinemachineVirtualCamera> getCinemachine;
    public static Func<string, GameObject> callObjectFromPool;
    public static Func<GameDataSO> getGameDataSO;

    public static Action showGameOverPanel;
    public static Action startGame;

    public static Action updateCoinScore;
    internal static Action<int> playAudio;
    internal static Action<int, float> playAudioWithVolume;
}
