using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TextRpg_MonsterHunting
{
    public class QuestManager
    {
        public static QuestManager Instance;
        public List<Quest> Quests;

        //퀘스트 목록 생성
        public QuestManager()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            Quests = new List<Quest>();
            Quests.Add(new ManaPotionQuest("마을을 위협하는 미니언 처치", 5));
            Quests.Add(new HealthPotionQuest("장비 장착해보자", 5));
            Quests.Add(new AttackItemQuest("더욱 더 강해지기!", 5));
        }

        //퀘스트 보상 지급
        public void CheckQuestCompletion(Character character, Quest quest)
        {
            if (quest.IsAccept && quest.IsClear)
            {
                Console.WriteLine("보상이 지급됐습니다!\n");
                Console.WriteLine("[보상 내역]");
                switch (quest.RewardItem)
                {
                    case ItemType.Mana:
                        character.inventory.Add(Utils.ManaPotion);
                        Console.WriteLine("마나 포션 x1");
                        ((ManaPotionQuest)quest).ClearMonsterCount();
                        break;
                    case ItemType.Health:
                        character.inventory.Add(Utils.HealthPotion);
                        Console.WriteLine("체력 포션 x1");
                        break;
                    case ItemType.Attack:
                        character.inventory.Add(Utils.Sword);
                        Console.WriteLine("단검 x1");                   
                        break;
                }
                character.ChangeGold(quest.RewardGold);
                quest.IsClear = false;
                quest.IsAccept = false;
                Console.WriteLine($"{quest.RewardGold}G");
                Console.WriteLine("아무키나 누르세요~~~~");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("아직 퀘스트가 진행중입니다!\n");
                Console.WriteLine("아무키나 누르세요~~~~");
                Console.ReadLine();
            }
        }
    }
}
