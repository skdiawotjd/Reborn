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
        // �Ϸ簡 ���� ����Ʈ�� �ο��Ѵ�. new day�� �ҷ��ش�.
        _questType = (int)Character.instance.MyJob;// ����Ʈ�� �������� �ٸ��� �ο��ȴ�. 
        _questLevel = (int)Character.instance.MySocialClass;// ����Ʈ�� ��޺��� ���̵��� ��������.
        // Character.instance.SetCharacterStat(4, 0); // todoProgress�� 0���� ����� �ش�.
    }

    public void QuestClear(int type, bool clear)
    {
        // ����Ʈ�� Ŭ�����ϰ� �θ� �Լ�
        Debug.Log("ActivePoint 10 �����ϱ� ��, ���� ��ġ : " + Character.instance.ActivePoint);
        Character.instance.SetCharacterStat(7,Character.instance.ActivePoint - 50); // Ȱ���� -10
        Debug.Log("ActivePoint 10 ����, ���� ��ġ : " + Character.instance.ActivePoint);

        if (clear)
        {
            Debug.Log("����Ʈ ����");

            if (Character.instance.TodoProgress < 100)
            {
                Debug.Log("TodoProgress 2 �����ϱ� ��, ���� ��ġ : " + Character.instance.TodoProgress);
                Character.instance.SetCharacterStat(4, 2); // todoProgress + 2
                Debug.Log("TodoProgress 2 ����, ���� ��ġ : " + Character.instance.TodoProgress);
            }
            else
            {
                Character.instance.SetCharacterStat(type + 8, 2); // ���� ��
            }
        }
        else
        {
            Debug.Log("����Ʈ ����");
        }
    }


    public void BoxCount()
    {
        EventCountChange.Invoke();
    }
}
