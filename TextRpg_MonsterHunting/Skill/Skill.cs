using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg_MonsterHunting
{
    public class Skill
    {
        // 스킬 속성
        public string Name { get; private set; }             // 스킬 이름
        public int MpCost { get; private set; }              // 스킬 사용에 필요한 MP
        public float DamageMultiplier { get; private set; }  // 공격력 배수
        public int TargetCount { get; private set; }         // 공격 대상 수

        // 생성자
        public Skill(string name, int mpCost, float damageMultiplier, int targetCount)
        {
            Name = name;
            MpCost = mpCost;
            DamageMultiplier = damageMultiplier;
            TargetCount = targetCount;
        }
    }
}
