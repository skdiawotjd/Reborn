using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestManager : MonoBehaviour
{
    private int _questType;
    private int _questLevel;

    public static QuestManager instance = null;

    public int questType
    {
        get
        {
            return _questType;
        }
    }
    public int QuestLevel
    {
        get
        {
            return _questLevel;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }
    public UnityEvent EventCountChange;

    public void QuestGive()
    {
        // 하루가 지나 퀘스트를 부여한다. new day에 불러준다.
        _questType = (int)Character.instance.MyJob;// 퀘스트는 직업별로 다르게 부여된다. 
        _questLevel = (int)Character.instance.MySocialClass;// 퀘스트는 계급별로 난이도가 정해진다.
        // Character.instance.SetCharacterStat(4, 0); // todoProgress를 0으로 만들어 준다.
    }

    public void QuestClear(int type, bool clear)
    {
        // 퀘스트를 클리어하고 부를 함수
        Debug.Log("ActivePoint 10 감소하기 전, 현재 수치 : " + Character.instance.ActivePoint);
        Character.instance.SetCharacterStat(7,Character.instance.ActivePoint - 50); // 활동력 -10
        Debug.Log("ActivePoint 10 감소, 현재 수치 : " + Character.instance.ActivePoint);

        if (clear)
        {
            Debug.Log("퀘스트 성공");

            if (Character.instance.TodoProgress < 100)
            {
                Debug.Log("TodoProgress 2 증가하기 전, 현재 수치 : " + Character.instance.TodoProgress);
                Character.instance.SetCharacterStat(4, 2); // todoProgress + 2
                Debug.Log("TodoProgress 2 증가, 현재 수치 : " + Character.instance.TodoProgress);
            }
            else
            {
                Character.instance.SetCharacterStat(type + 8, 2); // 스택 업
            }
        }
        else
        {
            Debug.Log("퀘스트 실패");
        }
    }


    public void BoxCount()
    {
        EventCountChange.Invoke();
    }
}
