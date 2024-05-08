using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TextRpg_MonsterHunting
{
    /*게임 동작에 필요한 기타 기능들
	 * enum , method, List<class>대체 함수 등
	*/
    // 직업
    public enum GameClassType
    {
        Warrior = 1, // 전사
        Wizard,      // 마법사
        Archer       // 궁수
    }

    internal static class Utils
	{
		//json파일 이름
		public static string PlayerFileName = "Player.json";
		public static string ShopItemsFileName = "ShopItems.json";

		public static Potion HealthPotion = new Potion("체력 포션", 30, "현재 체력을 30 회복합니다.", ItemType.Health, 10);
		public static Potion ManaPotion = new Potion("마나 포션", 30, "현재 마나를 30 회복합니다.", ItemType.Mana, 10);
		public static Equipment Sword = new Equipment("단검", EquipmentType.OneHand, 5, "추가 공격력이 5 증가합니다.", ItemType.Attack, 5);

		//영웅 저장 함수
		public static void SaveHero(Character hero)
		{
			var options = new JsonSerializerOptions
			{
				WriteIndented = true // 들여쓰기 설정
			};
			string jsonString = JsonSerializer.Serialize(hero, options);
			File.WriteAllText(PlayerFileName, jsonString);
		}

		//상점 아이템 저장 함수
		public static void SaveShopItems(EquipmentList shopItemList)
		{
			var options = new JsonSerializerOptions
			{
				WriteIndented = true // 들여쓰기 설정
			};
			string jsonString = JsonSerializer.Serialize(shopItemList, options);
			File.WriteAllText(ShopItemsFileName, jsonString);
		}

		//영웅 로딩 함수
		//로딩 성공시 true 반환
		public static bool LoadHero(out Character hero)
		{
			hero = null; // 초기화
			string? jsonString = null;
			try
			{
				jsonString = File.ReadAllText(PlayerFileName);
			}
			catch { }
			if (jsonString != null)
			{
				hero = JsonSerializer.Deserialize<Character>(jsonString);
				return true;
			}

			return false;
		}

		//상점 아이템 로딩 함수
		//로딩 성공시 true 반환
		public static bool LoadShopItems(out EquipmentList shopItemList)
		{
			shopItemList = null; // 초기화
			string? jsonString = null;
			try
			{
				jsonString = File.ReadAllText(ShopItemsFileName);
			}
			catch { }
			if (jsonString != null)
			{
				shopItemList = JsonSerializer.Deserialize<EquipmentList>(jsonString);
				return true;
			}

			return false;
		}

		//저장 파괴 함수
		//플레이어 사망시 또는 인위적으로 작동
		public static void SaveDestory()
		{
			if (File.Exists(PlayerFileName))
			{
				File.Delete(PlayerFileName);
			}
			if (File.Exists(ShopItemsFileName))
			{
				File.Delete(ShopItemsFileName);
			}

			Console.WriteLine("세이브가 초기화되고 종료됩니다!");
		}
	}

	//캐릭터, 몬스터 용 인터페이스
	public interface Humanoid
	{
		public string Name { get; }
		public double TotalAttackPower { get; }
		public double CurrentHealth { get; }
		public bool IsDie { get; }

		//체력 감소 또는 증가
		public void ChangeHealth(double changeHealth);

		// 공격 기능, 피해량 반환
		public double BasicAttack();
	}

	//json 저장용 List<Item> 대체 클래스
	public class ItemList
	{
		[JsonInclude]
		public List<Item> Data { get; private set; }

		[JsonInclude]
		public int Count { get; private set; }

		//Json 저장용 constructor
		[JsonConstructor]
		public ItemList(List<Item> Data, int Count)
		{
			this.Data = Data;
			this.Count = Count;
		}

		public ItemList()
		{
			this.Data = new List<Item>();
			this.Count = 0;
		}

		//기존 List의 Add 메소드를 이 클래스에 맞게 변환
		public void Add(Item item)
		{
			Data.Add(item);
			Count++;
		}

		//기존 List의 Remove 메소드를 이 클래스에 맞게 변환
		public void Remove(Item item)
		{
			Data.Remove(item);
			Count--;
		}

		//기존 List의 Contains 메소드를 이 클래스에 맞게 변환
		public bool Contains(Item item)
		{
			return Data.Contains(item);
		}

		// Sets or Gets the element at the given index.
		// 리스트가 [index]로 가져오는 함수를 가져와서 따라함
		public Item this[int index]
		{
			get
			{
				// Following trick can reduce the range check by one
				if ((uint)index >= (uint)Count)
				{
					throw new IndexOutOfRangeException("Index 범위 초과!!");
				}
				return Data[index];
			}

			set
			{
				if ((uint)index >= (uint)Count)
				{
					throw new IndexOutOfRangeException("Index 범위 초과!!");
				}
				Data[index] = value;
			}
		}
	}

	//json 저장용 List<Equipment> 대체 클래스
	public class EquipmentList
	{
		[JsonInclude]
		public List<Equipment> Data { get; set; }
		[JsonInclude]
		public int Count { get; private set; }

		[JsonConstructor]
		public EquipmentList(List<Equipment> Data, int Count)
		{
			this.Data = Data;
			this.Count = Count;
		}

		public EquipmentList()
		{
			this.Data = new List<Equipment>();
			this.Count = 0;
		}

		public void Add(Equipment item)
		{
			Data.Add(item);
			Count++;
		}

		public void Remove(Equipment item)
		{
			Data.Remove(item);
			Count--;
		}

		public bool Contains(Equipment item)
		{
			return Data.Contains(item);
		}
		public Equipment this[int index]
		{
			get
			{
				// Following trick can reduce the range check by one
				if ((uint)index >= (uint)Count)
				{
					throw new IndexOutOfRangeException("Index 범위 초과!!");
				}
				return Data[index];
			}

			set
			{
				if ((uint)index >= (uint)Count)
				{
					throw new IndexOutOfRangeException("Index 범위 초과!!");
				}
				Data[index] = value;
			}
		}
	}

	//json 저장용 List<Potion> 대체 클래스
	public class PotionList
	{
		[JsonInclude]
		public List<Potion> Data { get; set; }
		[JsonInclude]
		public int Count { get; private set; }

		[JsonConstructor]
		public PotionList(List<Potion> Data, int Count)
		{
			this.Data = Data;
			this.Count = Count;
		}

		public PotionList()
		{
			this.Data = new List<Potion>();
			this.Count = 0;
		}

		public void Add(Potion item)
		{
			Data.Add(item);
			Count++;
		}

		public void Remove(Potion item)
		{
			foreach(Potion potion in Data)
			{
				if(potion.ItemType == item.ItemType)
				{
					Data.Remove(potion);
					break;
				}
			}
			Count--;
		}

		public bool Contains(Potion item)
		{
			return Data.Contains(item);
		}
		public Potion this[int index]
		{
			get
			{
				// Following trick can reduce the range check by one
				if ((uint)index >= (uint)Count)
				{
					throw new IndexOutOfRangeException("Index 범위 초과!!");
				}
				return Data[index];
			}

			set
			{
				if ((uint)index >= (uint)Count)
				{
					throw new IndexOutOfRangeException("Index 범위 초과!!");
				}
				Data[index] = value;
			}
		}
	}

    //json 저장용 List<Skill> 대체 클래스
    public class SkillList
    {
        [JsonInclude]
        public List<Skill> Data { get; private set; }

        [JsonInclude]
        public uint Count { get; private set; }

        //Json 저장용 constructor
        [JsonConstructor]
        public SkillList(List<Skill> Data, uint Count)
        {
            this.Data = Data;
            this.Count = Count;
        }

        public SkillList()
        {
            this.Data = new List<Skill>();
            this.Count = 0;
        }

        //기존 List의 Add 메소드를 이 클래스에 맞게 변환
        public void Add(Skill skill)
        {
            Data.Add(skill);
            Count++;
        }

        //기존 List의 Remove 메소드를 이 클래스에 맞게 변환
        public void Remove(Skill skill)
        {
            Data.Remove(skill);
            Count--;
        }

        //기존 List의 Contains 메소드를 이 클래스에 맞게 변환
        public bool Contains(Skill skill)
        {
            return Data.Contains(skill);
        }

        // Sets or Gets the element at the given index.
        // 리스트가 [index]로 가져오는 함수를 가져와서 따라함
        public Skill this[int index]
        {
            get
            {
                // Following trick can reduce the range check by one
                if ((uint)index >= (uint)Count)
                {
                    throw new IndexOutOfRangeException("Index 범위 초과!!");
                }
                return Data[index];
            }

            set
            {
                if ((uint)index >= (uint)Count)
                {
                    throw new IndexOutOfRangeException("Index 범위 초과!!");
                }
                Data[index] = value;
            }
        }
    }
}
