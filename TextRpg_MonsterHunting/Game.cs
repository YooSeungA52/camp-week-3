using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg_MonsterHunting
{
    public class Game
    {
        SceneManager sceneManager;
        QuestManager questManager;

        IntroScene _introScene;
        UI _ui = new UI(); //UI 객체화
        bool leaveTown = false; //게임 실행 상태


        public Game()
        {
            sceneManager = new SceneManager();
            questManager = new QuestManager();
            _introScene = new IntroScene();
        }

        //게임 시작 method
        public void Start()
        {
            while (!leaveTown)
            {
                _introScene.loadScene(_ui); //이름, 직업 받은 후 마을로 이동
            }
        }
    }
}
