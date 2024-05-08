using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg_MonsterHunting
{
    public class QuestScene : Scene
    {
        int userInput;

        //퀘스트 관리 창
        //퀘스트를 수락, 거절 또는 클리어 시 보상 획득을 할 수 있음
        public void loadScene(UI ui, Character character)
        {
            do
            {
                ui.PrintTitle("퀘스트");

                List<Quest> QuestList = QuestManager.Instance.Quests;
                for (int i = 0; i < QuestList.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {QuestList[i].Title}");
                }

                Console.WriteLine("\n0. 나가기");
                userInput = ui.UserChoiceInput(0, QuestList.Count + 1);

                //
                ui.PrintTitle("퀘스트");

                switch (userInput)
                {
                    case 1:
                        QuestList[0].QuestContent();
                        QuestList[0].PrintQuestStatus();
                        userInput = ui.UserChoiceInput(1, 2);

                        if (userInput == 1) // 보상받기 or 수락  
                        {
                            if (QuestList[0].IsAccept == true) // 보상받기
                            {
                                QuestManager.Instance.CheckQuestCompletion(character, QuestList[0]);
                            }
                            else // 수락
                            {
                                QuestList[0].IsAccept = true;
                            }
                        }
                        break;
                    case 2:
                        QuestList[1].QuestContent();
                        QuestList[1].PrintQuestStatus();
                        userInput = ui.UserChoiceInput(1, 2);

                        if (userInput == 1)
                        {
                            if (QuestList[1].IsAccept == true)
                            {
                                QuestManager.Instance.CheckQuestCompletion(character, QuestList[1]);
                            }
                            else
                            {
                                QuestList[1].IsAccept = true;
                            }
                        }
                        break;
                    case 3:
                        QuestList[2].QuestContent();
                        QuestList[2].PrintQuestStatus();
                        userInput = ui.UserChoiceInput(1, 2);

                        if (userInput == 1)
                        {
                            if (QuestList[2].IsAccept == true)
                            {
                                QuestManager.Instance.CheckQuestCompletion(character, QuestList[2]);
                            }
                            else
                            {
                                QuestList[2].IsAccept = true;
                            }
                        }
                        break;
                }
            } while (userInput != 0);
            SceneManager.Instance._startScene.loadScene(ui, character);
        }
    }
}
