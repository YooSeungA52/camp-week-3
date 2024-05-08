using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg_MonsterHunting
{
    public interface Scene
    {
        //Scene 인터페이스에 다음 씬 load 함수
        public void loadScene(UI ui, Character character);
    }
}
