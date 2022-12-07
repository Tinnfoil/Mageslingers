using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUI : MonoBehaviour
{
    private Vector3 originalPosition;
    public bool IsOn;
    public virtual void Awake()
    {
        IntializeUI();
    }

    public virtual void Toggle(float time = 1)
    {
        Toggle(!IsOn, time);
    }
    public virtual void Toggle(bool enable, float time = 0)
    {
        IsOn = enable;
        if (IsOn)
        {
            ToggleOn(time);
        }
        else
        {
            ToggleOff(time);
        }
    }

    public virtual void ToggleOn(float time = 0)
    {
        InsertFromRight(time);
    }
    public virtual void ToggleOff(float time = 0)
    {
        ExtractToRight(time);
    }

    public virtual void InsertFromRight(float time = 1)
    {
        Insert(Direction.RIGHT, time);
    }
    public virtual void ExtractToRight(float time = 1)
    {
        Extract(Direction.RIGHT, time);
    }


    public virtual void ReturnToOrignalPosition(float time = 1)
    {
        Extract(time);
    }

    public virtual void Insert(Direction dir, float time)
    {
        if (time == 0)
        {
            gameObject.transform.localPosition = originalPosition + CalculateDirection(dir);
        }
        else
        {
            LeanTween.cancel(gameObject);
            LeanTween.moveLocal(gameObject, originalPosition + CalculateDirection(dir), time).setEaseInOutCubic();
            gameObject.SetActive(true);
            LeanTween.delayedCall(gameObject, time, () => EnableUI());
        }
    }

    public virtual void Extract(float time)
    {
        Extract(originalPosition, time);
    }

    public virtual void Extract(Direction dir, float time)
    {
        Extract(transform.localPosition - CalculateDirection(dir), time);
    }

    public virtual void Extract(Vector3 targetPosition, float time)
    {
        if (time == 0)
        {
            gameObject.transform.localPosition = targetPosition;
        }
        else
        {
            LeanTween.cancel(gameObject);
            LeanTween.moveLocal(gameObject, targetPosition, time).setEaseInOutCubic();
            gameObject.SetActive(true);
            LeanTween.delayedCall(gameObject, time, () => DisableUI());
        }
    }

    public virtual void EnableUI()
    {
        gameObject.SetActive(true);
    }

    public virtual void DisableUI()
    {
        gameObject.SetActive(false);
    }

    public virtual void IntializeUI()
    {
        originalPosition = transform.localPosition;
        Toggle(IsOn, 0);
        originalPosition = transform.localPosition;
    }

    public Vector3 CalculateDirection(Direction dir)
    {
        switch (dir)
        {
            case Direction.UP:
                return -Vector2.up * GetComponent<RectTransform>().sizeDelta.y;
            case Direction.RIGHT:
                return -Vector2.right * GetComponent<RectTransform>().sizeDelta.x;
            case Direction.DOWN:
                return Vector2.up * GetComponent<RectTransform>().sizeDelta.y;
            case Direction.LEFT:
                return Vector2.right * GetComponent<RectTransform>().sizeDelta.x;
        }
        return Vector2.right;
    }

    public enum Direction
    {
        UP,
        RIGHT,
        DOWN,
        LEFT
    }
}
