using System.ComponentModel;

namespace TextRpg_MonsterHunting
{
    // 몬스터를 나타내는 클래스 정의 /2024.04.30 박재우
    public class Monster : Humanoid
	{
        // 몬스터의 속성들 /2024.04.30 박재우
        public string Name { get; set; } // 몬스터 이름
        public double TotalAttackPower { get; set; } // 몬스터의 공격력
        public double CurrentHealth { get; set; } // 몬스터의 체력
        public bool IsDie { get; set; } // 몬스터가 죽었는지 여부를 나타내는 플래그
        public int EnemyExp { get; set; } // 몬스터를 처치했을 때 얻는 경험치

        // 몬스터 객체를 초기화하는 생성자
        public Monster(string name, double health, float attackPower, int enemyExp)
        {
            Name = name;
            TotalAttackPower = attackPower;
            CurrentHealth = health;
			IsDie = false; // 몬스터는 처음에는 죽지 않은 상태///
            EnemyExp = enemyExp;
        }

		public Monster(Monster monster)
		{
			Name = monster.Name;
			TotalAttackPower = monster.TotalAttackPower;
			CurrentHealth = monster.CurrentHealth;
			IsDie = false;
			EnemyExp = monster.EnemyExp;
		}

        // 몬스터 정보를 출력하는 메서드
        private void PrintMonstersInfo(List<Monster> monsters)
        {
            Console.WriteLine("\n[몬스터 정보]"); // 몬스터 정보 헤더 출력
            for (int i = 0; i < monsters.Count; i++)
            {
                if (!monsters[i].IsDie) // 몬스터가 살아 있다면
                {
                    Console.WriteLine($"{i + 1}. {monsters[i].Name}, 체력: {monsters[i].CurrentHealth}"); // 몬스터 이름과 체력 출력
                }
                else
                {
					Console.ForegroundColor = ConsoleColor.Gray; // 죽은 몬스터 글자색 회색
                    Console.WriteLine($"{i + 1}. {monsters[i].Name} Dead"); // 몬스터가 죽었다면 죽음 표시 출력
					Console.ResetColor();
                }
            }
        }

		// 총 체력 합산/감산
		public void ChangeHealth(double changeHealth)
		{
			CurrentHealth += changeHealth;

			// 체력이 0 이하로 내려가는지 확인
			if (CurrentHealth <= 0)
			{
				CurrentHealth = 0;
				IsDie = true;
				QuestManager.Instance.Quests[0].CheckQuestProgress();
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
			if (isCritical)
			{
				attackDamage *= 1.6; // 160% 데미지
			}

			// 적중 실패 확률
			bool isAttackMiss = random.NextDouble() < 0.10; // 10% 확률로 발생
			if (isAttackMiss)
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