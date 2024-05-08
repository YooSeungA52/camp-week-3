using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TextRpg_MonsterHunting
{
    public class IntroScene
    {
        Character character;

		//게임 시작 시 캐릭터를 생성하는 인트로 창
		//종료시 게임 메인 창으로 이동
        public void loadScene(UI ui)
        {
            bool isHeroLoaded = Utils.LoadHero(out character);
            if (isHeroLoaded)
            {
				Console.WriteLine($"스파르타 던전에 돌아오신 것을 환영합니다 {character.Name}님!");
            }
            else
            {
				Console.Write("스파르타 던전에 오신 여러분 환영합니다.\n원하시는 이름을 설정해주세요.\n>> ");
				string? heroName = Console.ReadLine();
				Console.Clear();

				Console.WriteLine("원하시는 직업을 선택하세요:");
				Console.WriteLine("1. 전사");
				Console.WriteLine("2. 마법사");
				Console.WriteLine("3. 궁수");

				//UI에 직업 선택 함수 결과
				int inputForClass = ui.UserChoiceInput(1, 3);

				switch (inputForClass)
				{       //명시적 형변환
					case (int)GameClassType.Warrior:
						character = new Character(GameClassType.Warrior, heroName ?? "르탄이");
						break;
					case (int)GameClassType.Wizard:
						character = new Character(GameClassType.Wizard, heroName ?? "르탄이");
						break;
					case (int)GameClassType.Archer:
						character = new Character(GameClassType.Archer, heroName ?? "르탄이");
						break;
				}
			}

            Console.Clear();
            Console.WriteLine($"\n입력한 정보 확인");
            Console.WriteLine($"이름 : {character.Name}");
            Console.WriteLine($"직업 : {character.ReturnGameClassName()}\n");

            ui.CountdownComment(3, "마을로 이동합니다."); //3초 후 마을로 이동
            SceneManager.Instance._startScene.loadScene(ui, character);
        }
    }
}
