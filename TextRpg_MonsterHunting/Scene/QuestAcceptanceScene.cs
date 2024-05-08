using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg_MonsterHunting
{
    internal class QuestAcceptScene : Scene
    {
        int userInput;

        public void loadScene(UI ui, Character character)
        {
            ui.PrintTitle("퀘스트");

            List<Quest> QuestList = QuestManager.Instance.Quests;

            switch (userInput)
            {
                case 1:
                    QuestList[0].QuestContent();
                    QuestList[0].PrintQuestStatus();
                    userInput = ui.UserChoiceInput(1, 3);

                    if (userInput == 1)
                    {
                        if (QuestList[0].IsAccept == true)
                        {
                            QuestManager.Instance.CheckQuestCompletion(character, QuestList[0]);
                        }
                        else
                        {
                            QuestList[0].IsAccept = true;
                        }
                    }
                    else
                    {
                        SceneManager.Instance._questScene.loadScene(ui, character);
                    }
                    break;
                case 2:
                    QuestList[1].QuestContent();
                    QuestList[1].PrintQuestStatus();
                    userInput = ui.UserChoiceInput(1, 3);

                    if (userInput == 2)
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
                    else
                    {
                        SceneManager.Instance._questScene.loadScene(ui, character);
                    }
                    break;
                case 3:
                    QuestList[2].QuestContent();
                    QuestList[2].PrintQuestStatus();
                    userInput = ui.UserChoiceInput(1, 3);

                    if (userInput == 3)
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
                    else
                    {
                        SceneManager.Instance._questScene.loadScene(ui, character);
                    }
                    break;
            }
        }
    }
}
