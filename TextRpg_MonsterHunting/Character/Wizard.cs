using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg_MonsterHunting
{
    public class Wizard : Character
    {
        public Wizard(string name) : base(name)
        {
            BaseAttackPower = 15;
            BaseDefensePower = 1;
            MaxMana = 150;
            CurrentMana = 150;
			ChangeAttack(0);
			ChangeDefense(0);

			skillManager.AddSkill(new Skill("크리스탈 블레이드", 60, 5f, 1));
            skillManager.AddSkill(new Skill("파이어 스톰", 30, 3f, 2));
        }
    }
}