using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg_MonsterHunting
{
    public class Archer : Character
    {
        public Archer(string name) : base(name)
        {
            BaseAttackPower = 18;
            BaseDefensePower = 3;
            MaxMana = 100;
            CurrentMana = 100;
			ChangeAttack(0);
			ChangeDefense(0);

			skillManager.AddSkill(new Skill("레드 스윙", 50, 3f, 2));
            skillManager.AddSkill(new Skill("바이올렛 샷", 30, 2f, 3));
        }
    }
}
