using DG.Tweening;
using UnityEngine;

public static class DotweenInit 
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init() => DOTween.Init();
}