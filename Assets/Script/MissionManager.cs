using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    List<Dictionary<string, object>> UniqueQuestList;
    // Start is called before the first frame update
    void Start()
    {
        UniqueQuestList = CSVReader.Read("UniqueQuest");
    }

    private void ChoiceButtonActive()
    {
        // ���⼭ Ȱ��ȭ �����ִ� �г� �ȿ� ����Ʈ �ޱ�� ����Ʈ �Ϸ��ϱ� - ä���� ���� �Ϸ��ϱ�� �̾����� QuestConfirm() ����
        // ����Ʈ �ޱ�� ��� �ұ�
        // ����Ʈ�� �Ϸ翡 �� �� �ڵ� �ο��ǰ�, â���� Ȯ�� �����ϸ�, �Ϸ��� ���� NPC�� ã�ƿ��� �ȴ�.
    }

    public void QuestConfirm()
    {
        // ������ ���� �ʿ�� �ϴ� �������� �ִ�.
        // ������ ������ ���� ������ ���� Ȯ��, �׿� �´� ������ �ѹ��� �־ CanDeleteItem Ȯ��
        if (Character.instance.MyItemManager.CanDeleteItem(UniqueQuestList[0][Character.instance.MyJob.ToString()].ToString()) )        // true��� SetCharacterStat���� �������� ���� �� ���� ����
        {
            Character.instance.SetCharacterStat(8, (UniqueQuestList[1][Character.instance.MyJob.ToString()].ToString())/*���� ������ �ѹ��� ����*/);
            Character.instance.SetCharacterStat(4, 5); // TodoProgress 5 ����
            // ����ߴٴ� �� ����ֱ�
        }
        else         // false��� ä������ ���� �޼� �� �ٽ� ����� ������ش�
        {
            // ���� �޼� �� �ٽÿ���� ����ֱ�
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "R_Weapon")
        {
            ChoiceButtonActive();
        }
    }
}
