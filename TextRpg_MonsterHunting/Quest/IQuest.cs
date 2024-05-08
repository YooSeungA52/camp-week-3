using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg_MonsterHunting
{
    //퀘스트 인터페이스
    public interface IQuest
    {
        public void QuestContent();
        public void CheckQuestProgress();
    }
}
