using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TextRpg_MonsterHunting
{	class Shop
	{
		public EquipmentList soldItems;

		[JsonIgnore]
		Character _customer;

		[JsonIgnore]
		UI _ui;

		//기본 세팅
		//기본 아이템 추가		
		public Shop(Character customer, UI ui)
		{
			bool isItemListLoaded = Utils.LoadShopItems(out soldItems);
			if (!isItemListLoaded)
			{
				soldItems = new EquipmentList();
				Add(new Equipment("갑옷1", EquipmentType.Body, 1, "아주 단단한 갑옷", ItemType.Defence, 400));
				Add(new Equipment("갑옷2", EquipmentType.Body, 2, "더욱 단단한 갑옷", ItemType.Defence, 700));
				Add(new Equipment("수련자 갑옷", EquipmentType.Body, 5, " 수련에 도움을 주는 갑옷입니다. ", ItemType.Defence, 2000));
				Add(new Equipment("무쇠갑옷", EquipmentType.Body, 9, " 수련에 도움을 주는 갑옷입니다. ", ItemType.Defence, 4000));
				Add(new Equipment("누더기", EquipmentType.Body, 3, " 이상하게 방어력이 올라갑니다. ", ItemType.Defence, 900));
				Add(new Equipment("무기1", EquipmentType.OneHand, 1, "날카로운 칼", ItemType.Attack, 300));
				Add(new Equipment("무기2", EquipmentType.OneHand, 2, "더욱 날카로운 칼", ItemType.Attack, 600));
				Add(new Equipment("스파르타의 단도", EquipmentType.OneHand, 7, "스파르타 전사들이 썼다는 단도입니다", ItemType.Attack, 3000));
			}
			_customer = customer;
			_ui = ui;
		}

		/*
		 * 상점 아이템 확인창
		 * 1을 누르면 selling상태가 활성화 되고 판매창 기능이 같이 켜짐
		 */
		public void ShowItems(UI ui, Character character)
		{
			bool exitShop = false;
			bool selling = false;
			bool buyFromCustomer = false;
			while (!exitShop)
			{
				Console.Clear();
				Console.Write("상점");
				if (buyFromCustomer)
				{
					Console.Write(" - 아이템 판매");
				}
				Console.WriteLine("\n필요한 아이템을 얻을 수 있는 상점입니다.\n");
				Console.WriteLine("[보유 골드]");
				Console.WriteLine($"{_customer.Gold} G");

				if (buyFromCustomer)
				{
					Console.WriteLine("\n[인벤토리 장비 아이템 목록]");
					for (int i = 0; i < _customer.inventory.EquipmentsInBag.Count; i++)
					{
						Item item = _customer.inventory.EquipmentsInBag[i];
						Console.WriteLine($"- {i + 1} {item.Name} | {item.GetType()} +{item.Stat} | {item.Discription} | {Math.Round(item.Price * 0.85f)} G");
					}
				}
				else
				{
					Console.WriteLine("\n[상점 아이템 목록]");
					for (int i = 0; i < soldItems.Count; i++)
					{
						Equipment item = soldItems[i];
						if (selling)
						{
							Console.Write($"- {i + 1} ");
						}
						else
						{
							Console.Write($"- ");
						}
						Console.WriteLine($"{item.Name} | {item.GetType()} +{item.Stat} | {item.Discription} | {item.Price} G");
					}
				}
				if (!selling && !buyFromCustomer)
				{
					Console.WriteLine("\n1. 아이템 구매");
					Console.WriteLine("\n2. 아이템 판매");
				}
				Console.WriteLine("\n0. 나가기");
				int input;
				if (selling)
				{
					input = _ui.UserChoiceInput(0, soldItems.Count);
				}
				else if (buyFromCustomer)
				{
					input = _ui.UserChoiceInput(0, _customer.inventory.EquipmentsInBag.Count);
				}
				else
				{
					input = _ui.UserChoiceInput(0, 2);
				}
				switch (input)
				{
					case 0:// 뒤로 나가기
						if (selling)
						{
							selling = false;
						}
						else if (buyFromCustomer)
						{
							buyFromCustomer = false;
						}
						else
						{
							exitShop = true;
						}
						break;
					default:
						if (!buyFromCustomer && !selling && input == 1) // 아이템 구매중이 아닐 때는 구매창으로 이동
						{
							selling = true;
						}
						else if (!selling && !buyFromCustomer && input == 2) // 아이템 판매중이 아닐 때는 판매창으로 이동
						{
							buyFromCustomer = true;
						}
						else if(selling) // 상점에서 구매
						{
							Equipment item = soldItems[input - 1];
							if (_customer.Gold >= item.Price)
							{
								Console.WriteLine("구매를 완료했습니다.");
								_customer.ChangeGold(-item.Price);
								_customer.inventory.Add(item);
								soldItems.Remove(item);
								Utils.SaveHero(_customer);
								Utils.SaveShopItems(this.soldItems);//구매 시 세이브
							}
							else
							{
								Console.WriteLine("Gold 가 부족합니다.");
							}
						}
						else if (buyFromCustomer) //상점에 판매
						{
							Equipment item = _customer.inventory.EquipmentsInBag[input - 1];
							_customer.ChangeGold((int)(item.Price*0.85f));
							_customer.inventory.Remove(item);
							Add(item);
							Utils.SaveHero(_customer);
							Utils.SaveShopItems(this.soldItems);//판매 시 세이브
						}
						break;
				}
			}
			SceneManager.Instance._startScene.loadScene(ui, character); //마을로 돌아가기
		}

		//상점에 아이템 추가
		public void Add(Equipment item)
		{
			soldItems.Add(item);
		}

		//상점에서 아이템 제거
		public void Remove(Equipment item)
		{
			soldItems.Remove(item);
		}
	}
}
