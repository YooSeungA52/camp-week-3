using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg_MonsterHunting
{
    public class ManaPotionQuest : Quest
    {
        int monstersKilledCount;
        public ManaPotionQuest(string title, int rewardGold)
            : base(title, rewardGold) 
        {
            RewardItem = ItemType.Mana;
            monstersKilledCount = 0;
        }

		//퀘스트 설명
		public override void QuestContent()
        {
            Console.WriteLine("\n== 마을을 위협하는 미니언 처치 ==");
            Console.WriteLine("이봐! 마을 근처에 미니언들이 너무 많아졌다고 생각하지 않나? ");
            Console.WriteLine("마을 주민들의 안전을 위해서라도 저것들 수를 좀 줄여야 한다고!");
            Console.WriteLine("모험가인 자네가 좀 처치해주게!\n");

            Console.Write($"미니언 ");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write($"5");
            Console.ResetColor();
            Console.Write($"마리 처치 (");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write($"{monstersKilledCount}/5");
            Console.ResetColor();
            Console.WriteLine($")");

            Console.WriteLine("\n보상");

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("마나 포션 x1, 5G");
            Console.ResetColor();
        }
		//퀘스트 클리어 업데이트
		public override void CheckQuestProgress()
        {          
            if (this.IsAccept) // 수락후
            {
                monstersKilledCount++;
                if(monstersKilledCount == 5)
                {
                    this.IsClear = true;
                }          
            }
        }
        //몬스터 처치 수 초기화
        public void ClearMonsterCount()
        {
            this.monstersKilledCount = 0;          
        }
    }
}
