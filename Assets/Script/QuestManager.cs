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
        // �Ϸ簡 ���� ����Ʈ�� �ο��Ѵ�. new day�� �ҷ��ش�.
        _questType = (int)Character.instance.MyJob;// ����Ʈ�� �������� �ٸ��� �ο��ȴ�. 
        _questLevel = (int)Character.instance.MySocialClass;// ����Ʈ�� ��޺��� ���̵��� ��������.
        Character.instance.SetCharacterStat(4, 0); // todoProgress�� 0���� ����� �ش�.
    }

    public void QuestClear(int type, bool clear)
    {
        // ����Ʈ�� Ŭ�����ϰ� �θ� �Լ�
        Character.instance.SetCharacterStat(6,Character.instance.ActivePoint - 10); // Ȱ���� -10
        if(clear)
        {
            Debug.Log("����Ʈ ����");

            if (Character.instance.TodoProgress < 100)
            {
                Character.instance.SetCharacterStat(4, Character.instance.TodoProgress + 20); // todoProgress + 20
            }
            else
            {
                Character.instance.SetCharacterStat(type + 7, 2); // ���� ��                                                      
            }
        }
        else
        {
            Debug.Log("����Ʈ ����");
        }
    }
}
