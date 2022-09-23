using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public int QuestLeve
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
    public void QuestGive()
    {
        // 하루가 지나 퀘스트를 부여한다. new day에 불러준다.
        _questType = (int)Character.instance.MyJob;// 퀘스트는 직업별로 다르게 부여된다. 
        _questLevel = (int)Character.instance.MySocialClass;// 퀘스트는 계급별로 난이도가 정해진다.
        Character.instance.SetCharacterStat(4, 0); // todoProgress를 0으로 만들어 준다.
    }

    public void QuestClear(int type, bool clear)
    {
        // 퀘스트를 클리어하고 부를 함수
        Character.instance.SetCharacterStat(6,Character.instance.ActivePoint - 10); // 활동력 -10
        if(clear)
        {
            Debug.Log("퀘스트 성공");

            if (Character.instance.TodoProgress < 100)
            {
                Character.instance.SetCharacterStat(4, Character.instance.TodoProgress + 20); // todoProgress + 20
            }
            else
            {
                Character.instance.SetCharacterStat(type + 7, 2); // 스택 업                                                      
            }
        }
        else
        {
            Debug.Log("퀘스트 실패");
        }
    }
}
