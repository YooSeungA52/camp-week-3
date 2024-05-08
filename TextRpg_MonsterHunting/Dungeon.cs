using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TextRpg_MonsterHunting
{
    internal class Dungeon
    {   //Class Dungeon에 변수를 선언, 몬스터리스트를 선언. 아래는 멤버변수     
        Character _hero;
        List<Monster> _monsterHouse;
        UI _ui;
        bool _stageClear;
        int _stageNum;

        //InDungeon()의 매개변수(서로 종속인 변수들을 묶어주는 변수)를 chracacter 클래스에 대입
        public void InDungeon(Character hero, UI ui) //hero = new Character(heroClass, heroName ?? "홍길동")
        {
            Console.Clear();
            _stageClear = false;
            _ui = ui;
            _hero = hero; //hero = new Character(heroClass, heroName ?? "홍길동"); 이므로 선언된 값 불러오기.
            List<Monster> monsterKind;
            Console.Write($"\n"); //public string Name { get; private set; }
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write($"\"{_hero.Name}\"");
            Console.ResetColor();
            Console.WriteLine("은(는) 던전에 입장했습니다.\n");



            //지역변수 monsterkind와 멤버변수_monsterHouse를 new 사용, 리스트를 초기화
            monsterKind = new List<Monster>();
            _monsterHouse = new List<Monster>();

            //선언한 _monsterKind new List<Monster>를 구성하기위해 public Monster 불러오기. ("string", 공격력, HP, 레벨)
            monsterKind.Add(new Monster("미니언", 15, 5, 2));
            monsterKind.Add(new Monster("공허충", 10, 9, 3));
            monsterKind.Add(new Monster("대포미니언", 25, 8, 5));

            //스테이지 생성 작업
            //몬스터 생성
            _stageNum = hero.CurrentStage;
            Random random = new Random();

            int howMany = random.Next(1 + _stageNum, 5 + _stageNum);

            for (int i = 0; i < howMany; i++)
            {
                int index = random.Next(0, 3);
                _monsterHouse.Add(new Monster(monsterKind[index]));
            }

            if (_stageNum % 5 == 0)
            {
                //보스 몬스터를 _stageNum / 5 만큼의 마릿수 만큼 추가
                for (int i = 0; i < _stageNum / 5; i++)
                {
                    _monsterHouse.Add(new Monster("유르기어", 120, 10, 20));
                }
            }
            Battle();
        }

        //영웅 행동 선택
        public void Battle()
        {
            bool fightEnd = false;
            double heroHealthBeforeFight = _hero.CurrentHealth;
            while (!fightEnd)
            {
                Console.Clear();
                Console.Write($"Stage {_stageNum}");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(" Battle!!\n");
                Console.ResetColor();
                for (int i = 0; i < _monsterHouse.Count; i++)
                {
                    Monster monster = _monsterHouse[i];
                    if (monster.IsDie)
                    {
						Console.ForegroundColor = ConsoleColor.Gray;
						Console.WriteLine($"Lv.{monster.EnemyExp} {monster.Name} Dead");
                        Console.ResetColor();
					}
                    else
                    {
						Console.WriteLine($"Lv.{monster.EnemyExp} {monster.Name} HP {monster.CurrentHealth}");
					}
                }
                PrintCharacterInfo();
                Console.WriteLine("\n1. 공격");
                Console.WriteLine("2. 스킬\n");

                int input = _ui.UserChoiceInput(1, 2);
                bool didAttack = false;//플레이어가 공격을 취소하지 않았는지
                switch (input)
                {
                    case 1:
                        didAttack = ChooseMonsterToAttack();
                        break;
                    case 2:
                        didAttack = UseSkill();
                        break;
                }

                if (didAttack)
                {
                    //몬스터들의 공격 차례
                    foreach (Monster monster in _monsterHouse)
                    {
                        if (!monster.IsDie)
                        {
                            AttackTarget(monster, _hero);
                        }
                    }
                    //전투 끝 확인
                    fightEnd = CheckFightEnd();
                }
            }

            //전투 끝나고 마무리 작업
            AfterFight(heroHealthBeforeFight);
        }

        //플레이어가 스킬 사용
        bool UseSkill()
        {
            bool didAttack = false;
            Console.Clear();
            Console.Write($"Stage {_stageNum}");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(" Battle!!\n");
            Console.ResetColor();

            for (int i = 0; i < _monsterHouse.Count; i++)
            {
				Monster monster = _monsterHouse[i];
				if (monster.IsDie)
				{
					Console.ForegroundColor = ConsoleColor.Gray;
					Console.WriteLine($"Lv.{monster.EnemyExp} {monster.Name} Dead");
					Console.ResetColor();
				}
				else
				{
					Console.WriteLine($"Lv.{monster.EnemyExp} {monster.Name} HP {monster.CurrentHealth}");
				}
			}
            PrintCharacterInfo();
            //스킬 목록 출력
            _hero.skillManager.PrintSkills();

            Console.WriteLine("0. 취소\n");

            int input = _ui.UserChoiceInput(0, 2);
            if (input != 0)
            {
                //스킬 사용
                didAttack = _hero.skillManager.UseSkill(input, _monsterHouse, _hero);
				Console.WriteLine("0. 다음");
				_ui.UserChoiceInput(0, 0);
			}
            return didAttack;
        }

        //플레이어가 공격 상대 선택
        bool ChooseMonsterToAttack()
        {
            bool didAttack = false;

            Console.Clear();
            Console.Write($"Stage {_stageNum}");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(" Battle!!\n");
            Console.ResetColor();
            for (int i = 0; i < _monsterHouse.Count; i++)
            {
                Monster monster = _monsterHouse[i];
                Console.Write($"- {i + 1}. ");
                if (monster.IsDie)
                {
                    Console.Write($"Lv.{monster.EnemyExp} {monster.Name} Dead\n");
                }
                else
                {
                    Console.Write($"Lv.{monster.EnemyExp} {monster.Name} HP {monster.CurrentHealth}\n");
                }              
            }

            PrintCharacterInfo();
            Console.WriteLine("0. 취소\n");
            Console.Write("공격할 대상을 선택해주세요. \n>> ");

            int input = int.Parse(Console.ReadLine());
            while (input < 0 || input > _monsterHouse.Count || (input > 0 && _monsterHouse[input - 1].IsDie))
            {
                Console.Write("잘못된 입력입니다. 다시 입력해 주세요. \n>> ");
                input = int.Parse(Console.ReadLine());
            }

            switch (input)
            {
                case 0:
                    didAttack = false;
                    break;
                default:
                    AttackTarget(_hero, _monsterHouse[input - 1]);
                    didAttack = true;
                    break;
            }
            return didAttack;
        }

        //게임 끝 체크
        bool CheckFightEnd()
        {
            bool fightEnd = false;
            if (_hero.IsDie)
            {
                fightEnd = true;
            }
            else
            {
                fightEnd = true;
                foreach (Monster monster in _monsterHouse)
                {
                    if (!monster.IsDie)
                    {
                        fightEnd = false;
                        break;
                    }
                }
                _stageClear = fightEnd;
            }
            return fightEnd;
        }

        // 몬스터를 공격하는 메서드
        public void AttackTarget(Humanoid attacker, Humanoid target)
        {
            Console.Clear();
            Console.Write($"Stage {_stageNum}");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write(" Battle!!\n");
            Console.ResetColor();
            if (attacker is Monster)
            {
                Monster monster = attacker as Monster;
                Console.Write($"Lv. ");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write($"{monster.EnemyExp} {monster.Name} ");
                Console.ResetColor();
                Console.WriteLine("의 공격!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write($"{attacker.Name} ");
                Console.ResetColor();
                Console.WriteLine("의 공격!");
            }

            //데미지 계산
            double totalDamage = attacker.BasicAttack();
            double beforeHealth = target.CurrentHealth;
            double totalDefence = 0;
            if (attacker is Monster)
            {
                totalDefence = _hero.TotalDefensePower;
            }
            totalDamage -= totalDefence;
            if (totalDamage < 0)
            {
                totalDamage = 0;
            }
            totalDamage = Math.Round(totalDamage);
            target.ChangeHealth(-totalDamage);

            //데미지 받은 경우 받은 후 상태 출력
            if (beforeHealth - target.CurrentHealth > 0)
            {
                if (target is Monster)
                {
                    Monster monster = (Monster)target;
                    Console.Write($"Lv.");

                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write($"{monster.EnemyExp} {monster.Name}");
                    Console.ResetColor();
					Console.Write("을(를) 맞췄습니다.");

					Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("[데미지 : ");
					Console.Write($"{totalDamage}");
					Console.Write("]");
					Console.ResetColor();

                    if (totalDamage > attacker.TotalAttackPower * 1.1) // 치명타시 추가 출력
                    {
                        Console.Write(" - 치명타 공격!!");
                    }
                    Console.WriteLine($"\n\nLv.{monster.EnemyExp} {monster.Name}");
                }
                else if (target is Character)
                {
                    Character human = (Character)target;
					Console.ForegroundColor = ConsoleColor.DarkGreen;
					Console.Write($"{human.Name}");
					Console.ResetColor();
					Console.Write("을(를) 맞췄습니다.");

					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write("[데미지 : ");
					Console.Write($"{totalDamage}");
					Console.Write("]");
					Console.ResetColor();

					if (totalDamage > attacker.TotalAttackPower * 1.1) // 치명타시 추가 출력
                    {
                        Console.Write(" - 치명타 공격!!");
                    }
                    Console.WriteLine($"\n\nLv.{human.Level} {human.Name}");
                }

                if (target.IsDie)
                {
                    Console.WriteLine($"HP {beforeHealth} -> Dead");
                }
                else
                {
                    Console.WriteLine($"HP {beforeHealth} -> {target.CurrentHealth}");
                }
            }
            else //회피한 경우
            {
                if (target is Monster)
                {
                    Monster monster = (Monster)target;
                    Console.WriteLine($"Lv.{monster.EnemyExp} {monster.Name} 을(를) 공격했지만 아무일도 일어나지 않았습니다.\n");
                }
                else if (target is Character)
                {
                    Character human = (Character)target;
                    Console.WriteLine($"{human.Name} 을(를) 공격했지만 아무일도 일어나지 않았습니다.\n");
                }
            }

            Console.WriteLine("0. 다음");
            _ui.UserChoiceInput(0, 0);
        }

        //캐릭터 상태 출력
        void PrintCharacterInfo()
        {
            Console.WriteLine("\n[내정보]");
            Console.WriteLine($"Lv.{_hero.Level} {_hero.Name} ({_hero.ReturnGameClassName()})");
            Console.WriteLine("HP {0}/{1}", _hero.CurrentHealth, Character.MaxHealth);
            Console.WriteLine("MP {0}/{1}\n", _hero.CurrentMana, _hero.MaxMana);
        }

        //전투 끝나고 마무리 작업
        //게임 클리어 또는 패배 정보 출력
        //클리어 시 보상 획득 함수 호출
        void AfterFight(double heroHealthBeforeFight)
        {
            Console.Clear();
            Console.WriteLine($"Stage {_stageNum} Battle!! - Result\n");
            if (_stageClear)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Victory\n");
                Console.ResetColor();

                Console.WriteLine($"던전에서 몬스터 {_monsterHouse.Count}마리를 잡았습니다.");
                Reward();
            }
            else
            {
                Console.WriteLine("You Lose\n");
            }

            Console.WriteLine($"Lv.{_hero.Level} {_hero.Name}");
            Console.WriteLine($"HP {heroHealthBeforeFight} -> {_hero.CurrentHealth}\n");
            Console.WriteLine("0. 다음");
            int userInput = _ui.UserChoiceInput(0, 0);
            SceneManager.Instance._startScene.loadScene(_ui, _hero);
        }

        //던전 보상 지급
        void Reward()
        {
            Console.Clear();
            Random random = new Random();
            Console.WriteLine("보상 획득!!\n");

            //마나 물약
            int manaAmount = random.Next(1, 2);
            Console.WriteLine($"마나 물약 {manaAmount}개\n");
            for (int i = 0; i < manaAmount; i++)
            {
                _hero.inventory.Add(Utils.ManaPotion);
            }

            //체력 물약
            int healAmount = random.Next(_stageNum, _stageNum + 1);
            Console.WriteLine($"체력 물약 {healAmount}개\n");
            for (int i = 0; i < healAmount; i++)
            {
                _hero.inventory.Add(Utils.HealthPotion);
            }

            //무기 공격력  random.Next(1, _stageNum + 1);
            int weaponAttack = random.Next(1, _stageNum+3);
            Equipment dungeonSword = new Equipment($"던전의 잔혹함{_stageNum}", EquipmentType.OneHand, weaponAttack, "매서운 던전에서만 벼려진다는 날카로운 칼입니다.", ItemType.Attack, _stageNum*200);
            _hero.inventory.Add(dungeonSword);
            Console.WriteLine($"무기 : {dungeonSword.Name} | 공격력 + {dungeonSword.Stat} | {dungeonSword.Discription}\n");

            //방어구 방어력 random.Next(1, _stageNum + 1);
            int armorDefence = random.Next(1, _stageNum+3);
            Equipment dungeonArmor = new Equipment($"던전의 고요함{_stageNum}", EquipmentType.Body, armorDefence, "던전의 깊은 어둠을 담아낸 갑옷입니다.", ItemType.Defence, _stageNum*200);
            _hero.inventory.Add(dungeonArmor);
            Console.WriteLine($"방어구 : {dungeonArmor.Name} | 방어력 + {dungeonArmor.Stat} | {dungeonArmor.Discription}\n");
        
            //경험치 계산
            foreach(Monster monster in _monsterHouse)
            {
                _hero.Leveling(monster.EnemyExp);
            }
            _hero.ChangeStage(_hero.CurrentStage + 1);
        }
    }
}

