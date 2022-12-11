using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIManager : MonoBehaviour
{
    [SerializeField]
    protected GameObject Panel;

    protected virtual void Start()
    {
        GameManager.instance.AddDayStart(StartUI);
        GameManager.instance.AddDayEnd(EndUI);
    }

    public virtual void SetActivePanel()
    {
        Panel.SetActive(!Panel.activeSelf);
    }
    public virtual void SetActivePanel(bool Active)
    {
        Panel.SetActive(Active);
    }

    protected abstract void StartUI();
    protected abstract void EndUI();
}
