using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TextRpg_MonsterHunting
{
    public class Character : Humanoid
	{
        public GameClassType ClassType { get; private set; }
        public int Level { get; private set; }
        public string Name { get; private set; }
        public double BaseAttackPower { get; protected set; }
        public double TotalAttackPower { get; private set; }
        public double BaseDefensePower { get; protected set; }
        public double TotalDefensePower { get; private set; }
        public double CurrentHealth { get; private set; }
        public double MaxMana { get; protected set; }
        public double CurrentMana { get; protected set; }
        public int Experience { get; private set; }
        public int Gold { get; private set; }
        public bool IsDie { get; set; }
        public int CurrentStage { get; private set; }
        public Inventory inventory { get; private set; }

        [JsonIgnore]
        public SkillManager skillManager;
        [JsonIgnore]
		public static Character Instance;
        [JsonIgnore]
		public const double MaxHealth = 100;

		[JsonConstructor]
		public Character(GameClassType ClassType, int Level, string Name, double BaseAttackPower, double TotalAttackPower, double BaseDefensePower, double TotalDefensePower,
	double CurrentHealth, double MaxMana, double CurrentMana, int Experience, int Gold, bool IsDie, int CurrentStage, Inventory inventory)
		{
            this.ClassType = ClassType;
			this.Level = Level;
			this.Name = Name;
			this.BaseAttackPower = BaseAttackPower;
			this.TotalAttackPower = TotalAttackPower;
			this.BaseDefensePower = BaseDefensePower;
			this.TotalDefensePower = TotalDefensePower;
			this.CurrentHealth = CurrentHealth;
			this.MaxMana = MaxMana;
			this.CurrentMana = CurrentMana;
			this.Experience = Experience;
			this.Gold = Gold;
			this.IsDie = IsDie;
			this.CurrentStage = CurrentStage;
			this.inventory = inventory;

			if (Instance == null)
			{
				Instance = this;
			}
            skillManager = new SkillManager();
			switch (this.ClassType)
			{
				case GameClassType.Warrior:

					// 스킬 추가
					skillManager.AddSkill(new Skill("알파 스트라이크", 10, 2f, 1));
					skillManager.AddSkill(new Skill("더블 스트라이크", 15, 1.5f, 2));
					break;
				case GameClassType.Wizard:

					skillManager.AddSkill(new Skill("크리스탈 블레이드", 60, 5f, 1));
					skillManager.AddSkill(new Skill("파이어 스톰", 30, 3f, 2));
					break;
				case GameClassType.Archer:

					skillManager.AddSkill(new Skill("레드 스윙", 50, 3f, 2));
					skillManager.AddSkill(new Skill("바이올렛 샷", 30, 2f, 3));
					break;
			}
		}


		public Character(GameClassType ClassType, string name)
        {
            if (Instance == null)
            {
                Instance = this;
            }

            this.ClassType = ClassType;
            Name = name;

            Level = 1;
            CurrentHealth = 100;
            Gold = 1500;
            CurrentStage = 0;          

			inventory = new Inventory();
            skillManager = new SkillManager();

            //캐릭터 처음 생성시 체력 포션 3개 추가
            for(int i = 0; i < 3; i++)
            {
                inventory.Add(Utils.HealthPotion);
            }

            switch (ClassType)
            {
                case GameClassType.Warrior:
                    BaseAttackPower = 10;
                    BaseDefensePower = 5;
                    MaxMana = 50;
                    CurrentMana = 50;

                    // 스킬 추가
                    skillManager.AddSkill(new Skill("알파 스트라이크", 10, 2f, 1));
                    skillManager.AddSkill(new Skill("더블 스트라이크", 15, 1.5f, 2));
                    break;
                case GameClassType.Wizard:
                    BaseAttackPower = 15;
                    BaseDefensePower = 1;
                    MaxMana = 150;
                    CurrentMana = 150;

                    skillManager.AddSkill(new Skill("크리스탈 블레이드", 60, 5f, 1));
                    skillManager.AddSkill(new Skill("파이어 스톰", 30, 3f, 2));
                    break;
                case GameClassType.Archer:
                    BaseAttackPower = 18;
                    BaseDefensePower = 3;
                    MaxMana = 100;
                    CurrentMana = 100;

                    skillManager.AddSkill(new Skill("레드 스윙", 50, 3f, 2));
                    skillManager.AddSkill(new Skill("바이올렛 샷", 30, 2f, 3));
                    break;
            }

			ChangeAttack(0);
			ChangeDefense(0);
		}

        // 캐릭터의 정보 출력
        public void PrintCharacterInfo() 
        {
            Console.Write($"Lv.");
            Console.WriteLine($"{Level.ToString("00")}");

            Console.Write($"직업 (");
            Console.Write($"{ReturnGameClassName()}");
            Console.WriteLine(")");

            Console.Write($"공격력 : ");
            Console.Write($"{BaseAttackPower} ");
			Console.ForegroundColor = ConsoleColor.DarkGreen;
            if(inventory.RightHand != null)
			    Console.Write($"(+ {inventory.RightHand.Stat})");
			Console.WriteLine();
			Console.ResetColor();

            Console.Write($"방어력 : ");
            Console.Write($"{BaseDefensePower} ");
			Console.ForegroundColor = ConsoleColor.DarkGreen;
			if (inventory.Body != null)
				Console.Write($"(+ {inventory.Body.Stat})");
            Console.WriteLine();
			Console.ResetColor();

            Console.Write($"체 력 : ");
            Console.WriteLine($"{CurrentHealth}");

			Console.Write($"마 나 : ");
			Console.WriteLine($"{CurrentMana}");

			Console.Write($"골드 : ");
            Console.WriteLine($"{Gold}");
        }
        
        // 직업명 한글로 변환
        public string ReturnGameClassName()
        {
            switch (ClassType)
            {
                case GameClassType.Warrior:
                    return "전사";
                case GameClassType.Wizard:
                    return "마법사";
                case GameClassType.Archer:
                    return "궁수";
                default:
                    return "전사";
            }
        }

        // 총 방어력 합산/감산
        public void ChangeDefense(double changeDefense)
        {
            TotalDefensePower = BaseDefensePower + changeDefense;
        }

        // 총 공격력 합산/감산
        public void ChangeAttack(double changeAttack)
        {
            TotalAttackPower = BaseAttackPower + changeAttack;
        }

        // 총 체력 합산/감산
        public void ChangeHealth(double changeHealth)
        {
            CurrentHealth += changeHealth;

            // 체력이 최대 체력을 넘어가는지 확인
            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
            // 체력이 0 이하로 내려가는지 확인
            else if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                IsDie = true;
            }
        }

        // 마나 합산/감산
        public void ChangeMana(double changeMana)
        {
            CurrentMana += changeMana;

            // 마나가 최대 마나를 넘어가는지 확인
            if (CurrentMana > MaxMana)
            {
                CurrentMana = MaxMana;
            }
            // 마나가 0 이하로 내려가는지 확인
            else if (CurrentMana <= 0)
            {
                CurrentMana = 0;
            }
        }

        // 골드 증가/감소
        public void ChangeGold(int changeGold)
        {
            Gold += changeGold;
        }

        // 스테이지 변화
        public void ChangeStage(int changeStage)
        {
            CurrentStage = changeStage;
        }

        // 레벨 / 경험치 증가
        public void Leveling(int monsterLevel)
        {
            int currentLevel = this.Level;
			Experience += monsterLevel;
			if (Experience >= 10 && Level==1)
            {
				Level += 1;
				BaseAttackPower += 0.5;
				BaseDefensePower += 1.0;
			}
			if (Experience >= 35 && Level == 2)
			{
				Level += 1;
				BaseAttackPower += 0.5;
				BaseDefensePower += 1.0;
			}
			if (Experience >= 65 && Level == 3)
			{
				Level += 1;
				BaseAttackPower += 0.5;
				BaseDefensePower += 1.0;
			}
			if (Experience >= 100 && Level == 4)
			{
				Level += 1;
				BaseAttackPower += 0.5;
				BaseDefensePower += 1.0;
			}
            if(Experience > 100 && Experience >= Level * 20)
            {
				Level += 1;
				BaseAttackPower += 0.5;
				BaseDefensePower += 1.0;
			}

            if(currentLevel != this.Level)
            {
                QuestManager.Instance.Quests[2].CheckQuestProgress();
            }
		}

        // 공격 기능, 피해량 반환
        public double BasicAttack()
        {
            // 공격력은 10%의 오차를 가짐
            double errorRange = TotalAttackPower * 0.1;
            errorRange = Math.Ceiling(errorRange); // 올림

            // 피해량 계산
            Random random = new Random();
            double min = TotalAttackPower - errorRange;
            double max = TotalAttackPower + errorRange;
            double attackDamage = min + random.NextDouble() * (max - min);

            // 치명타 계산 
            bool isCritical = random.NextDouble() < 0.15; // 15% 확률로 발생
            if(isCritical)
            {
                attackDamage *= 1.6; // 160% 데미지
            }

            // 적중 실패 확률
            bool isAttackMiss = random.NextDouble() < 0.10; // 10% 확률로 발생
            if(isAttackMiss)
            {
                return 0;
            }
            else // 적중 성공,적 체력 감소
            {
                return attackDamage;
            }
        }
    }
}
