using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg_MonsterHunting
{
    internal class PotionScene : Scene
    {
        //포션 사용 창
        //회복 또는 마나 회복 가능
        public void loadScene(UI ui, Character character)
        {
            int userInput;
            do
            {
                ui.PrintTitle("회복");

                character.inventory.PrintPotionItems();
                Console.WriteLine("\n0. 나가기");

				userInput = ui.UserChoiceInput(0, 2);
                bool skipEnter = character.inventory.ManagePotions(userInput, character);
                if (!skipEnter)
                {
					Console.WriteLine("0. 다음");
					ui.UserChoiceInput(0, 0);
				}
			} while (userInput != 0);

            SceneManager.Instance._startScene.loadScene(ui, character);
        }
    }
}
