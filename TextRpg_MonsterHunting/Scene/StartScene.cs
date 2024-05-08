using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg_MonsterHunting
{
    //메인 씬
    //상태 보기, 인벤토리 관리 등 다른 창으로 이동 가능
    //7번을 입력하면 게임 저장 데이터를 초기화하고 게임을 종료함
    //여기에 들어오면 게임이 저장됨
    //전투 종료 후 죽을 경우 여기서 체력 1%로 회복
    public class StartScene : Scene
    {
        public void loadScene(UI ui, Character character)
        {
            Shop shop = new Shop(character, ui);
            character.IsDie = false;
            character.ChangeHealth(Character.MaxHealth * 0.01);
            Console.Clear();
            Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
            Console.WriteLine("이제 전투를 시작할 수 있습니다.\n");

            Utils.SaveHero(character);
            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리 관리");
            Console.WriteLine("3. 회복 아이템 사용");
            Console.WriteLine("4. 상점 입장");
			Console.WriteLine("5. 퀘스트");
            Console.WriteLine("6. 전투 시작");
            Console.WriteLine("7. 저장 초기화");
            Console.WriteLine("\n0. 나가기");

            int userInput = ui.UserChoiceInput(0, 7);

            switch(userInput)
            {
                case 0: //게임 종료
                    ui.CountdownComment(2, "게임이 종료됩니다.");

                    Environment.Exit(0); //정상 종료 코드 0, 음수값이 들어가면 비정상 종료

                    break;
                case 1: //상태 보기
                    SceneManager.Instance._characterInfoScene.loadScene(ui, character);
                    break;
                case 2: //인벤토리 관리
                    SceneManager.Instance._inventoryScene.loadScene(ui, character);
                    break;
                case 3: //회복 아이템 사용
                    SceneManager.Instance._potionScene.loadScene(ui, character);
                    break;
                case 4: //상점 입장
                    shop.ShowItems(ui, character);
                    break;
                case 5:  //퀘스트
                    SceneManager.Instance._questScene.loadScene(ui, character);
					break;
                case 6: //전투 시작(던전 입장)
                    Dungeon dungeon = new Dungeon();
                    dungeon.InDungeon(character, ui);
                    break;
                case 7: //저장 초기화
					Utils.SaveDestory();
					Environment.Exit(0); //정상 종료 코드 0, 음수값이 들어가면 비정상 종료
					break;
            }
        }
    }
}
