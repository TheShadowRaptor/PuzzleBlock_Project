using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class UIExtensions
{
    public static void SetDisplayBasedOnBool(this VisualElement vs, bool b)
    {
        vs.style.display =
            b ? new StyleEnum<DisplayStyle>(DisplayStyle.Flex) : new StyleEnum<DisplayStyle>(DisplayStyle.None);
    }

    public static void SetDisplayAndOpacityBasedOnBool(this VisualElement vs, bool b)
    {
        vs.style.display =
            b ? new StyleEnum<DisplayStyle>(DisplayStyle.Flex) : new StyleEnum<DisplayStyle>(DisplayStyle.None);
        vs.style.opacity = b ? 1 : 0;
    }
}
