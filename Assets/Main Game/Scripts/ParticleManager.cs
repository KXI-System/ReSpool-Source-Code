using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ParticleManager : MonoBehaviour
{
    public float minSpeed;
    public float maxSpeed;
    public float transitionTime;
    public float holdTime;
    public Ease easeType;

    [Space]

    public ParticleSystem MainSystem;

    [ContextMenu("Change Speed")]
    public void SpeedAndSlow() 
    {
        var main = MainSystem.main;

        Sequence myseq = DOTween.Sequence();
        myseq
            .Append(DOTween.To(() => main.simulationSpeed, x => main.simulationSpeed = x, maxSpeed, transitionTime).SetEase(easeType))
            .AppendInterval(holdTime)
            .Append(DOTween.To(() => main.simulationSpeed, x => main.simulationSpeed = x, minSpeed, transitionTime).SetEase(easeType));
    }

    public void SpeedUp() 
    {
        var main = MainSystem.main;
        DOTween.To(() => main.simulationSpeed, x => main.simulationSpeed = x, maxSpeed, transitionTime).SetEase(easeType);
    }

    public void SlowDown() 
    {
        var main = MainSystem.main;
        DOTween.To(() => main.simulationSpeed, x => main.simulationSpeed = x, minSpeed, transitionTime).SetEase(easeType);
    }
}
