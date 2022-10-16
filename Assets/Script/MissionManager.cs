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
        // 여기서 활성화 시켜주는 패널 안에 퀘스트 받기와 퀘스트 완료하기 - 채팅을 통해 완료하기로 이어지면 QuestConfirm() 실행
        // 퀘스트 받기는 어떻게 할까
        // 퀘스트는 하루에 한 번 자동 부여되고, 창에서 확인 가능하며, 완료할 때만 NPC를 찾아오면 된다.
    }

    public void QuestConfirm()
    {
        // 직업에 따라 필요로 하는 아이템이 있다.
        // 컨펌을 받으러 오면 직업을 먼저 확인, 그에 맞는 아이템 넘버를 넣어서 CanDeleteItem 확인
        if (Character.instance.MyItemManager.CanDeleteItem(UniqueQuestList[0][Character.instance.MyJob.ToString()].ToString()) )        // true라면 SetCharacterStat으로 아이템을 제거 후 보상 제공
        {
            Character.instance.SetCharacterStat(8, (UniqueQuestList[1][Character.instance.MyJob.ToString()].ToString())/*없앨 아이템 넘버와 개수*/);
            Character.instance.SetCharacterStat(4, 5); // TodoProgress 5 증가
            // 고생했다는 말 띄워주기
        }
        else         // false라면 채팅으로 조건 달성 후 다시 오라고 얘기해준다
        {
            // 조건 달성 후 다시오라고 띄워주기
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
