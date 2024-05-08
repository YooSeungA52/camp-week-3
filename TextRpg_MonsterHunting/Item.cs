using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace TextRpg_MonsterHunting
{
	//아이템이 올리는 능력치 타입
	public enum ItemType
	{
		Health = 1,
		Attack,
		Defence,
		Mana
	}

	//아이템 장비 신체 부위 타입
	public enum EquipmentType
	{
		Body = 1,
		OneHand
	}

	//아이템 인터페이스
	public interface Item
	{
		public string Name { get; }
		public string Discription { get; }
		public int Price { get; }
		public ItemType ItemType { get; }
		public int Stat { get; }
		public string GetType();
	}

	//장비 아이템 클래스
	public class Equipment : Item
	{
		public string Name { get; private set; }
		public EquipmentType EquipType { get; private set; }
		public int Stat { get; private set; }
		public string Discription { get; private set; }
		public int Price { get; private set; }
		public ItemType ItemType { get; private set; }

		public bool Equipped { get; set; }

		// Json 불러오는용 생성자
		[JsonConstructor]
		public Equipment(string Name, EquipmentType EquipType, int Stat, string Discription, int Price,
			ItemType ItemType, bool Equipped)
		{
			this.Name = Name;
			this.EquipType = EquipType;
			this.Stat = Stat;
			this.Discription = Discription;
			this.ItemType = ItemType;
			this.Equipped = Equipped;
			this.Price = Price;
		}

		public Equipment(string name, EquipmentType equipType, int stat, string discription,
			 ItemType itemType, int price)
		{
			Name = name;
			EquipType = equipType;
			Stat = stat;
			Discription = discription;
			ItemType = itemType;
			Equipped = false;
			Price = price;
		}

		//장비 아이템 스탯 정보 출력
		public string GetType()
		{
			switch(ItemType)
			{
				case ItemType.Mana:
					return "마나";
					break;
				case ItemType.Attack:
					return "공격력";
					break;
				case ItemType.Health:
					return "체력";
					break;
				case ItemType.Defence:
					return "방어력";
					break;
				default:
					return "";
					break;
			}
		}
	}

	public class Potion : Item
	{
		public string Name { get; private set; }
		public int Stat { get; private set; }
		public string Discription { get; private set; }
		public ItemType ItemType { get; private set; }
		public int Price { get; private set; }

		// Json 불러오는용 생성자
		[JsonConstructor]
		public Potion(string Name, int Stat, string Discription, ItemType ItemType, int Price)
		{
			this.Name = Name;
			this.Stat = Stat;
			this.Discription = Discription;
			this.ItemType = ItemType;
			this.Price = Price;
		}

		//아이템 소비 기능
		//소비된 아이템은 inventory에서 제거(주인공에 인벤토리 추가시 구현)
		public void Use(Character hero)
		{
			switch (ItemType)
			{
				case ItemType.Health:
					hero.ChangeHealth(Stat);
					break;
				case ItemType.Attack:
					hero.ChangeAttack(Stat);
					break;
				case ItemType.Defence:
					hero.ChangeDefense(Stat);
					break;
				case ItemType.Mana:
					//마나 수정 필요
					break;
			}
			//hero.Inventory.Remove(this);
		}

		//장비 아이템 스탯 정보 출력
		public string GetType()
		{
			switch (ItemType)
			{
				case ItemType.Mana:
					return "마나";
					break;
				case ItemType.Attack:
					return "공격력";
					break;
				case ItemType.Health:
					return "체력";
					break;
				case ItemType.Defence:
					return "방어력";
					break;
				default:
					return "";
					break;
			}
		}
	}

}
