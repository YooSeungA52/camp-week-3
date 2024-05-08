using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg_MonsterHunting
{
    internal class EquipmentScene : Scene
    {
		//장착 관리 창 출력
		public void loadScene(UI ui, Character character)
        {
            int userInput;
            do
            {
                ui.PrintTitle("인벤토리 - 장착 관리");

                character.inventory.PrintItems(true); //장착 관리 목록 출력
                Console.WriteLine("\n0. 나가기");

                int itemChoice = character.inventory.EquipmentsInBag.Data.Count;
                userInput = ui.UserChoiceInput(0, itemChoice);
                character.inventory.ManageEquipments(userInput);
            } while (userInput != 0);

            SceneManager.Instance._startScene.loadScene(ui, character);
        }
    }
}
