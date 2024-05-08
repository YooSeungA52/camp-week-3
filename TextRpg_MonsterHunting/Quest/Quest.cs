using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TextRpg_MonsterHunting
{
    public class Quest //2024.05.02 박재우
    {
        public string Title { get; private set; }
        public int RewardGold { get; private set; }
        public ItemType RewardItem { get; protected set; }
        public bool IsAccept { get; set; }
        public bool IsClear { get; set; }

        public Quest(string title, int rewardGold)
        {
            Title = title;
            RewardGold = rewardGold;
        }

        //퀘스트 수락 창 출력
        public void PrintQuestStatus()
        {
            if (IsAccept)
            {
                Console.WriteLine("\n1. 보상 받기");
                Console.WriteLine("2. 돌아가기");
            }
            else
            {
                Console.WriteLine("\n1. 수락");
                Console.WriteLine("2. 거절");
            }
        }
        public virtual void QuestContent() { }
        public virtual void CheckQuestProgress() { }
    }
}