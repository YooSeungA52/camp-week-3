using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg_MonsterHunting
{
    public class CharacterInfoScene : Scene
    {
        //캐릭터 정보 창 출력
        public void loadScene(UI ui, Character character)
        {
            ui.PrintTitle("상태 보기");

            character.PrintCharacterInfo(); //캐릭터 정보 출력

            Console.WriteLine("\n0. 나가기");

            int userInput = ui.UserChoiceInput(0, 0);

            SceneManager.Instance._startScene.loadScene(ui, character);
        }
    }
}
