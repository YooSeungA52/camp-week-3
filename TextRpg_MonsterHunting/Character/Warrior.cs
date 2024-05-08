using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg_MonsterHunting
{
    public class Warrior : Character
    {
        public Warrior(string name) : base(name)
        {
            BaseAttackPower = 10;
            BaseDefensePower = 5;
            MaxMana = 50;
            CurrentMana = 50;
            ChangeAttack(0);
            ChangeDefense(0);

            // 스킬 추가
            skillManager.AddSkill(new Skill("알파 스트라이크", 10, 2f, 1));
            skillManager.AddSkill(new Skill("더블 스트라이크", 15, 1.5f, 2));
        }
    }
}
