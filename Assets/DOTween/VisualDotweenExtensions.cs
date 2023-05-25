using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UIElements;

public static class VisualDotweenExtensions 
{
    public static int DOKill(this VisualElement target, bool complete = false) => DOTween.Kill((object) target, complete);
    public static int DOKill(this ITransform target, bool complete = false) => DOTween.Kill((object) target, complete);
    public static TweenerCore<Color, Color, ColorOptions> DOBackgroundColor(
        this VisualElement target,
        Color endValue,
        float duration)
    {
        TweenerCore<Color, Color, ColorOptions> t = DOTween.To((DOGetter<Color>) (() => target.style.backgroundColor.value),
            (DOSetter<Color>) (x => target.style.backgroundColor = new StyleColor(x)), endValue, duration);
        t.SetTarget<TweenerCore<Color, Color, ColorOptions>>((object) target);
        return t;
    }
    
    
    public static TweenerCore<float, float, FloatOptions> DORange(
        this Light target,
        float endValue,
        float duration)
    {
        TweenerCore<float, float, FloatOptions> t = DOTween.To((DOGetter<float>) (() => target.range),
            (DOSetter<float>) (x => target.range = x), endValue, duration);
        t.SetTarget<TweenerCore<float, float, FloatOptions>>((object) target);
        return t;
    }
    public static TweenerCore<float, float, FloatOptions> DOOpacity(
        this VisualElement target,
        float endValue,
        float duration)
    {
        TweenerCore<float, float, FloatOptions> t = DOTween.To((DOGetter<float>) (() => target.style.opacity.value),
            (DOSetter<float>) (x => target.style.opacity = x), endValue, duration);
        t.SetTarget<TweenerCore<float, float, FloatOptions>>((object) target);
        return t;
    }
    public static TweenerCore<Color, Color, ColorOptions> DOColor(
        this VisualElement target,
        Color endValue,
        float duration)
    {
        TweenerCore<Color, Color, ColorOptions> t = DOTween.To((DOGetter<Color>) (() => target.style.color.value),
            (DOSetter<Color>) (x => target.style.color = new StyleColor(x)), endValue, duration);
        t.SetTarget<TweenerCore<Color, Color, ColorOptions>>((object) target);
        return t;
    }
    public static TweenerCore<Vector3, Vector3, VectorOptions> DOScale(
        this ITransform target,
        Vector3 endValue,
        float duration)
    {
        TweenerCore<Vector3, Vector3, VectorOptions> t = DOTween.To((DOGetter<Vector3>) (() => target.scale),
            (DOSetter<Vector3>) (x => target.scale = x), endValue, duration);
        t.SetTarget<TweenerCore<Vector3, Vector3, VectorOptions>>((object) target);
        return t;
    }
    
    
    public static Tween DOScale(
        this Transform target,Vector3 midValue,
        Vector3 endValue, float duration)
    {
        Sequence s = DOTween.Sequence();
        TweenerCore<Vector3, Vector3, VectorOptions> pre_t = DOTween.To((DOGetter<Vector3>) (() => target.localScale),
            (DOSetter<Vector3>) (x => target.localScale = x),midValue, duration*0.65f).SetEase(Ease.OutQuad);
        pre_t.SetTarget<TweenerCore<Vector3, Vector3, VectorOptions>>((object) target);
        TweenerCore<Vector3, Vector3, VectorOptions> t = DOTween.To((DOGetter<Vector3>) (() => target.localScale),
            (DOSetter<Vector3>) (x => target.localScale = x), endValue, duration*0.35f).SetEase(Ease.InQuad);
        t.SetTarget<TweenerCore<Vector3, Vector3, VectorOptions>>((object) target);
        s.Append(pre_t);
        s.Append(t);
        return s;
    }
    
    public static Tween DOScale(
        this ITransform target,Vector3 midValue,
        Vector3 endValue, float duration)
    {
        Sequence s = DOTween.Sequence();
        TweenerCore<Vector3, Vector3, VectorOptions> pre_t = DOTween.To((DOGetter<Vector3>) (() => target.scale),
            (DOSetter<Vector3>) (x => target.scale = x),midValue, duration*0.65f).SetEase(Ease.OutQuad);
        pre_t.SetTarget<TweenerCore<Vector3, Vector3, VectorOptions>>((object) target);
        TweenerCore<Vector3, Vector3, VectorOptions> t = DOTween.To((DOGetter<Vector3>) (() => target.scale),
            (DOSetter<Vector3>) (x => target.scale = x), endValue, duration*0.35f).SetEase(Ease.InQuad);
        t.SetTarget<TweenerCore<Vector3, Vector3, VectorOptions>>((object) target);
        s.Append(pre_t);
        s.Append(t);
        return s;
    }

    public static Sequence DOJump(
        this ITransform target,
        Vector3 endValue,
        float jumpPower,
        int numJumps,
        float duration,
        bool snapping = false)
    {
        if (numJumps < 1)
            numJumps = 1;
        float startPosY = target.position.y;
        float offsetY = -1f;
        bool offsetYSet = false;
        Sequence s = DOTween.Sequence();
        Tween yTween = (Tween) DOTween
            .To((DOGetter<Vector3>) (() => target.position), (DOSetter<Vector3>) (x => target.position = x),
                new Vector3(0.0f, jumpPower, 0.0f), duration / (float) (numJumps * 2))
            .SetOptions(AxisConstraint.Y, snapping).SetEase<Tweener>(Ease.OutQuad).SetRelative<Tweener>()
            .SetLoops<Tweener>(numJumps * 2, LoopType.Yoyo)
            .OnStart<Tweener>((TweenCallback) (() => startPosY = target.position.y));
        s.Append((Tween) DOTween
                .To((DOGetter<Vector3>) (() => target.position), (DOSetter<Vector3>) (x => target.position = x),
                    new Vector3(endValue.x, 0.0f, 0.0f), duration).SetOptions(AxisConstraint.X, snapping)
                .SetEase<Tweener>(Ease.Linear))
            .Join((Tween) DOTween
                .To((DOGetter<Vector3>) (() => target.position), (DOSetter<Vector3>) (x => target.position = x),
                    new Vector3(0.0f, 0.0f, endValue.z), duration).SetOptions(AxisConstraint.Z, snapping)
                .SetEase<Tweener>(Ease.Linear)).Join(yTween).SetTarget<Sequence>((object) target)
            .SetEase<Sequence>(DOTween.defaultEaseType);
        yTween.OnUpdate<Tween>((TweenCallback) (() =>
        {
            if (!offsetYSet)
            {
                offsetYSet = true;
                offsetY = s.isRelative ? endValue.y : endValue.y - startPosY;
            }

            Vector3 position = target.position;
            position.y += DOVirtual.EasedValue(0.0f, offsetY, yTween.ElapsedPercentage(), Ease.OutQuad);
            target.position = position;
        }));
        return s;
    }

    public static TweenerCore<Vector3, Vector3, VectorOptions> DOMove(
        this ITransform target,
        Vector3 endValue,
        float duration,
        bool snapping = false)
    {
        TweenerCore<Vector3, Vector3, VectorOptions> t = DOTween.To((DOGetter<Vector3>) (() => target.position), (DOSetter<Vector3>) (x => target.position = x), endValue, duration);
        t.SetOptions(snapping).SetTarget<Tweener>((object) target);
        return t;
    }
    public static Tweener DOPunchScale(
        this ITransform target,
        Vector3 punch,
        float duration,
        int vibrato = 10,
        float elasticity = 1f)
    {
        if ((double) duration > 0.0)
            return (Tweener) DOTween.Punch((DOGetter<Vector3>) (() => target.scale), (DOSetter<Vector3>) (x => target.scale = x), punch, duration, vibrato, elasticity).SetTarget<TweenerCore<Vector3, Vector3[], Vector3ArrayOptions>>((object) target);
        if (Debugger.logPriority > 0)
            Debug.LogWarning((object) "DOPunchScale: duration can't be 0, returning NULL without creating a tween");
        return (Tweener) null;
    }
    
    public static Tweener DOShakePosition(
        this ITransform target,
        float duration,
        float strength = 1f,
        int vibrato = 10,
        float randomness = 90f,
        bool snapping = false,
        bool fadeOut = true)
    {
        if ((double) duration > 0.0)
            return DOTween.Shake((DOGetter<Vector3>) (() => target.position), (DOSetter<Vector3>) (x => target.position = x), duration, strength, vibrato, randomness, false, fadeOut).SetTarget<TweenerCore<Vector3, Vector3[], Vector3ArrayOptions>>((object) target).SetSpecialStartupMode<TweenerCore<Vector3, Vector3[], Vector3ArrayOptions>>(SpecialStartupMode.SetShake).SetOptions(snapping);
        if (Debugger.logPriority > 0)
            Debug.LogWarning((object) "DOShakePosition: duration can't be 0, returning NULL without creating a tween");
        return (Tweener) null;
    }
    
    public static Tweener DOShakeScale(
        this ITransform target,
        float duration,
        float strength = 1f,
        int vibrato = 10,
        float randomness = 90f,
        bool fadeOut = true)
    {
        if ((double) duration > 0.0)
            return (Tweener) DOTween.Shake((DOGetter<Vector3>) (() => target.scale), (DOSetter<Vector3>) (x => target.scale = x), duration, strength, vibrato, randomness, false, fadeOut).SetTarget<TweenerCore<Vector3, Vector3[], Vector3ArrayOptions>>((object) target).SetSpecialStartupMode<TweenerCore<Vector3, Vector3[], Vector3ArrayOptions>>(SpecialStartupMode.SetShake);
        Debug.Log((object) Debugger.logPriority);
        if (Debugger.logPriority > 0)
            Debug.LogWarning((object) "DOShakeScale: duration can't be 0, returning NULL without creating a tween");
        return (Tweener) null;
    }

}
