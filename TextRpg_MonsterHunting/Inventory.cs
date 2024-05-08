using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TextRpg_MonsterHunting
{
    // 인벤토리 클래스
    public class Inventory
    {
        // 장착템 목록
        [JsonInclude]
        public EquipmentList EquipmentsInBag { get; private set; }

        // 포션 목록
        public PotionList PotionsInBag { get; private set; }

        // 신체 부위별 장착 아이템
        public Equipment? Body { get; set; }
        public Equipment? RightHand { get; set; }

        //Json 불러오는용 초기화 메소드
        [JsonConstructor]
        public Inventory(EquipmentList EquipmentsInBag, PotionList PotionsInBag, Equipment? Body, Equipment? RightHand)
        {
            this.EquipmentsInBag = EquipmentsInBag;
            this.PotionsInBag = PotionsInBag;
            this.Body = Body;
            this.RightHand = RightHand;
        }

        //초기화
        public Inventory()
        {
            EquipmentsInBag = new EquipmentList();
            PotionsInBag = new PotionList();
        }

        //인벤토리에 아이템 추가
        public void Add(Item item)
        {
            if (item is Equipment)
            {
                Equipment equipment = (Equipment)item;
                EquipmentsInBag.Add(equipment);
            }
            else if (item is Potion)
            {
                Potion potion = (Potion)item;
                PotionsInBag.Add(potion);
            }
        }

        //인벤토리에서 아이템 제거
        public void Remove(Item item)
        {
            if (item is Equipment)
            {
                //장착템의 경우 장착 되어있으면 장착 해제 후 제거
                Equipment equipment = (Equipment)item;
                if (equipment.Equipped)
                {
                    if (equipment.EquipType == EquipmentType.OneHand)
                    {
                        RightHand = null;
                    }
                    else if (equipment.EquipType == EquipmentType.Body)
                    {
                        Body = null;
                    }
                    equipment.Equipped = false;
                }
                EquipmentsInBag.Remove(equipment);
            }
            else if (item is Potion)
            {
                Potion potion = (Potion)item;
                PotionsInBag.Remove(potion);
            }
        }

        //장비 장착
        //해당 신체부위에 넣어주고, 해당 아이템의 equipped = true
        //이미 장착된 템이 있으면 교체
        public void EquipItem(Equipment item)
        {
            if (item.EquipType == EquipmentType.Body)
            {
                if (Body != null)
                {
                    UnEquipItem(Body);
                }
                Body = item;
            }
            else if (item.EquipType == EquipmentType.OneHand)
            {
                if (RightHand != null)
                {
                    UnEquipItem(RightHand);
                }
                RightHand = item;              
            }
            item.Equipped = true;
            QuestManager.Instance.Quests[1].CheckQuestProgress();
        }

        //장착 해제
        //해제된 아이템 반환
        public Equipment UnEquipItem(Equipment item)
        {
            if (item.EquipType == EquipmentType.Body)
            {
                Body = null;
                item.Equipped = false;
            }
            else if (item.EquipType == EquipmentType.OneHand)
            {
                RightHand = null;
                item.Equipped = false;
            }
            return item;
        }

        //인벤토리 창
        //장착아이템, 소모템 목록을 표시
        public void PrintItems(bool isManagement) //ManageItems
        {
			Console.WriteLine("[장착 아이템 목록]");
			//EquipmentList equipmentList = EquipmentsInBag;
			for (int i = 0; i < EquipmentsInBag.Data.Count; i++)
			{
				Equipment item = EquipmentsInBag[i];
				string name = item.Name;                      //이름
															  //EquipmentType equipmentType = item.EquipType; //장비(방어구, 무기) 타입
				int stat = item.Stat;                         //스탯
				string discription = item.Discription;        //설명
				ItemType itemType = item.ItemType;            //아이템이 올려주는 능력치 타입
				bool equipped = item.Equipped;                //장착 상태
                if(isManagement)
				    Console.Write($"- {i + 1}. ");
                else
					Console.Write($"- ");

				if (equipped == false)
				{                    //({equipmentType})
					Console.WriteLine($"{name} | {itemType}+{stat} | {discription}");
				}
				else
				{
					Console.WriteLine($"[E]{name} | {itemType}+{stat} | {discription}");
				}
			}
			Console.WriteLine();
		}

        public void ManageEquipments(int userInput)
        {
            if (userInput <= 0)
                return;
            Equipment item = EquipmentsInBag[userInput - 1];

            if (item.Equipped)//장착된 아이템 해제
            {
                UnEquipItem(item);
            }
            else//새로운 아이템은 장착
            {
                EquipItem(item);
            }
        }

        //회복 (포션 아이템) 출력 창
        public void PrintPotionItems()
        {
            int healthPotionCount = 0;
            int manaPotionCount = 0;

            foreach (var item in PotionsInBag.Data)
            {
                if (item.ItemType == ItemType.Health)
                {
                    healthPotionCount++;
                }
                else if (item.ItemType == ItemType.Mana)
                {
                    manaPotionCount++;
                }
            }

            int i = 1;
            var manaPotion = Utils.ManaPotion;
            var healthPotion = Utils.HealthPotion;
			Console.WriteLine($"- {i++}. {healthPotion.Name} | {healthPotion.ItemType}+{healthPotion.Stat} | {healthPotionCount} 개");
			Console.WriteLine($"- {i++}. {manaPotion.Name} | {manaPotion.ItemType}+{manaPotion.Stat} | {manaPotionCount} 개");
		}

        //포션 아이템 사용 시 아이템 개수 감소 및 체력/마나 증가
        public bool ManagePotions(int userInput, Character character)
        {
            bool skipEnter = false;
            Console.Clear();
            double UseHealthPotion = 30;
            double UseManaPotion = 30;
            double beforeHealth = character.CurrentHealth;
            double beforeMana = character.CurrentMana;

			int healthPotionCount = 0;
			int manaPotionCount = 0;

			foreach (var item in PotionsInBag.Data)
			{
				if (item.ItemType == ItemType.Health)
				{
					healthPotionCount++;
				}
				else if (item.ItemType == ItemType.Mana)
				{
					manaPotionCount++;
				}
			}

			if (userInput == 1 && healthPotionCount > 0) //체력 포션 사용
            {
                //소지 중인 체력 포션 1개 감소
                PotionsInBag.Remove(Utils.HealthPotion);

                //현재 체력 30 증가
                character.ChangeHealth(UseHealthPotion);

                Console.WriteLine("체력 회복을 완료했습니다.");
				Console.WriteLine($"HP {beforeHealth} -> {character.CurrentHealth}");
			}
            else if (userInput == 2 && manaPotionCount > 0) //마나 포션 사용
            {
                //소지 중인 마나 포션 1개 감소
                PotionsInBag.Remove(Utils.ManaPotion);

                //현재 마나 30 증가
                character.ChangeMana(UseManaPotion);

                Console.WriteLine("마나 회복을 완료했습니다.");
				Console.WriteLine($"MP {beforeMana} -> {character.CurrentMana}");
			}
            else if(userInput == 0)
            {
				skipEnter = true;

			}
            else
            {
                Console.WriteLine("포션이 부족합니다.");
            }
            return skipEnter;
		}
    }
}
