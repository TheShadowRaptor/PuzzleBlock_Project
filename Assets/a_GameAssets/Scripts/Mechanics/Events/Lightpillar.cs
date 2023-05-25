using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public interface LighListener {
    public void LightHasChanged();
}
public class Lightpillar : EventObject, ICellOccupier {
    public static List<LighListener> lightListeners = new List<LighListener>();

    public Transform GemTransform;
    public bool IsSolid { get => true; set { } }
    [SerializeField] float maxLightRange;
    [SerializeField] float minLightRange;
    [FormerlySerializedAs("speed")] [SerializeField] private float duration = 2;
    public Ease easeType = Ease.Linear;

    private Light light;
    private Transform lightSize;

    bool playEvent = false;
    // Start is called before the first frame update
    protected override void Start()
    {
        light = GetComponentInChildren<Light>();
        lightSize = light.transform;
        AnimateOff();
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        GemTransform.DOKill();
        light.DOKill();
        light.transform.DOKill();
    }

    public float rotationDurationOn=0.5f,rotationDurationOff=0.5f;
    public override void PlayEvent()
    {
        playEvent = true;
        ChangeRange(maxLightRange);
        AnimateOn();
    }

    private void AnimateOn()
    {
        GemTransform.DOKill(false);
        GemTransform.DOLocalRotate(new Vector3(0, 10, 0), rotationDurationOn).SetRelative(true).SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental);
        GemTransform.DOLocalMoveY(1.8f, 0.5f).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            GemTransform.DOLocalMoveY(1.7f, 1).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
        });
    }
    
    public override void CancelEvent()
    {
        playEvent = false;
        ChangeRange(minLightRange);
        AnimateOff();
    }

    private void AnimateOff()
    {
        GemTransform.DOKill(false);
        GemTransform.DOLocalRotate(new Vector3(0, 10, 0), rotationDurationOff).SetRelative(true).SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental);
        GemTransform.DOLocalMoveY(1.2f, 1.25f).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            GemTransform.DOLocalMoveY(1.3f, 1).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
        });
    }
    
    private void ChangeRange(float newRange)
    {
        light.DOKill();
        lightSize.transform.DOKill();
        light.DORange(newRange, duration).SetEase(easeType).OnUpdate(OnLightRangeUpdate).OnComplete(BroadcastToListeners);
        
        lightSize.transform.DOScale(newRange, duration).SetEase(easeType);
    }

    private float lastSentLightRangeUpdate;
    private void OnLightRangeUpdate() {
        if (!(lastSentLightRangeUpdate + 0.33f < Time.time)) return;
        lastSentLightRangeUpdate = Time.time;
        BroadcastToListeners();
    }
    
    private void BroadcastToListeners() {
        for (int i = 0; i < lightListeners.Count; i++)
            lightListeners[i].LightHasChanged();
    }


    public virtual Vector3 GetPosition() { return transform.position; }

    public void BlockEnteredHere(BlockCharacter entered, Vector3Int dir)
    {
       
    }

    public void BlockExitHere(BlockCharacter exited)
    {

    }

    public void OnBlockMoveAttemptFail(BlockCharacter attempt)
    {
        if (!attempt.CanInteract()) return;
        attempt.Interact();
        //Debug.Log("Attempted");
        if (playEvent == false) PlayEvent();
        else CancelEvent();
    }
}
