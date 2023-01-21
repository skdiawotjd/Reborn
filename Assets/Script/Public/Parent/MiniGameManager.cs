using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class MiniGameManager : MonoBehaviour
{
    protected int GameType;
    protected bool panelActiveSelf;
    protected UIFrameWorkManager FrameWorkManager;
    protected virtual void Start()
    {
        Character.instance.transform.position = new Vector3(0f, 0f, 0f);
        FrameWorkManager = GameObject.Find("UIMinigameManager").GetComponent<UIFrameWorkManager>();
    }
    public abstract void GameStart();
    public abstract void GameEnd(bool clear);
    void Update()
    {
        
    }

    public virtual void SetRound(int num) { }
    public virtual void SetGame() { }
    public virtual void SetMainWork() { }
    public virtual void SetMainWork(int num) { }
    
    public virtual void Generate() { }
    public virtual void PressKey(int num) { }
    public int GetGameType() { return GameType; }
    public void SetPanelActive(bool active)
    {
        panelActiveSelf = active;
    }
    public virtual IEnumerator CountTime(float delayTime) { yield return new WaitForSeconds(delayTime); }
    public virtual IEnumerator ChangeTimingValue(float delayTime) { yield return new WaitForSeconds(delayTime); }
}
