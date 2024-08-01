using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// An attempt to extend the events of the UI slider
/// </summary>
public class ExtendedSlider : Slider
{
    // Extended Events
    public UnityEvent onHighlight;
    public UnityEvent onUnhighlight;
    public UnityEvent onValueChangeHold;
    public UnityEvent onValueChangeRelease;

    // Bool to check for highlighting
    private bool isHighlighted;

    // Will trigger with keyboard/gamepad inputs
    #region Keyed Inputs

    // Triggers on highlight and selected
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        // If it wasnt highlighted before, Invoke Event
        if (!isHighlighted)
        {
            isHighlighted = true;
            onHighlight.Invoke();
        }
    }

    // Triggers on de-highlight and de-selected
    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        // If it wasnt de-highlighted before, Invoke Event
        if (isHighlighted)
        {
            isHighlighted = false;
            onUnhighlight.Invoke();
        }
    }

    #endregion

    #region Pointer Inputs
    // Will trigger with mouse or a pointer

    // Triggers on highlight and selected
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        // If it wasnt highlighted before, Invoke Event
        if (!isHighlighted)
        {
            isHighlighted = true;
            onHighlight.Invoke();
        }
    }

    // Triggers on de-highlight and de-selected
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        // If it wasnt de-highlighted before, Invoke Event
        if (isHighlighted)
        {
            isHighlighted = false;
            onUnhighlight.Invoke();
        }
    }

    // Fires only once at the start of the mouse dragging
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        onValueChangeHold.Invoke();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        onValueChangeRelease.Invoke();
    }

    #endregion

}
