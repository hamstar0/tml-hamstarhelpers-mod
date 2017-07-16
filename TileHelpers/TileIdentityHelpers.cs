using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace HamstarHelpers.TileHelpers {
	public static class TileIdentityHelpers {
		public static IDictionary<int, IDictionary<int, string>> Data;

		static TileIdentityHelpers() {
			try {
			TileIdentityHelpers.Data = new Dictionary<int, IDictionary<int, string>>();

			TileIdentityHelpers.Data[0] = new Dictionary<int, string> {
				{ -1, "Dirt Block" }
			};
			TileIdentityHelpers.Data[1] = new Dictionary<int, string> {
				{ -1, "Stone Block" }
			};
			TileIdentityHelpers.Data[2] = new Dictionary<int, string> {
				{ -1, "Grass" }
			};
			TileIdentityHelpers.Data[3] = new Dictionary<int, string> {
				{ 9, "Mushroom" },
				{ 1, "Short Grass Plants" },
				{ 2, "Short Grass Plants" },
				{ 3, "Short Grass Plants" },
				{ 4, "Short Grass Plants" },
				{ 5, "Short Grass Plants" },
				{ 6, "Short Grass Plants" },
				{ 7, "Short Grass Plants" },
				{ 8, "Short Grass Plants" },
				{ 10, "Short Grass Plants" },
				{ 11, "Short Grass Plants" },
				{ 12, "Short Grass Plants" },
				{ 13, "Short Grass Plants" },
				{ 14, "Short Grass Plants" },
				{ 15, "Short Grass Plants" },
				{ 16, "Short Grass Plants" },
				{ 17, "Short Grass Plants" },
				{ 18, "Short Grass Plants" },
				{ 19, "Short Grass Plants" },
				{ 20, "Short Grass Plants" },
				{ 21, "Short Grass Plants" },
				{ 22, "Short Grass Plants" },
				{ -1, "Short Grass Plants" }
			};
			TileIdentityHelpers.Data[4] = new Dictionary<int, string> {
				{ 2, "Blue Torch" },
				{ 3, "Red Torch" },
				{ 4, "Green Torch" },
				{ 5, "Purple Torch" },
				{ 6, "White Torch" },
				{ 7, "Yellow Torch" },
				{ 8, "Demon Torch" },
				{ 9, "Cursed Torch" },
				{ 10, "Ice Torch" },
				{ 11, "Orange Torch" },
				{ 12, "Ichor Torch" },
				{ 13, "Ultrabright Torch" },
				{ 14, "Bone Torch" },
				{ 15, "Rainbow Torch" },
				{ 16, "Pink Torch" },
				{ -1, "Torch" }
			};
			TileIdentityHelpers.Data[5] = new Dictionary<int, string> {
				{ 0, "Corrupt Tree" },
				{ 1, "Jungle Tree" },
				{ 2, "Hallow Tree" },
				{ 3, "Boreal Tree" },
				{ 4, "Crimson Tree" },
				{ 5, "Living Mahogany Tree" },
				{ 6, "Giant Glowing Mushroom (surface)" },
				{ -1, "Tree" }
			};
			TileIdentityHelpers.Data[6] = new Dictionary<int, string> {
				{ -1, "Iron Ore" }
			};
			TileIdentityHelpers.Data[7] = new Dictionary<int, string> {
				{ -1, "Copper Ore" }
			};
			TileIdentityHelpers.Data[8] = new Dictionary<int, string> {
				{ -1, "Gold Ore" }
			};
			TileIdentityHelpers.Data[9] = new Dictionary<int, string> {
				{ -1, "Silver Ore" }
			};
			TileIdentityHelpers.Data[10] = new Dictionary<int, string> {
				{ 2, "Ebonwood Door (closed)" },
				{ 3, "Rich Mahogany Door (closed)" },
				{ 4, "Pearlwood Door (closed)" },
				{ 5, "Cactus Door (closed)" },
				{ 6, "Flesh Door (closed)" },
				{ 7, "Mushroom Door (closed)" },
				{ 8, "Living Wood Door (closed)" },
				{ 9, "Bone Door (closed)" },
				{ 10, "Skyware Door (closed)" },
				{ 11, "Shadewood Door (closed)" },
				{ 12, "Lihzahrd Door (locked)" },
				{ 13, "Lihzahrd Door (unlocked (closed))" },
				{ 14, "Dungeon Door (closed)" },
				{ 15, "Lead Door (closed)" },
				{ 16, "Iron Door (closed)" },
				{ 17, "Blue Dungeon Door (closed)" },
				{ 18, "Green Dungeon Door (closed)" },
				{ 19, "Pink Dungeon Door (closed)" },
				{ 20, "Obsidian Door (closed)" },
				{ 21, "Glass Door (closed)" },
				{ 22, "Golden Door (closed)" },
				{ 23, "Honey Door (closed)" },
				{ 24, "Steam Punk Door (closed)" },
				{ 25, "Pumpkin Door (closed)" },
				{ 26, "Spooky Door (closed)" },
				{ 27, "Pine Door (closed)" },
				{ 28, "Frozen Door (closed)" },
				{ 29, "Dynasty Door (closed)" },
				{ 30, "Palm Wood Door (closed)" },
				{ 31, "Boreal Wood Door (closed)" },
				{ 32, "Slime Door (closed)" },
				{ 33, "Martian Door (closed)" },
				{ 34, "Meteorite Door  (closed)" },
				{ 35, "Granite Door  (closed)" },
				{ 36, "Marble Door  (closed)" },
				{ 37, "Crystal Door  (closed)" },
				{ -1, "Wooden Door (closed)" }
			};
			TileIdentityHelpers.Data[11] = new Dictionary<int, string> {
				{ 2, "Ebonwood Door (open)" },
				{ 3, "Rich Mahogany Door (open)" },
				{ 4, "Pearlwood Door (open)" },
				{ 5, "Cactus Door (open)" },
				{ 6, "Flesh Door (open)" },
				{ 7, "Mushroom Door (open)" },
				{ 8, "Living Wood Door (open)" },
				{ 9, "Bone Door (open)" },
				{ 10, "Skyware Door (open)" },
				{ 11, "Shadewood Door (open)" },
				{ 12, "Lihzahrd Door (open)" },
				{ 13, "Lihzahrd Door (open)" },
				{ 14, "Dungeon Door (open)" },
				{ 15, "Lead Door (open)" },
				{ 16, "Iron Door (open)" },
				{ 17, "Blue Dungeon Door (open)" },
				{ 18, "Green Dungeon Door (open)" },
				{ 19, "Pink Dungeon Door (open)" },
				{ 20, "Obsidian Door (open)" },
				{ 21, "Glass Door (open)" },
				{ 22, "Golden Door (open)" },
				{ 23, "Honey Door (open)" },
				{ 24, "Steam Punk Door (open)" },
				{ 25, "Pumpkin Door (open)" },
				{ 26, "Spooky Door (open)" },
				{ 27, "Pine Door (open)" },
				{ 28, "Frozen Door (open)" },
				{ 29, "Dynasty Door (open)" },
				{ 30, "Palm Wood Door (open)" },
				{ 31, "Boreal Wood Door (open)" },
				{ 32, "Slime Door (open)" },
				{ 33, "Martian Door (open)" },
				{ 34, "Meteorite Door  (open)" },
				{ 35, "Granite Door  (open)" },
				{ 36, "Marble Door  (open)" },
				{ 37, "Crystal Door  (open)" },
				{ -1, "Wooden Door (open)" },
			};
			TileIdentityHelpers.Data[12] = new Dictionary<int, string> {
				{ -1, "Crystal Heart" }
			};
			TileIdentityHelpers.Data[13] = new Dictionary<int, string> {
				{ 2, "Lesser Healing Potion" },
				{ 3, "Lesser Mana Potion" },
				{ 4, "Pink Vase" },
				{ 5, "Mug" },
				{ 6, "Dynasty Cup" },
				{ 7, "Wine Glass" },
				{ 8, "Honey Cup" },
				{ 9, "Chalice" },
				{ -1, "Bottle" }
			};
			TileIdentityHelpers.Data[14] = new Dictionary<int, string> {
				{ 2, "Ebonwood Table" },
				{ 3, "Rich Mahogany Table" },
				{ 4, "Pearlwood Table" },
				{ 5, "Bone Table" },
				{ 6, "Flesh Table" },
				{ 7, "Living Wood Table" },
				{ 8, "Skyware Table" },
				{ 9, "Shadewood Table" },
				{ 10, "Lihzahrd Table" },
				{ 11, "Blue Dungeon Table" },
				{ 12, "Green Dungeon Table" },
				{ 13, "Pink Dungeon Table" },
				{ 14, "Obsidian Table" },
				{ 15, "Gothic Table" },
				{ 16, "Glass Table" },
				{ 17, "Banquet Table" },
				{ 18, "Bar" },
				{ 19, "Golden Table" },
				{ 20, "Honey Table" },
				{ 21, "Steam Punk Table" },
				{ 22, "Pumpkin Table" },
				{ 23, "Spooky Table" },
				{ 24, "Pine Table" },
				{ 25, "Frozen Table" },
				{ 26, "Dynasty Table" },
				{ 27, "Palm Wood Table" },
				{ 28, "Mushroom Table" },
				{ 29, "Boreal Wood Table" },
				{ 30, "Slime Table" },
				{ 31, "Cactus Table" },
				{ 32, "Martian Table" },
				{ 33, "Meteorite Table" },
				{ 34, "Granite Table" },
				{ 35, "Marble Table" },
				{ 36, "Crystal Table" },
				{ -1, "Wooden Table" }
			};
			TileIdentityHelpers.Data[15] = new Dictionary<int, string> {
				{ 2, "Toilet" },
				{ 3, "Ebonwood Chair" },
				{ 4, "Rich Mahogany Chair" },
				{ 5, "Pearlwood Chair" },
				{ 6, "Living Wood Chair" },
				{ 7, "Cactus Chair" },
				{ 8, "Bone Chair" },
				{ 9, "Flesh Chair" },
				{ 10, "Mushroom Chair" },
				{ 11, "Skyware Chair" },
				{ 12, "Shadewood Chair" },
				{ 13, "Lihzahrd Chair" },
				{ 14, "Blue Dungeon Chair" },
				{ 15, "Green Dungeon Chair" },
				{ 16, "Pink Dungeon Chair" },
				{ 17, "Obsidian Chair" },
				{ 18, "Gothic Chair" },
				{ 19, "Glass Chair" },
				{ 20, "Golden Chair" },
				{ 21, "Golden Toilet" },
				{ 22, "Bar Stool" },
				{ 23, "Honey Chair" },
				{ 24, "Steam Punk Chair" },
				{ 25, "Pumpkin Chair" },
				{ 26, "Spooky Chair" },
				{ 27, "Pine Chair" },
				{ 28, "Dynasty Chair" },
				{ 29, "Frozen Chair" },
				{ 30, "Palm Wood Chair" },
				{ 31, "Boreal Wood Chair" },
				{ 32, "Slime Chair" },
				{ 33, "Martian Hover Chair" },
				{ 34, "Meteorite Chair" },
				{ 35, "Granite Chair" },
				{ 36, "Marble Chair" },
				{ 37, "Crystal Chair" },
				{ -1, "Wooden Chair" }
			};
			TileIdentityHelpers.Data[16] = new Dictionary<int, string> {
				{ 2, "Lead Anvil" },
				{ -1, "Iron Anvil" }
			};
			TileIdentityHelpers.Data[17] = new Dictionary<int, string> {
				{ -1, "Furnace" }
			};
			TileIdentityHelpers.Data[18] = new Dictionary<int, string> {
				{ 2, "Ebonwood Work Bench" },
				{ 3, "Rich Mahogany Work Bench" },
				{ 4, "Pearlwood Work Bench" },
				{ 5, "Bone Work Bench" },
				{ 6, "Cactus Work Bench" },
				{ 7, "Flesh Work Bench" },
				{ 8, "Mushroom Work Bench" },
				{ 9, "Slime Work Bench" },
				{ 10, "Shadewood Work Bench" },
				{ 11, "Lihzahrd Work Bench" },
				{ 12, "Blue Dungeon Work Bench" },
				{ 13, "Green Dungeon Work Bench" },
				{ 14, "Pink Dungeon Work Bench" },
				{ 15, "Obsidian Work Bench" },
				{ 16, "Gothic Work Bench" },
				{ 17, "Pumpkin Work Bench" },
				{ 18, "Spooky Work Bench" },
				{ 19, "Dynasty Work Bench" },
				{ 20, "Honey Work Bench" },
				{ 21, "Frozen Work Bench" },
				{ 22, "Steampunk Work Bench" },
				{ 23, "Palm Wood Work Bench" },
				{ 24, "Boreal Wood Work Bench" },
				{ 25, "Skyware Work Bench" },
				{ 26, "Glass Work Bench" },
				{ 27, "Living Wood Work Bench" },
				{ 28, "Martian Work Bench" },
				{ 29, "Meteorite Work Bench" },
				{ 30, "Granite Work Bench" },
				{ 31, "Marble Work Bench" },
				{ 32, "Crystal Work Bench" },
				{ 33, "Golden Work Bench" },
				{ -1, "Work Bench" }
			};
			TileIdentityHelpers.Data[19] = new Dictionary<int, string> {
				{ 2, "Ebonwood Platform" },
				{ 3, "Rich Mahogany Platform" },
				{ 4, "Pearlwood Platform" },
				{ 5, "Bone Platform" },
				{ 6, "Shadewood Platform" },
				{ 7, "Blue Brick Platform" },
				{ 8, "Pink Brick Platform" },
				{ 9, "Green Brick Platform" },
				{ 10, "Metal Shelf" },
				{ 11, "Brass Shelf" },
				{ 12, "Wood Shelf" },
				{ 13, "Dungeon Shelf" },
				{ 14, "Obsidian Platform" },
				{ 15, "Glass Platform" },
				{ 16, "Pumpkin Platform" },
				{ 17, "Spooky Platform" },
				{ 18, "Palm Wood Platform" },
				{ 19, "Mushroom Platform" },
				{ 20, "Boreal Wood Platform" },
				{ 21, "Slime Platform" },
				{ 22, "Steampunk Platform" },
				{ 23, "Skyware Platform" },
				{ 24, "Living Wood Platform" },
				{ 25, "Honey Platform" },
				{ 26, "Cactus Platform" },
				{ 27, "Martian Platform" },
				{ 28, "Meteorite Platform" },
				{ 29, "Granite Platform" },
				{ 30, "Marble Platform" },
				{ 31, "Crystal Platform" },
				{ 32, "Golden Platform" },
				{ 33, "Dynasty Platform" },
				{ 34, "Lihzahrd Platform" },
				{ 35, "Flesh Platform" },
				{ 36, "Frozen Platform" },
				{ -1, "Wood Platform" }
			};
			TileIdentityHelpers.Data[20] = new Dictionary<int, string> {
				{ -1, "Acorn" }
			};
			TileIdentityHelpers.Data[21] = new Dictionary<int, string> {
				{ 2, "Gold Chest" },
				{ 3, "Locked Gold Chest" },
				{ 4, "Shadow Chest" },
				{ 5, "Locked Shadow Chest" },
				{ 6, "Barrel" },
				{ 7, "Trash Can" },
				{ 8, "Ebonwood Chest" },
				{ 9, "Rich Mahogany Chest" },
				{ 10, "Pearlwood Chest" },
				{ 11, "Ivy Chest" },
				{ 12, "Ice Chest" },
				{ 13, "Living Wood Chest" },
				{ 14, "Skyware Chest" },
				{ 15, "Shadewood Chest" },
				{ 16, "Web Covered Chest" },
				{ 17, "Lihzahrd Chest" },
				{ 18, "Water Chest" },
				{ 19, "Jungle Chest" },
				{ 20, "Corruption Chest" },
				{ 21, "Crimson Chest" },
				{ 22, "Hallowed Chest" },
				{ 23, "Frozen Chest" },
				{ 24, "Locked Jungle Chest" },
				{ 25, "Locked Corruption Chest" },
				{ 26, "Locked Crimson Chest" },
				{ 27, "Locked Hallowed Chest" },
				{ 28, "Locked Frozen Chest" },
				{ 29, "Dynasty Chest" },
				{ 30, "Honey Chest" },
				{ 31, "Steampunk Chest" },
				{ 32, "Palm Wood Chest" },
				{ 33, "Mushroom Chest" },
				{ 34, "Boreal Wood Chest" },
				{ 35, "Slime Chest" },
				{ 36, "Green Dungeon Chest" },
				{ 37, "Locked Green Dungeon Chest" },
				{ 38, "Pink Dungeon Chest" },
				{ 39, "Locked Pink Dungeon Chest" },
				{ 40, "Blue Dungeon Chest" },
				{ 41, "Locked Blue Dungeon Chest" },
				{ 42, "Bone Chest" },
				{ 43, "Cactus Chest" },
				{ 44, "Flesh Chest" },
				{ 45, "Obsidian Chest" },
				{ 46, "Pumpkin Chest" },
				{ 47, "Spooky Chest" },
				{ 48, "Glass Chest" },
				{ 49, "Martian Chest" },
				{ 50, "Meteorite Chest" },
				{ 51, "Granite Chest" },
				{ 52, "Marble Chest" },
				{ 53, "Crystal Chest" },
				{ 54, "Golden Chest" },
				{ -1, "Chest" }
			};
			TileIdentityHelpers.Data[22] = new Dictionary<int, string> {
				{ -1, "Demonite Ore" }
			};
			TileIdentityHelpers.Data[23] = new Dictionary<int, string> {
				{ -1, "Corrupt grass" }
			};
			TileIdentityHelpers.Data[24] = new Dictionary<int, string> {
				{ 9, "Vile Mushroom" },
				{ 1, "Corruption Plants" },
				{ 2, "Corruption Plants" },
				{ 3, "Corruption Plants" },
				{ 4, "Corruption Plants" },
				{ 5, "Corruption Plants" },
				{ 6, "Corruption Plants" },
				{ 7, "Corruption Plants" },
				{ 8, "Corruption Plants" },
				{ 10, "Corruption Plants" },
				{ 11, "Corruption Plants" },
				{ 12, "Corruption Plants" },
				{ 13, "Corruption Plants" },
				{ 14, "Corruption Plants" },
				{ 15, "Corruption Plants" },
				{ 16, "Corruption Plants" },
				{ 17, "Corruption Plants" },
				{ 18, "Corruption Plants" },
				{ 19, "Corruption Plants" },
				{ 20, "Corruption Plants" },
				{ 21, "Corruption Plants" },
				{ 22, "Corruption Plants" },
				{ 23, "Corruption Plants" },
				{ -1, "Corruption Plants" }
			};
			TileIdentityHelpers.Data[25] = new Dictionary<int, string> {
				{ -1, "Ebonstone Block" }
			};
			TileIdentityHelpers.Data[26] = new Dictionary<int, string> {
				{ 2, "Crimson Altar" },
				{ -1, "Demon Altar" }
			};
			TileIdentityHelpers.Data[27] = new Dictionary<int, string> {
				{ -1, "Sunflower" }
			};
			TileIdentityHelpers.Data[28] = new Dictionary<int, string> {
				{ -1, "Pot" }
			};
			TileIdentityHelpers.Data[29] = new Dictionary<int, string> {
				{ -1, "Piggy Bank" }
			};
			TileIdentityHelpers.Data[30] = new Dictionary<int, string> {
				{ -1, "Wood" }
			};
			TileIdentityHelpers.Data[31] = new Dictionary<int, string> {
				{ 2, "Crimson Heart" },
				{ -1, "Shadow Orb" }
			};
			TileIdentityHelpers.Data[32] = new Dictionary<int, string> {
				{ -1, "Corruption Thorny Bush" }
			};
			TileIdentityHelpers.Data[33] = new Dictionary<int, string> {
				{ 2, "Blue Dungeon Candle" },
				{ 3, "Green Dungeon Candle" },
				{ 4, "Pink Dungeon Candle" },
				{ 5, "Cactus Candle" },
				{ 6, "Ebonwood Candle" },
				{ 7, "Flesh Candle" },
				{ 8, "Glass Candle" },
				{ 9, "Frozen Candle" },
				{ 10, "Rich Mahogany Candle" },
				{ 11, "Pearlwood Candle" },
				{ 12, "Lihzahrd Candle" },
				{ 13, "Skyware Candle" },
				{ 14, "Pumpkin Candle" },
				{ 15, "Living Wood Candle" },
				{ 16, "Shadewood Candle" },
				{ 17, "Golden Candle" },
				{ 18, "Dynasty Candle" },
				{ 19, "Palm Wood Candle" },
				{ 20, "Mushroom Candle" },
				{ 21, "Boreal Wood Candle" },
				{ 22, "Slime Candle" },
				{ 23, "Honey Candle" },
				{ 24, "Steampunk Candle" },
				{ 25, "Spooky Candle" },
				{ 26, "Obsidian Candle" },
				{ 27, "Martian Hover Candle" },
				{ 28, "Meteorite Candle" },
				{ 29, "Granite Candle" },
				{ 30, "Marble Candle" },
				{ 31, "Crystal Candle" },
				{ -1, "Candle" }
			};
			TileIdentityHelpers.Data[34] = new Dictionary<int, string> {
				{ 2, "Silver Chandelier" },
				{ 3, "Gold Chandelier" },
				{ 4, "Tin Chandelier" },
				{ 5, "Tungsten Chandelier" },
				{ 6, "Platinum Chandelier" },
				{ 7, "Jackelier" },
				{ 8, "Cactus Chandelier" },
				{ 9, "Ebonwood Chandelier" },
				{ 10, "Flesh Chandelier" },
				{ 11, "Honey Chandelier" },
				{ 12, "Frozen Chandelier" },
				{ 13, "Rich Mahogany Chandelier" },
				{ 14, "Pearlwood Chandelier" },
				{ 15, "Lihzahrd Chandelier" },
				{ 16, "Skyware Chandelier" },
				{ 17, "Spooky Chandelier" },
				{ 18, "Glass Chandelier" },
				{ 19, "Living Wood Chandelier" },
				{ 20, "Shadewood Chandelier" },
				{ 21, "Golden Chandelier" },
				{ 22, "Bone Chandelier" },
				{ 23, "Large Dynasty Lantern" },
				{ 24, "Palm Wood Chandelier" },
				{ 25, "Mushroom Chandelier" },
				{ 26, "Boreal Wood Chandelier" },
				{ 27, "Slime Chandelier" },
				{ 28, "Blue Dungeon Chandelier" },
				{ 29, "Green Dungeon Chandelier" },
				{ 30, "Pink Dungeon Chandelier" },
				{ 31, "Steampunk Chandelier" },
				{ 32, "Pumpkin Chandelier" },
				{ 33, "Obsidian Chandelier" },
				{ 34, "Martian Chandelier" },
				{ 35, "Meteorite Chandelier" },
				{ 36, "Granite Chandelier" },
				{ 37, "Marble Chandelier" },
				{ 38, "Crystal Chandelier" },
				{ -1, "Copper Chandelier" }
			};
			TileIdentityHelpers.Data[35] = new Dictionary<int, string> {
				{ -1, "Jack 'O Lantern" }
			};
			TileIdentityHelpers.Data[36] = new Dictionary<int, string> {
				{ -1, "Present" }
			};
			TileIdentityHelpers.Data[37] = new Dictionary<int, string> {
				{ -1, "Meteorite" }
			};
			TileIdentityHelpers.Data[38] = new Dictionary<int, string> {
				{ -1, "Gray Brick" }
			};
			TileIdentityHelpers.Data[39] = new Dictionary<int, string> {
				{ -1, "Red Brick" }
			};
			TileIdentityHelpers.Data[40] = new Dictionary<int, string> {
				{ -1, "Clay Block" }
			};
			TileIdentityHelpers.Data[41] = new Dictionary<int, string> {
				{ -1, "Blue Brick" }
			};
			TileIdentityHelpers.Data[42] = new Dictionary<int, string> {
				{ 2, "Brass Lantern" },
				{ 3, "Caged Lantern" },
				{ 4, "Carriage Lantern" },
				{ 5, "Alchemy Lantern" },
				{ 6, "Diablost Lamp" },
				{ 7, "Oil Rag Sconse" },
				{ 8, "Star in a Bottle" },
				{ 9, "Hanging Jack 'O Lantern" },
				{ 10, "Heart Lantern" },
				{ 11, "Cactus Lantern" },
				{ 12, "Ebonwood Lantern" },
				{ 13, "Flesh Lantern" },
				{ 14, "Honey Lantern" },
				{ 15, "Steampunk Lantern" },
				{ 16, "Glass Lantern" },
				{ 17, "Rich Mahogany Lantern" },
				{ 18, "Pearlwood Lantern" },
				{ 19, "Frozen Lantern" },
				{ 20, "Lihzahrd Lantern" },
				{ 21, "Skyware Lantern" },
				{ 22, "Spooky Lantern" },
				{ 23, "Living Wood Lantern" },
				{ 24, "Shadewood Lantern" },
				{ 25, "Golden Lantern" },
				{ 26, "Bone Lantern" },
				{ 27, "Dynasty Lantern" },
				{ 28, "Palm Wood Lantern" },
				{ 29, "Mushroom Lantern" },
				{ 30, "Boreal Wood Lantern" },
				{ 31, "Slime Lantern" },
				{ 32, "Pumpkin Lantern" },
				{ 33, "Obsidian Lantern" },
				{ 34, "Martian Lantern" },
				{ 35, "Meteorite Lantern" },
				{ 36, "Granite Lantern" },
				{ 37, "Marble Lantern" },
				{ 38, "Crystal Lantern" },
				{ -1, "Chain Lantern" }
			};
			TileIdentityHelpers.Data[43] = new Dictionary<int, string> {
				{ -1, "Green Brick" }
			};
			TileIdentityHelpers.Data[44] = new Dictionary<int, string> {
				{ -1, "Pink Brick" }
			};
			TileIdentityHelpers.Data[45] = new Dictionary<int, string> {
				{ -1, "Gold Brick" }
			};
			TileIdentityHelpers.Data[46] = new Dictionary<int, string> {
				{ -1, "Silver Brick" }
			};
			TileIdentityHelpers.Data[47] = new Dictionary<int, string> {
				{ -1, "Copper Brick" }
			};
			TileIdentityHelpers.Data[48] = new Dictionary<int, string> {
				{ -1, "Spike" }
			};
			TileIdentityHelpers.Data[49] = new Dictionary<int, string> {
				{ -1, "Water Candle" }
			};
			TileIdentityHelpers.Data[50] = new Dictionary<int, string> {
				{ -1, "Book" }
			};
			TileIdentityHelpers.Data[51] = new Dictionary<int, string> {
				{ -1, "Cobweb" }
			};
			TileIdentityHelpers.Data[52] = new Dictionary<int, string> {
				{ -1, "Vines" }
			};
			TileIdentityHelpers.Data[53] = new Dictionary<int, string> {
				{ -1, "Sand Block" }
			};
			TileIdentityHelpers.Data[54] = new Dictionary<int, string> {
				{ -1, "Glass" }
			};
			TileIdentityHelpers.Data[55] = new Dictionary<int, string> {
				{ -1, "Sign" }
			};
			TileIdentityHelpers.Data[56] = new Dictionary<int, string> {
				{ -1, "Obsidian" }
			};
			TileIdentityHelpers.Data[57] = new Dictionary<int, string> {
				{ -1, "Ash Block" }
			};
			TileIdentityHelpers.Data[58] = new Dictionary<int, string> {
				{ -1, "Hellstone" }
			};
			TileIdentityHelpers.Data[59] = new Dictionary<int, string> {
				{ -1, "Mud Block" }
			};
			TileIdentityHelpers.Data[60] = new Dictionary<int, string> {
				{ -1, "Jungle Grass" }
			};
			TileIdentityHelpers.Data[61] = new Dictionary<int, string> {
				{ 9, "Jungle Spore" },
				{ 10, "Nature's Gift" },
				{ 1, "Short Jungle Plants" },
				{ 2, "Short Jungle Plants" },
				{ 3, "Short Jungle Plants" },
				{ 4, "Short Jungle Plants" },
				{ 5, "Short Jungle Plants" },
				{ 6, "Short Jungle Plants" },
				{ 7, "Short Jungle Plants" },
				{ 8, "Short Jungle Plants" },
				{ 11, "Short Jungle Plants" },
				{ 12, "Short Jungle Plants" },
				{ 13, "Short Jungle Plants" },
				{ 14, "Short Jungle Plants" },
				{ 15, "Short Jungle Plants" },
				{ 16, "Short Jungle Plants" },
				{ 17, "Short Jungle Plants" },
				{ 18, "Short Jungle Plants" },
				{ 19, "Short Jungle Plants" },
				{ 20, "Short Jungle Plants" },
				{ 21, "Short Jungle Plants" },
				{ 22, "Short Jungle Plants" },
				{ 23, "Short Jungle Plants" },
				{ -1, "Short Jungle Plants" }
			};
			TileIdentityHelpers.Data[62] = new Dictionary<int, string> {
				{ -1, "Jungle Vine" }
			};
			TileIdentityHelpers.Data[63] = new Dictionary<int, string> {
				{ -1, "Sapphire Block" }
			};
			TileIdentityHelpers.Data[64] = new Dictionary<int, string> {
				{ -1, "Ruby Block" }
			};
			TileIdentityHelpers.Data[65] = new Dictionary<int, string> {
				{ -1, "Emerald Block" }
			};
			TileIdentityHelpers.Data[66] = new Dictionary<int, string> {
				{ -1, "Topaz Block" }
			};
			TileIdentityHelpers.Data[67] = new Dictionary<int, string> {
				{ -1, "Amethyst Block" }
			};
			TileIdentityHelpers.Data[68] = new Dictionary<int, string> {
				{ -1, "Diamond Block" }
			};
			TileIdentityHelpers.Data[69] = new Dictionary<int, string> {
				{ -1, "Jungle Thorny Bush" }
			};
			TileIdentityHelpers.Data[70] = new Dictionary<int, string> {
				{ -1, "Mushroom Grass" }
			};
			TileIdentityHelpers.Data[71] = new Dictionary<int, string> {
				{ -1, "Glowing Mushroom (growing)" }
			};
			TileIdentityHelpers.Data[72] = new Dictionary<int, string> {
				{ -1, "Giant Glowing Mushroom (underground)" }
			};
			TileIdentityHelpers.Data[73] = new Dictionary<int, string> {
				{ -1, "Tall Grass Plants" }
			};
			TileIdentityHelpers.Data[74] = new Dictionary<int, string> {
				{ -1, "Tall Jungle Plants" }
			};
			TileIdentityHelpers.Data[75] = new Dictionary<int, string> {
				{ -1, "Obsidian Brick" }
			};
			TileIdentityHelpers.Data[76] = new Dictionary<int, string> {
				{ -1, "Hellstone Brick" }
			};
			TileIdentityHelpers.Data[77] = new Dictionary<int, string> {
				{ -1, "Hellforge" }
			};
			TileIdentityHelpers.Data[78] = new Dictionary<int, string> {
				{ -1, "Clay Pot" }
			};
			TileIdentityHelpers.Data[79] = new Dictionary<int, string> {
				{ 2, "Ebonwood Bed" },
				{ 3, "Rich Mahogany Bed" },
				{ 4, "Pearlwood Bed" },
				{ 5, "Shadewood Bed" },
				{ 6, "Blue Dungeon Bed" },
				{ 7, "Green Dungeon Bed" },
				{ 8, "Pink Dungeon Bed" },
				{ 9, "Obsidian Bed" },
				{ 10, "Glass Bed" },
				{ 11, "Golden Bed" },
				{ 12, "Honey Bed" },
				{ 13, "Steampunk Bed" },
				{ 14, "Cactus Bed" },
				{ 15, "Flesh Bed" },
				{ 16, "Frozen Bed" },
				{ 17, "Lihzahrd Bed" },
				{ 18, "Skyware Bed" },
				{ 19, "Spooky Bed" },
				{ 20, "Living Wood Bed" },
				{ 21, "Bone Bed" },
				{ 22, "Dynasty Bed" },
				{ 23, "Palm Wood Bed" },
				{ 24, "Mushroom Bed" },
				{ 25, "Boreal Wood Bed" },
				{ 26, "Slime Bed" },
				{ 27, "Pumpkin Bed" },
				{ 28, "Martian Bed" },
				{ 29, "Meteorite Bed" },
				{ 30, "Granite Bed" },
				{ 31, "Marble Bed" },
				{ 32, "Crystal Bed" },
				{ -1, "Bed" }
			};
			TileIdentityHelpers.Data[80] = new Dictionary<int, string> {
				{ -1, "Cactus (growing)" }
			};
			TileIdentityHelpers.Data[81] = new Dictionary<int, string> {
				{ -1, "Coral" }
			};
			TileIdentityHelpers.Data[82] = new Dictionary<int, string> {
				{ 2, "Moonglow (growing)" },
				{ 3, "Blinkroot (growing)" },
				{ 4, "Deathweed (growing)" },
				{ 5, "Waterleaf (growing)" },
				{ 6, "Fireblossom (growing)" },
				{ 7, "Shiverthorn (growing)" },
				{ -1, "Daybloom (growing)" }
			};
			TileIdentityHelpers.Data[83] = new Dictionary<int, string> {
				{ 2, "Moonglow (mature)" },
				{ 3, "Blinkroot (mature)" },
				{ 4, "Deathweed (mature)" },
				{ 5, "Waterleaf (mature)" },
				{ 6, "Fireblossom (mature)" },
				{ 7, "Shiverthorn (mature)" },
				{ -1, "Daybloom (mature)" }
			};
			TileIdentityHelpers.Data[84] = new Dictionary<int, string> {
				{ 2, "Moonglow (blooming)" },
				{ 3, "Blinkroot (blooming)" },
				{ 4, "Deathweed (blooming)" },
				{ 5, "Waterleaf (blooming)" },
				{ 6, "Fireblossom (blooming)" },
				{ 7, "Shiverthorn (blooming)" },
				{ -1, "Daybloom (blooming)" }
			};
			TileIdentityHelpers.Data[85] = new Dictionary<int, string> {
				{ 2, "Grave Marker" },
				{ 3, "Cross Grave Marker" },
				{ 4, "Headstone" },
				{ 5, "Gravestone" },
				{ 6, "Obelisk" },
				{ 7, "Golden Cross Grave Marker" },
				{ 8, "Golden Tombstone" },
				{ 9, "Golden Grave Marker" },
				{ 10, "Golden Gravestone" },
				{ 11, "Golden Headstone" },
				{ -1, "Tombstone" }
			};
			TileIdentityHelpers.Data[86] = new Dictionary<int, string> {
				{ -1, "Loom" }
			};
			TileIdentityHelpers.Data[87] = new Dictionary<int, string> {
				{ 2, "Ebonwood Piano" },
				{ 3, "Rich Mahogany Piano" },
				{ 4, "Pearlwood Piano" },
				{ 5, "Shadewood Piano" },
				{ 6, "Living Wood Piano" },
				{ 7, "Flesh Piano" },
				{ 8, "Frozen Piano" },
				{ 9, "Glass Piano" },
				{ 10, "Honey Piano" },
				{ 11, "Steampunk Piano" },
				{ 12, "Blue Dungeon Piano" },
				{ 13, "Green Dungeon Piano" },
				{ 14, "Pink Dungeon Piano" },
				{ 15, "Golden Piano" },
				{ 16, "Obsidian Piano" },
				{ 17, "Bone Piano" },
				{ 18, "Cactus Piano" },
				{ 19, "Spooky Piano" },
				{ 20, "Skyware Piano" },
				{ 21, "Lihzahrd Piano" },
				{ 22, "Palm Wood Piano" },
				{ 23, "Mushroom Piano" },
				{ 24, "Boreal Wood Piano" },
				{ 25, "Slime Piano" },
				{ 26, "Pumpkin Piano" },
				{ 27, "Martian Piano" },
				{ 28, "Meteorite Piano" },
				{ 29, "Granite Piano" },
				{ 30, "Marble Piano" },
				{ 31, "Crystal Piano" },
				{ 32, "Dynasty Piano" },
				{ -1, "Piano" }
			};
			TileIdentityHelpers.Data[88] = new Dictionary<int, string> {
				{ 2, "Ebonwood Dresser" },
				{ 3, "Rich Mahogany Dresser" },
				{ 4, "Pearlwood Dresser" },
				{ 5, "Shadewood Dresser" },
				{ 6, "Blue Dungeon Dresser" },
				{ 7, "Green Dungeon Dresser" },
				{ 8, "Pink Dungeon Dresser" },
				{ 9, "Golden Dresser" },
				{ 10, "Obsidian Dresser" },
				{ 11, "Bone Dresser" },
				{ 12, "Cactus Dresser" },
				{ 13, "Spooky Dresser" },
				{ 14, "Skyware Dresser" },
				{ 15, "Honey Dresser" },
				{ 16, "Lihzahrd Dresser" },
				{ 17, "Palm Wood Dresser" },
				{ 18, "Mushroom Dresser" },
				{ 19, "Boreal Wood Dresser" },
				{ 20, "Slime Dresser" },
				{ 21, "Pumpkin Dresser" },
				{ 22, "Steampunk Dresser" },
				{ 23, "Glass Dresser" },
				{ 24, "Flesh Dresser" },
				{ 25, "Martian Dresser" },
				{ 26, "Meteorite Dresser" },
				{ 27, "Granite Dresser" },
				{ 28, "Marble Dresser" },
				{ 29, "Crystal Dresser" },
				{ 30, "Dynasty Dresser" },
				{ 31, "Frozen Dresser" },
				{ 32, "Living Wood Dresser" },
				{ -1, "Dresser" }
			};
			TileIdentityHelpers.Data[89] = new Dictionary<int, string> {
				{ 2, "Sofa" },
				{ 3, "Ebonwood Sofa" },
				{ 4, "Rich Mahogany Sofa" },
				{ 5, "Pearlwood Sofa" },
				{ 6, "Shadewood Sofa" },
				{ 7, "Blue Dungeon Sofa" },
				{ 8, "Green Dungeon Sofa" },
				{ 9, "Pink Dungeon Sofa" },
				{ 10, "Golden Sofa" },
				{ 11, "Obsidian Sofa" },
				{ 12, "Bone Sofa" },
				{ 13, "Cactus Sofa" },
				{ 14, "Spooky Sofa" },
				{ 15, "Skyware Sofa" },
				{ 16, "Honey Sofa" },
				{ 17, "Steampunk Sofa" },
				{ 18, "Mushroom Sofa" },
				{ 19, "Glass Sofa" },
				{ 20, "Pumpkin Sofa" },
				{ 21, "Lihzahrd Sofa" },
				{ 22, "Palm Wood Bench" },
				{ 23, "Palm Wood Sofa" },
				{ 24, "Mushroom Bench" },
				{ 25, "Boreal Wood Sofa" },
				{ 26, "Slime Sofa" },
				{ 27, "Flesh Sofa" },
				{ 28, "Frozen Sofa" },
				{ 29, "Living Wood Sofa" },
				{ 30, "Martian Sofa" },
				{ 31, "Meteorite Sofa" },
				{ 32, "Granite Sofa" },
				{ 33, "Marble Sofa" },
				{ 34, "Crystal Sofa" },
				{ 35, "Dynasty Sofa" },
				{ -1, "Bench" }
			};
			TileIdentityHelpers.Data[90] = new Dictionary<int, string> {
				{ 1, "Cactus Bathtub" },
				{ 2, "Ebonwood Bathtub" },
				{ 3, "Flesh Bathtub" },
				{ 4, "Glass Bathtub" },
				{ 5, "Frozen Bathtub" },
				{ 6, "Rich Mahogany Bathtub" },
				{ 7, "Pearlwood Bathtub" },
				{ 8, "Lihzahrd Bathtub" },
				{ 9, "Skyware Bathtub" },
				{ 10, "Spooky Bathtub" },
				{ 11, "Honey Bathtub" },
				{ 12, "Steampunk Bathtub" },
				{ 13, "Living Wood Bathtub" },
				{ 14, "Shadewood Bathtub" },
				{ 15, "Bone Bathtub" },
				{ 16, "Dynasty Bathtub" },
				{ 17, "Palm Wood Bathtub" },
				{ 18, "Mushroom Bathtub" },
				{ 19, "Boreal Wood Bathtub" },
				{ 20, "Slime Bathtub" },
				{ 21, "Blue Dungeon Bathtub" },
				{ 22, "Green Dungeon Bathtub" },
				{ 23, "Pink Dungeon Bathtub" },
				{ 24, "Pumpkin Bathtub" },
				{ 25, "Obsidian Bathtub" },
				{ 26, "Golden Bathtub" },
				{ 27, "Martian Bathtub" },
				{ 28, "Meteorite Bathtub" },
				{ 29, "Granite Bathtub" },
				{ 30, "Marble Bathtub" },
				{ 31, "Crystal Bathtub" },
				{ -1, "Bathtub" }
			};
			TileIdentityHelpers.Data[91] = new Dictionary<int, string> {
				{ 2, "Green Banner" },
				{ 3, "Blue Banner" },
				{ 4, "Yellow Banner" },
				{ 5, "Ankh Banner" },
				{ 6, "Snake Banner" },
				{ 7, "Omega Banner" },
				{ 8, "World Banner" },
				{ 9, "Sun Banner" },
				{ 10, "Gravity Banner" },
				{ 11, "Marching Bones Banner" },
				{ 12, "Necromantic Sign" },
				{ 13, "Rusted Company Standard" },
				{ 14, "Ragged Brotherhood Sigil" },
				{ 15, "Molten Legion Flag" },
				{ 16, "Diabolic Sigil" },
				{ 17, "Hellbound Banner" },
				{ 18, "Hell Hammer Banner" },
				{ 19, "Helltower Banner" },
				{ 20, "Lost Hopes of Man Banner" },
				{ 21, "Obsidian Watcher Banner" },
				{ 22, "Lava Erupts Banner" },
				{ 23, "Angler Fish Banner" },
				{ 24, "Angry Nimbus Banner" },
				{ 25, "Anomura Fungus Banner" },
				{ 26, "Antlion Banner" },
				{ 27, "Arapaima Banner" },
				{ 28, "Armored Skeleton Banner" },
				{ 29, "Cave Bat Banner" },
				{ 30, "Bird Banner" },
				{ 31, "Black Recluse Banner" },
				{ 32, "Blood Feeder Banner" },
				{ 33, "Blood Jelly Banner" },
				{ 34, "Blood Crawler Banner" },
				{ 35, "Bone Serpent Banner" },
				{ 36, "Bunny Banner" },
				{ 37, "Chaos Elemental Banner" },
				{ 38, "Mimic Banner" },
				{ 39, "Clown Banner" },
				{ 40, "Corrupt Bunny Banner" },
				{ 41, "Corrupt Goldfish Banner" },
				{ 42, "Crab Banner" },
				{ 43, "Crimera Banner" },
				{ 44, "Crimson Axe Banner" },
				{ 45, "Cursed Hammer Banner" },
				{ 46, "Demon Banner" },
				{ 47, "Demon Eye Banner" },
				{ 48, "Derpling Banner" },
				{ 49, "Eater of Souls Banner" },
				{ 50, "Enchanted Sword Banner" },
				{ 51, "Zombie Eskimo Banner" },
				{ 52, "Face Monster Banner" },
				{ 53, "Floaty Gross Banner" },
				{ 54, "Flying Fish Banner" },
				{ 55, "Flying Snake Banner" },
				{ 56, "Frankenstein Banner" },
				{ 57, "Fungi Bulb Banner" },
				{ 58, "Fungo Fish Banner" },
				{ 59, "Gastropod Banner" },
				{ 60, "Goblin Thief Banner" },
				{ 61, "Goblin Sorcerer Banner" },
				{ 62, "Goblin Peon Banner" },
				{ 63, "Goblin Scout Banner" },
				{ 64, "Goblin Warrior Banner" },
				{ 65, "Goldfish Banner" },
				{ 66, "Harpy Banner" },
				{ 67, "Hellbat Banner" },
				{ 68, "Herpling Banner" },
				{ 69, "Hornet Banner" },
				{ 70, "Ice Elemental Banner" },
				{ 71, "Icy Merman Banner" },
				{ 72, "Fire Imp Banner" },
				{ 73, "Blue Jellyfish Banner" },
				{ 74, "Jungle Creeper Banner" },
				{ 75, "Lihzahrd Banner" },
				{ 76, "Man Eater Banner" },
				{ 77, "Meteor Head Banner" },
				{ 78, "Moth Banner" },
				{ 79, "Mummy Banner" },
				{ 80, "Mushi Ladybug Banner" },
				{ 81, "Parrot Banner" },
				{ 82, "Pigron Banner" },
				{ 83, "Piranha Banner" },
				{ 84, "Pirate Deckhand Banner" },
				{ 85, "Pixie Banner" },
				{ 86, "Raincoat Zombie Banner" },
				{ 87, "Reaper Banner" },
				{ 88, "Shark Banner" },
				{ 89, "Skeleton Banner" },
				{ 90, "Skeleton Mage Banner" },
				{ 91, "Blue Slime Banner" },
				{ 92, "Snow Flinx Banner" },
				{ 93, "Wall Creeper Banner" },
				{ 94, "Spore Zombie Banner" },
				{ 95, "Swamp Thing Banner" },
				{ 96, "Giant Tortoise Banner" },
				{ 97, "Toxic Sludge Banner" },
				{ 98, "Umbrella Slime Banner" },
				{ 99, "Unicorn Banner" },
				{ 100, "Vampire Banner" },
				{ 101, "Vulture Banner" },
				{ 102, "Nymph Banner" },
				{ 103, "Werewolf Banner" },
				{ 104, "Wolf Banner" },
				{ 105, "World Feeder Banner" },
				{ 106, "Worm Banner" },
				{ 107, "Wraith Banner" },
				{ 108, "Wyvern Banner" },
				{ 109, "Zombie Banner" },
				{ 110, "Angry Trapper Banner" },
				{ 111, "Armored Viking Banner" },
				{ 112, "Black Slime Banner" },
				{ 113, "Blue Armored Bones Banner" },
				{ 114, "Blue Cultist Archer Banner" },
				{ 115, "Blue Cultist Caster Banner" },
				{ 116, "Blue Cultist Fighter Banner" },
				{ 117, "Bone Lee Banner" },
				{ 118, "Clinger Banner" },
				{ 119, "Cochineal Beetle Banner" },
				{ 120, "Corrupt Penguin Banner" },
				{ 121, "Corrupt Slime Banner" },
				{ 122, "Corruptor Banner" },
				{ 123, "Crimslime Banner" },
				{ 124, "Cursed Skull Banner" },
				{ 125, "Cyan Beetle Banner" },
				{ 126, "Devourer Banner" },
				{ 127, "Diabolist Banner" },
				{ 128, "Doctor Bones Banner" },
				{ 129, "Dungeon Slime Banner" },
				{ 130, "Dungeon Spirit Banner" },
				{ 131, "Elf Archer Banner" },
				{ 132, "Elf Copter Banner" },
				{ 133, "Eyezor Banner" },
				{ 134, "Flocko Banner" },
				{ 135, "Ghost Banner" },
				{ 136, "Giant Bat Banner" },
				{ 137, "Giant Cursed Skull Banner" },
				{ 138, "Giant Flying Fox Banner" },
				{ 139, "Gingerbread Man Banner" },
				{ 140, "Goblin Archer Banner" },
				{ 141, "Green Slime Banner" },
				{ 142, "Headless Horseman Banner" },
				{ 143, "Hell Armored Bones Banner" },
				{ 144, "Hellhound Banner" },
				{ 145, "Hoppin' Jack Banner" },
				{ 146, "Ice Bat Banner" },
				{ 147, "Ice Golem Banner" },
				{ 148, "Ice Slime Banner" },
				{ 149, "Ichor Sticker Banner" },
				{ 150, "Illuminant Bat Banner" },
				{ 151, "Illuminant Slime Banner" },
				{ 152, "Jungle Bat Banner" },
				{ 153, "Jungle Slime Banner" },
				{ 154, "Krampus Banner" },
				{ 155, "Lac Beetle Banner" },
				{ 156, "Lava Bat Banner" },
				{ 157, "Martian Brainscrambler Banner" },
				{ 158, "Martian Drone Banner" },
				{ 159, "Martian Engineer Banner" },
				{ 160, "Martian Gigazapper Banner" },
				{ 161, "Martian Gray Grunt Banner" },
				{ 162, "Martian Officer Banner" },
				{ 163, "Martian Raygunner Banner" },
				{ 164, "Martian Scutlix Gunner Banner" },
				{ 165, "Martian Tesla Turret Banner" },
				{ 166, "Mister Stabby Banner" },
				{ 167, "Mother Slime Banner" },
				{ 168, "Necromancer Banner" },
				{ 169, "Nutcracker Banner" },
				{ 170, "Paladin Banner" },
				{ 171, "Penguin Banner" },
				{ 172, "Pinky Banner" },
				{ 173, "Poltergeist Banner" },
				{ 174, "Possessed Armor Banner" },
				{ 175, "Present Mimic Banner" },
				{ 176, "Purple Slime Banner" },
				{ 177, "Ragged Caster Banner" },
				{ 178, "Rainbow Slime Banner" },
				{ 179, "Raven Banner" },
				{ 180, "Red Slime Banner" },
				{ 181, "Rune Wizard Banner" },
				{ 182, "Rusty Armored Bones Banner" },
				{ 183, "Scarecrow Banner" },
				{ 184, "Scutlix Banner" },
				{ 185, "Skeleton Archer Banner" },
				{ 186, "Skeleton Commando Banner" },
				{ 187, "Skeleton Sniper Banner" },
				{ 188, "Slimer Banner" },
				{ 189, "Snatcher Banner" },
				{ 190, "Snow Balla Banner" },
				{ 191, "Snowman Gangsta Banner" },
				{ 192, "Spiked Ice Slime Banner" },
				{ 193, "Spiked Jungle Slime Banner" },
				{ 194, "Splinterling Banner" },
				{ 195, "Squid Banner" },
				{ 196, "Tactical Skeleton Banner" },
				{ 197, "The Groom Banner" },
				{ 198, "Tim Banner" },
				{ 199, "Undead Miner Banner" },
				{ 200, "Undead Viking Banner" },
				{ 201, "White Cultist Archer Banner" },
				{ 202, "White Cultist Caster Banner" },
				{ 203, "White Cultist Fighter Banner" },
				{ 204, "Yellow Slime Banner" },
				{ 205, "Yeti Banner" },
				{ 206, "Zombie Elf Banner" },
				{ 207, "Goblin Summoner Banner" },
				{ 208, "Salamander Banner" },
				{ 209, "Giant Shelly Banner" },
				{ 210, "Crawdad Banner" },
				{ 211, "Fritz Banner" },
				{ 212, "Creature from the Deep Banner" },
				{ 213, "Dr. Man Fly Banner" },
				{ 214, "Mothron Banner" },
				{ 215, "Severed Hand Banner" },
				{ 216, "The Possessed Banner" },
				{ 217, "Butcher Banner" },
				{ 218, "Psycho Banner" },
				{ 219, "Deadly Sphere Banner" },
				{ 220, "Nailhead Banner" },
				{ 221, "Poisonous Spore Banner" },
				{ 222, "Medusa Banner" },
				{ 223, "Hoplite Banner" },
				{ 224, "Granite Elemental Banner" },
				{ 225, "Grolem Banner" },
				{ 226, "Blood Zombie Banner" },
				{ 227, "Drippler Banner" },
				{ 228, "Tomb Crawler Banner" },
				{ 229, "Dune Splicer Banner" },
				{ 230, "Antlion Swarmer Banner" },
				{ 231, "Antlion Charger Banner" },
				{ 232, "Ghoul Banner" },
				{ 233, "Lamia Banner" },
				{ 234, "Desert Spirit Banner" },
				{ 235, "Basilisk Banner" },
				{ 236, "Ravager Scorpion Banner" },
				{ 237, "Stargazer Banner" },
				{ 238, "Milkyway Weaver Banner" },
				{ 239, "Flow Invader Banner" },
				{ 240, "Twinkle Popper Banner" },
				{ 241, "Small Star Cell Banner" },
				{ 242, "Star Cell Banner" },
				{ 243, "Corite Banner" },
				{ 244, "Sroller Banner" },
				{ 245, "Crawltipede Banner" },
				{ 246, "Drakomire Rider Banner" },
				{ 247, "Drakomire Banner" },
				{ 248, "Selenian Banner" },
				{ 249, "Predictor Banner" },
				{ 250, "Brain Suckler Banner" },
				{ 251, "Nebula Floater Banner" },
				{ 252, "Evolution Beast Banner" },
				{ 253, "Alien Larva Banner" },
				{ 254, "Alien Queen Banner" },
				{ 255, "Alien Hornet Banner" },
				{ 266, "Vortexian Banner" },
				{ 267, "Storm Diver Banner" },
				{ 268, "Pirate Captain Banner" },
				{ 269, "Pirate Deadeye Banner" },
				{ 270, "Pirate Corsair Banner" },
				{ 271, "Pirate Crossbower Banner" },
				{ 272, "Martian Walker Banner" },
				{ 273, "Red Devil Banner" },
				{ 274, "Pink Jellyfish Banner" },
				{ 275, "Green Jellyfish Banner" },
				{ 276, "Dark Mummy Banner" },
				{ 277, "Light Mummy Banner" },
				{ 278, "Angry Bones Banner" },
				{ 279, "Ice Tortoise Banner" },
				{ 280, "Sand Slime Banner" },
				{ 281, "Sea Snail Banner" },
				{ 282, "Sand Elemental Banner" },
				{ 283, "Sand Shark Banner" },
				{ 284, "Bone Biter Banner" },
				{ 285, "Flesh Reaver Banner" },
				{ 286, "Crystal Thresher Banner" },
				{ 287, "Angry Tumbler Banner" },
				{ 288, "Etherian Goblin Bomber Banner" },
				{ 289, "Etherian Goblin Banner" },
				{ 290, "Old One's Skeleton Banner" },
				{ 291, "Drakin Banner" },
				{ 292, "Kobold Glider Banner" },
				{ 293, "Kobold Banner" },
				{ 294, "Wither Beast Banner" },
				{ 295, "Etherian Wyvern Banner" },
				{ 296, "Etherian Javelin Thrower Banner" },
				{ 297, "Etherian Lightning Bug Banner" },
				{ -1, "Red Banner" }
			};
			TileIdentityHelpers.Data[92] = new Dictionary<int, string> {
				{ -1, "Lamp Post" }
			};
			TileIdentityHelpers.Data[93] = new Dictionary<int, string> {
				{ 2, "Cactus Lamp" },
				{ 3, "Ebonwood Lamp" },
				{ 4, "Flesh Lamp" },
				{ 5, "Glass Lamp" },
				{ 6, "Frozen Lamp" },
				{ 7, "Rich Mahogany Lamp" },
				{ 8, "Pearlwood Lamp" },
				{ 9, "Lihzahrd Lamp" },
				{ 10, "Skyware Lamp" },
				{ 11, "Spooky Lamp" },
				{ 12, "Honey Lamp" },
				{ 13, "Steampunk Lamp" },
				{ 14, "Living Wood Lamp" },
				{ 15, "Shadewood Lamp" },
				{ 16, "Golden Lamp" },
				{ 17, "Bone Lamp" },
				{ 18, "Dynasty Lamp" },
				{ 19, "Palm Wood Lamp" },
				{ 20, "Mushroom Lamp" },
				{ 21, "Boreal Wood Lamp" },
				{ 22, "Slime Lamp" },
				{ 23, "Pumpkin Lamp" },
				{ 24, "Obsidian Lamp" },
				{ 25, "Blue Dungeon Lamp" },
				{ 26, "Green Dungeon Lamp" },
				{ 27, "Pink Dungeon Lamp" },
				{ 28, "Martian Lamppost" },
				{ 29, "Meteorite Lamp" },
				{ 30, "Granite Lamp" },
				{ 31, "Marble Lamp" },
				{ 32, "Crystal Lamp" },
				{ -1, "Tiki Torch" }
			};
			TileIdentityHelpers.Data[94] = new Dictionary<int, string> {
				{ -1, "Keg" }
			};
			TileIdentityHelpers.Data[95] = new Dictionary<int, string> {
				{ -1, "Chinese Lantern" }
			};
			TileIdentityHelpers.Data[96] = new Dictionary<int, string> {
				{ 2, "Cauldron" },
				{ -1, "Cooking Pot" }
			};
			TileIdentityHelpers.Data[97] = new Dictionary<int, string> {
				{ -1, "Safe" }
			};
			TileIdentityHelpers.Data[98] = new Dictionary<int, string> {
				{ -1, "Skull Lantern" }
			};
			TileIdentityHelpers.Data[99] = new Dictionary<int, string> {
				{ -1, "Trash Can (not used)" }
			};
			TileIdentityHelpers.Data[100] = new Dictionary<int, string> {
				{ 2, "Cactus Candelabra" },
				{ 3, "Ebonwood Candelabra" },
				{ 4, "Flesh Candelabra" },
				{ 5, "Honey Candelabra" },
				{ 6, "Steampunk Candelabra" },
				{ 7, "Glass Candelabra" },
				{ 8, "Rich Mahogany Candelabra" },
				{ 9, "Pearlwood Candelabra" },
				{ 10, "Frozen Candelabra" },
				{ 11, "Lihzahrd Candelabra" },
				{ 12, "Skyware Candelabra" },
				{ 13, "Spooky Candelabra" },
				{ 14, "Living Wood Candelabra" },
				{ 15, "Shadewood Candelabra" },
				{ 16, "Golden Candelabra" },
				{ 17, "Bone Candelabra" },
				{ 18, "Large Dynasty Candle" },
				{ 19, "Palm Wood Candelabra" },
				{ 20, "Mushroom Candelabra" },
				{ 21, "Boreal Wood Candelabra" },
				{ 22, "Slime Candelabra" },
				{ 23, "Blue Dungeon Candelabra" },
				{ 24, "Green Dungeon Candelabra" },
				{ 25, "Pink Dungeon Candelabra" },
				{ 26, "Obsidian Candelabra" },
				{ 27, "Pumpkin Candelabra" },
				{ 28, "Martian Table Lamp" },
				{ 29, "Meteorite Candelabra" },
				{ 30, "Granite Candelabra" },
				{ 31, "Marble Candelabra" },
				{ 32, "Crystal Candelabra" },
				{ -1, "Candelabra" }
			};
			TileIdentityHelpers.Data[101] = new Dictionary<int, string> {
				{ 2, "Blue Dungeon Bookcase" },
				{ 3, "Green Dungeon Bookcase" },
				{ 4, "Pink Dungeon Bookcase" },
				{ 5, "Obsidian Bookcase" },
				{ 6, "Gothic Bookcase" },
				{ 7, "Cactus Bookcase" },
				{ 8, "Ebonwood Bookcase" },
				{ 9, "Flesh Bookcase" },
				{ 10, "Honey Bookcase" },
				{ 11, "Steampunk Bookcase" },
				{ 12, "Glass Bookcase" },
				{ 13, "Rich Mahogany Bookcase" },
				{ 14, "Pearlwood Bookcase" },
				{ 15, "Spooky Bookcase" },
				{ 16, "Skyware Bookcase" },
				{ 17, "Lihzahrd Bookcase" },
				{ 18, "Frozen Bookcase" },
				{ 19, "Living Wood Bookcase" },
				{ 20, "Shadewood Bookcase" },
				{ 21, "Golden Bookcase" },
				{ 22, "Bone Bookcase" },
				{ 23, "Dynasty Bookcase" },
				{ 24, "Martian Holobookcase" },
				{ 25, "Meteorite Bookcase" },
				{ 26, "Granite Bookcase" },
				{ 27, "Marble Bookcase" },
				{ 28, "Crystal Bookcase" },
				{ -1, "Bookcase" }
			};
			TileIdentityHelpers.Data[102] = new Dictionary<int, string> {
				{ -1, "Throne" }
			};
			TileIdentityHelpers.Data[103] = new Dictionary<int, string> {
				{ 2, "Dynasty Bowl" },
				{ 3, "Fancy Dishes" },
				{ 4, "Glass Bowl" },
				{ -1, "Bowl" }
			};
			TileIdentityHelpers.Data[104] = new Dictionary<int, string> {
				{ 2, "Dynasty Clock" },
				{ 3, "Golden Clock" },
				{ 4, "Glass Clock" },
				{ 5, "Honey Clock" },
				{ 6, "Steampunk Clock" },
				{ 7, "Boreal Wood Clock" },
				{ 8, "Slime Clock" },
				{ 9, "Bone Clock" },
				{ 10, "Cactus Clock" },
				{ 11, "Ebonwood Clock" },
				{ 12, "Frozen Clock" },
				{ 13, "Lihzahrd Clock" },
				{ 14, "Living Wood Clock" },
				{ 15, "Rich Mahogany Clock" },
				{ 16, "Flesh Clock" },
				{ 17, "Mushroom Clock" },
				{ 18, "Obsidian Clock" },
				{ 19, "Palm Wood Clock" },
				{ 20, "Pearlwood Clock" },
				{ 21, "Pumpkin Clock" },
				{ 22, "Shadewood Clock" },
				{ 23, "Spooky Clock" },
				{ 24, "Skyware Clock" },
				{ 25, "Martian Astro Clock" },
				{ 26, "Meteorite Clock" },
				{ 27, "Granite Clock" },
				{ 28, "Marble Clock" },
				{ 29, "Crystal Clock" },
				{ 30, "Sunplate Clock" },
				{ 31, "Blue Dungeon Clock" },
				{ 32, "Green Dungeon Clock" },
				{ 33, "Pink Dungeon Clock" },
				{ -1, "Grandfather Clock" }
			};
			TileIdentityHelpers.Data[105] = new Dictionary<int, string> {
				{ 2, "Angel Statue" },
				{ 3, "Star Statue" },
				{ 4, "Sword Statue" },
				{ 5, "Slime Statue" },
				{ 6, "Goblin Statue" },
				{ 7, "Shield Statue" },
				{ 8, "Bat Statue" },
				{ 9, "Fish Statue" },
				{ 10, "Bunny Statue" },
				{ 11, "Skeleton Statue" },
				{ 12, "Reaper Statue" },
				{ 13, "Woman Statue" },
				{ 14, "Imp Statue" },
				{ 15, "Gargoyle Statue" },
				{ 16, "Gloom Statue" },
				{ 17, "Hornet Statue" },
				{ 18, "Bomb Statue" },
				{ 19, "Crab Statue" },
				{ 20, "Hammer Statue" },
				{ 21, "Potion Statue" },
				{ 22, "Spear Statue" },
				{ 23, "Cross Statue" },
				{ 24, "Jellyfish Statue" },
				{ 25, "Bow Statue" },
				{ 26, "Boomerang Statue" },
				{ 27, "Boot Statue" },
				{ 28, "Chest Statue" },
				{ 29, "Bird Statue" },
				{ 30, "Axe Statue" },
				{ 31, "Corrupt Statue" },
				{ 32, "Tree Statue" },
				{ 33, "Anvil Statue" },
				{ 34, "Pickaxe Statue" },
				{ 35, "Mushroom Statue" },
				{ 36, "Eyeball Statue" },
				{ 37, "Pillar Statue" },
				{ 38, "Heart Statue" },
				{ 39, "Pot Statue" },
				{ 40, "Sunflower Statue" },
				{ 41, "King Statue" },
				{ 42, "Queen Statue" },
				{ 43, "Piranha Statue" },
				{ 44, "Lihzahrd Statue" },
				{ 45, "Lihzahrd Watcher Statue" },
				{ 46, "Lihzahrd Guardian Statue" },
				{ 47, "Blue Dungeon Vase" },
				{ 48, "Green Dungeon Vase" },
				{ 49, "Pink Dungeon Vase" },
				{ 50, "Obsidian Vase" },
				{ 51, "Shark Statue" },
				{ 52, "Squirrel Statue" },
				{ 53, "Butterfly Statue" },
				{ 54, "Worm Statue" },
				{ 55, "Firefly Statue" },
				{ 56, "Scorpion Statue" },
				{ 57, "Snail Statue" },
				{ 58, "Grasshopper Statue" },
				{ 59, "Mouse Statue" },
				{ 60, "Duck Statue" },
				{ 61, "Penguin Statue" },
				{ 62, "Frog Statue" },
				{ 63, "Buggy Statue" },
				{ 64, "Wall Creeper Statue" },
				{ 65, "Unicorn Statue" },
				{ 66, "Drippler Statue" },
				{ 67, "Wraith Statue" },
				{ 68, "Bone Skeleton Statue" },
				{ 69, "Undead Viking Statue" },
				{ 70, "Medusa Statue" },
				{ 71, "Harpy Statue" },
				{ 72, "Pigron Statue" },
				{ 73, "Hoplite Statue" },
				{ 74, "Granite Golem Statue" },
				{ 75, "Armed Zombie Statue" },
				{ 76, "Blood Zombie Statue" },
				{ 77, "(Unimplemented Statue)" },
				{ 78, "(Unimplemented Statue)" },
				{ -1, "Armor Statue" }
			};
			TileIdentityHelpers.Data[106] = new Dictionary<int, string> {
				{ -1, "Sawmill" }
			};
			TileIdentityHelpers.Data[107] = new Dictionary<int, string> {
				{ -1, "Cobalt Ore" }
			};
			TileIdentityHelpers.Data[108] = new Dictionary<int, string> {
				{ -1, "Mythril Ore" }
			};
			TileIdentityHelpers.Data[109] = new Dictionary<int, string> {
				{ -1, "Hallowed Grass" }
			};
			TileIdentityHelpers.Data[110] = new Dictionary<int, string> {
				{ -1, "Short Hallowed Plants" }
			};
			TileIdentityHelpers.Data[111] = new Dictionary<int, string> {
				{ -1, "Adamantite Ore" }
			};
			TileIdentityHelpers.Data[112] = new Dictionary<int, string> {
				{ -1, "Ebonsand Block" }
			};
			TileIdentityHelpers.Data[113] = new Dictionary<int, string> {
				{ -1, "Tall Hallowed Plants" }
			};
			TileIdentityHelpers.Data[114] = new Dictionary<int, string> {
				{ -1, "Tinkerer's Workshop" }
			};
			TileIdentityHelpers.Data[115] = new Dictionary<int, string> {
				{ -1, "Hallowed Vines" }
			};
			TileIdentityHelpers.Data[116] = new Dictionary<int, string> {
				{ -1, "Pearlsand Block" }
			};
			TileIdentityHelpers.Data[117] = new Dictionary<int, string> {
				{ -1, "Pearlstone Block" }
			};
			TileIdentityHelpers.Data[118] = new Dictionary<int, string> {
				{ -1, "Pearlstone Brick" }
			};
			TileIdentityHelpers.Data[119] = new Dictionary<int, string> {
				{ -1, "Iridescent Brick" }
			};
			TileIdentityHelpers.Data[120] = new Dictionary<int, string> {
				{ -1, "Mudstone Brick" }
			};
			TileIdentityHelpers.Data[121] = new Dictionary<int, string> {
				{ -1, "Cobalt Brick" }
			};
			TileIdentityHelpers.Data[122] = new Dictionary<int, string> {
				{ -1, "Mythril Brick" }
			};
			TileIdentityHelpers.Data[123] = new Dictionary<int, string> {
				{ -1, "Silt Block" }
			};
			TileIdentityHelpers.Data[124] = new Dictionary<int, string> {
				{ -1, "Wooden Beam" }
			};
			TileIdentityHelpers.Data[125] = new Dictionary<int, string> {
				{ -1, "Crystal Ball" }
			};
			TileIdentityHelpers.Data[126] = new Dictionary<int, string> {
				{ -1, "Disco Ball" }
			};
			TileIdentityHelpers.Data[127] = new Dictionary<int, string> {
				{ -1, "Ice Block ( Ice Rod)" }
			};
			TileIdentityHelpers.Data[128] = new Dictionary<int, string> {
				{ -1, "Mannequin" }
			};
			TileIdentityHelpers.Data[129] = new Dictionary<int, string> {
				{ -1, "Crystal Shard" }
			};
			TileIdentityHelpers.Data[130] = new Dictionary<int, string> {
				{ -1, "Active Stone Block" }
			};
			TileIdentityHelpers.Data[131] = new Dictionary<int, string> {
				{ -1, "Inactive Stone Block" }
			};
			TileIdentityHelpers.Data[132] = new Dictionary<int, string> {
				{ -1, "Lever" }
			};
			TileIdentityHelpers.Data[133] = new Dictionary<int, string> {
				{ 2, "Titanium Forge" },
				{ -1, "Adamantite Forge" }
			};
			TileIdentityHelpers.Data[134] = new Dictionary<int, string> {
				{ 2, "Orichalcum Anvil" },
				{ -1, "Mythril Anvil" }
			};
			TileIdentityHelpers.Data[135] = new Dictionary<int, string> {
				{ 2, "Green Pressure Plate" },
				{ 3, "Gray Pressure Plate" },
				{ 4, "Brown Pressure Plate" },
				{ 5, "Blue Pressure Plate" },
				{ 6, "Yellow Pressure Plate" },
				{ 7, "Lihzahrd Pressure Plate" },
				{ -1, "Red Pressure Plate" }
			};
			TileIdentityHelpers.Data[136] = new Dictionary<int, string> {
				{ -1, "Switch" }
			};
			TileIdentityHelpers.Data[137] = new Dictionary<int, string> {
				{ 2, "Super Dart Trap" },
				{ 3, "Flame Trap" },
				{ 4, "Spiky Ball Trap" },
				{ 5, "Spear Trap" },
				{ -1, "Dart Trap" }
			};
			TileIdentityHelpers.Data[138] = new Dictionary<int, string> {
				{ -1, "Boulder" }
			};
			TileIdentityHelpers.Data[139] = new Dictionary<int, string> {
				{ 2, "Music Box (Eerie)" },
				{ 3, "Music Box (Night)" },
				{ 4, "Music Box (Title)" },
				{ 5, "Music Box (Underground)" },
				{ 6, "Music Box (Boss 1)" },
				{ 7, "Music Box (Jungle)" },
				{ 8, "Music Box (Corruption)" },
				{ 9, "Music Box (Underground Corruption)" },
				{ 10, "Music Box (The Hallow)" },
				{ 11, "Music Box (Boss 2)" },
				{ 12, "Music Box (Underground Hallow)" },
				{ 13, "Music Box (Boss 3)" },
				{ 14, "Music Box (Snow)" },
				{ 15, "Music Box (Space)" },
				{ 16, "Music Box (Crimson)" },
				{ 17, "Music Box (Boss 4)" },
				{ 18, "Music Box (Alt Overworld Day)" },
				{ 19, "Music Box (Rain)" },
				{ 20, "Music Box (Ice)" },
				{ 21, "Music Box (Desert)" },
				{ 22, "Music Box (Ocean)" },
				{ 23, "Music Box (Dungeon)" },
				{ 24, "Music Box (Plantera)" },
				{ 25, "Music Box (Boss 5)" },
				{ 26, "Music Box (Temple)" },
				{ 27, "Music Box (Eclipse)" },
				{ 28, "Music Box (Mushrooms)" },
				{ 29, "Music Box (Pumpkin Moon)" },
				{ 30, "Music Box (Alt Underground)" },
				{ 31, "Music Box (Frost Moon)" },
				{ 32, "Music Box (Underground Crimson)" },
				{ 33, "Music Box (Lunar Boss)" },
				{ 34, "Music Box (Martian Madness)" },
				{ 35, "Music Box (Pirate Invasion)" },
				{ 36, "Music Box (Hell)" },
				{ 37, "Music Box (The Towers)" },
				{ 38, "Music Box (Goblin Invasion)" },
				{ 39, "Music Box (Sandstorm)" },
				{ 40, "Music Box (Old One's Army)" },
				{ -1, "Music Box (Overworld Day)" }
			};
			TileIdentityHelpers.Data[140] = new Dictionary<int, string> {
				{ -1, "Demonite Brick" }
			};
			TileIdentityHelpers.Data[141] = new Dictionary<int, string> {
				{ -1, "Explosives" }
			};
			TileIdentityHelpers.Data[142] = new Dictionary<int, string> {
				{ -1, "Inlet Pump" }
			};
			TileIdentityHelpers.Data[143] = new Dictionary<int, string> {
				{ -1, "Outlet Pump" }
			};
			TileIdentityHelpers.Data[144] = new Dictionary<int, string> {
				{ 2, "3 Second Timer" },
				{ 3, "5 Second Timer" },
				{ -1, "1 Second Timer" }
			};
			TileIdentityHelpers.Data[145] = new Dictionary<int, string> {
				{ -1, "Candy Cane Block" }
			};
			TileIdentityHelpers.Data[146] = new Dictionary<int, string> {
				{ -1, "Green Candy Cane Block" }
			};
			TileIdentityHelpers.Data[147] = new Dictionary<int, string> {
				{ -1, "Snow Block" }
			};
			TileIdentityHelpers.Data[148] = new Dictionary<int, string> {
				{ -1, "Snow Brick" }
			};
			TileIdentityHelpers.Data[149] = new Dictionary<int, string> {
				{ 2, "Red Light" },
				{ 3, "Green Light" },
				{ -1, "Blue Light" }
			};
			TileIdentityHelpers.Data[150] = new Dictionary<int, string> {
				{ -1, "Adamantite Beam" }
			};
			TileIdentityHelpers.Data[151] = new Dictionary<int, string> {
				{ -1, "Sandstone Brick" }
			};
			TileIdentityHelpers.Data[152] = new Dictionary<int, string> {
				{ -1, "Ebonstone Brick" }
			};
			TileIdentityHelpers.Data[153] = new Dictionary<int, string> {
				{ -1, "Red Stucco" }
			};
			TileIdentityHelpers.Data[154] = new Dictionary<int, string> {
				{ -1, "Yellow Stucco" }
			};
			TileIdentityHelpers.Data[155] = new Dictionary<int, string> {
				{ -1, "Green Stucco" }
			};
			TileIdentityHelpers.Data[156] = new Dictionary<int, string> {
				{ -1, "Gray Stucco" }
			};
			TileIdentityHelpers.Data[157] = new Dictionary<int, string> {
				{ -1, "Ebonwood" }
			};
			TileIdentityHelpers.Data[158] = new Dictionary<int, string> {
				{ -1, "Rich Mahogany" }
			};
			TileIdentityHelpers.Data[159] = new Dictionary<int, string> {
				{ -1, "Pearlwood" }
			};
			TileIdentityHelpers.Data[160] = new Dictionary<int, string> {
				{ -1, "Rainbow Brick" }
			};
			TileIdentityHelpers.Data[161] = new Dictionary<int, string> {
				{ -1, "Ice Block" }
			};
			TileIdentityHelpers.Data[162] = new Dictionary<int, string> {
				{ -1, "Thin Ice" }
			};
			TileIdentityHelpers.Data[163] = new Dictionary<int, string> {
				{ -1, "Purple Ice Block" }
			};
			TileIdentityHelpers.Data[164] = new Dictionary<int, string> {
				{ -1, "Pink Ice Block" }
			};
			TileIdentityHelpers.Data[165] = new Dictionary<int, string> {
				{ -1, "Ambient Objects" }
			};
			TileIdentityHelpers.Data[166] = new Dictionary<int, string> {
				{ -1, "Tin Ore" }
			};
			TileIdentityHelpers.Data[167] = new Dictionary<int, string> {
				{ -1, "Lead Ore" }
			};
			TileIdentityHelpers.Data[168] = new Dictionary<int, string> {
				{ -1, "Tungsten Ore" }
			};
			TileIdentityHelpers.Data[169] = new Dictionary<int, string> {
				{ -1, "Platinum Ore" }
			};
			TileIdentityHelpers.Data[170] = new Dictionary<int, string> {
				{ -1, "Pine Tree Block" }
			};
			TileIdentityHelpers.Data[171] = new Dictionary<int, string> {
				{ -1, "Christmas Tree" }
			};
			TileIdentityHelpers.Data[172] = new Dictionary<int, string> {
				{ 2, "Ebonwood Sink" },
				{ 3, "Rich Mahogany Sink" },
				{ 4, "Pearlwood Sink" },
				{ 5, "Bone Sink" },
				{ 6, "Flesh Sink" },
				{ 7, "Living Wood Sink" },
				{ 8, "Skyware Sink" },
				{ 9, "Shadewood Sink" },
				{ 10, "Lihzahrd Sink" },
				{ 11, "Blue Dungeon Sink" },
				{ 12, "Green Dungeon Sink" },
				{ 13, "Pink Dungeon Sink" },
				{ 14, "Obsidian Sink" },
				{ 15, "Metal Sink" },
				{ 16, "Glass Sink" },
				{ 17, "Golden Sink" },
				{ 18, "Honey Sink" },
				{ 19, "Steampunk Sink" },
				{ 20, "Pumpkin Sink" },
				{ 21, "Spooky Sink" },
				{ 22, "Frozen Sink" },
				{ 23, "Dynasty Sink" },
				{ 24, "Palm Wood Sink" },
				{ 25, "Mushroom Sink" },
				{ 26, "Boreal Wood Sink" },
				{ 27, "Slime Sink" },
				{ 28, "Cactus Sink" },
				{ 29, "Martian Sink" },
				{ 30, "Meteorite Sink" },
				{ 31, "Granite Sink" },
				{ 32, "Marble Sink" },
				{ 33, "Crystal Sink" },
				{ -1, "Wooden Sink" }
			};
			TileIdentityHelpers.Data[173] = new Dictionary<int, string> {
				{ -1, "Platinum Candelabra" }
			};
			TileIdentityHelpers.Data[174] = new Dictionary<int, string> {
				{ -1, "Platinum Candle" }
			};
			TileIdentityHelpers.Data[175] = new Dictionary<int, string> {
				{ -1, "Tin Brick" }
			};
			TileIdentityHelpers.Data[176] = new Dictionary<int, string> {
				{ -1, "Tungsten Brick" }
			};
			TileIdentityHelpers.Data[177] = new Dictionary<int, string> {
				{ -1, "Platinum Brick" }
			};
			TileIdentityHelpers.Data[178] = new Dictionary<int, string> {
				{ 2, "Topaz" },
				{ 3, "Sapphire" },
				{ 4, "Emerald" },
				{ 5, "Ruby" },
				{ 6, "Diamond" },
				{ 7, "Amber" },
				{ -1, "Amethyst" }
			};
			TileIdentityHelpers.Data[179] = new Dictionary<int, string> {
				{ -1, "Teal Moss" }
			};
			TileIdentityHelpers.Data[180] = new Dictionary<int, string> {
				{ -1, "Chartreuse	Moss" }
			};
			TileIdentityHelpers.Data[181] = new Dictionary<int, string> {
				{ -1, "Red Moss" }
			};
			TileIdentityHelpers.Data[182] = new Dictionary<int, string> {
				{ -1, "Blue Moss" }
			};
			TileIdentityHelpers.Data[183] = new Dictionary<int, string> {
				{ -1, "Purple Moss" }
			};
			TileIdentityHelpers.Data[184] = new Dictionary<int, string> {
				{ -1, "Moss Growth" }
			};
			TileIdentityHelpers.Data[185] = new Dictionary<int, string> {
				{ -1, "Small Ambient Objects" }
			};
			TileIdentityHelpers.Data[186] = new Dictionary<int, string> {
				{ -1, "Large Ambient Objects" }
			};
			TileIdentityHelpers.Data[187] = new Dictionary<int, string> {
				{ -1, "Large Ambient Objects" }
			};
			TileIdentityHelpers.Data[188] = new Dictionary<int, string> {
				{ -1, "Cactus (placed)" }
			};
			TileIdentityHelpers.Data[189] = new Dictionary<int, string> {
				{ -1, "Cloud" }
			};
			TileIdentityHelpers.Data[190] = new Dictionary<int, string> {
				{ -1, "Glowing Mushroom (placed)" }
			};
			TileIdentityHelpers.Data[191] = new Dictionary<int, string> {
				{ -1, "Living Wood" }
			};
			TileIdentityHelpers.Data[192] = new Dictionary<int, string> {
				{ -1, "Leaf Block" }
			};
			TileIdentityHelpers.Data[193] = new Dictionary<int, string> {
				{ -1, "Slime Block" }
			};
			TileIdentityHelpers.Data[194] = new Dictionary<int, string> {
				{ -1, "Bone Block" }
			};
			TileIdentityHelpers.Data[195] = new Dictionary<int, string> {
				{ -1, "Flesh Block" }
			};
			TileIdentityHelpers.Data[196] = new Dictionary<int, string> {
				{ -1, "Rain Cloud" }
			};
			TileIdentityHelpers.Data[197] = new Dictionary<int, string> {
				{ -1, "Frozen Slime Block" }
			};
			TileIdentityHelpers.Data[198] = new Dictionary<int, string> {
				{ -1, "Asphalt Block" }
			};
			TileIdentityHelpers.Data[199] = new Dictionary<int, string> {
				{ -1, "Crimson Grass" }
			};
			TileIdentityHelpers.Data[200] = new Dictionary<int, string> {
				{ -1, "Red Ice Block" }
			};
			TileIdentityHelpers.Data[201] = new Dictionary<int, string> {
				{ 16, "Vicious Mushroom" },
				{ 1, "Short Crimson Plants" },
				{ 2, "Short Crimson Plants" },
				{ 3, "Short Crimson Plants" },
				{ 4, "Short Crimson Plants" },
				{ 5, "Short Crimson Plants" },
				{ 6, "Short Crimson Plants" },
				{ 7, "Short Crimson Plants" },
				{ 8, "Short Crimson Plants" },
				{ 9, "Short Crimson Plants" },
				{ 10, "Short Crimson Plants" },
				{ 11, "Short Crimson Plants" },
				{ 12, "Short Crimson Plants" },
				{ 13, "Short Crimson Plants" },
				{ 14, "Short Crimson Plants" },
				{ 15, "Short Crimson Plants" },
				{ 17, "Short Crimson Plants" },
				{ 18, "Short Crimson Plants" },
				{ 19, "Short Crimson Plants" },
				{ 20, "Short Crimson Plants" },
				{ 21, "Short Crimson Plants" },
				{ 22, "Short Crimson Plants" },
				{ 23, "Short Crimson Plants" },
				{ -1, "Short Crimson Plants" }
			};
			TileIdentityHelpers.Data[202] = new Dictionary<int, string> {
				{ -1, "Sunplate Block" }
			};
			TileIdentityHelpers.Data[203] = new Dictionary<int, string> {
				{ -1, "Crimstone Block" }
			};
			TileIdentityHelpers.Data[204] = new Dictionary<int, string> {
				{ -1, "Crimtane Ore" }
			};
			TileIdentityHelpers.Data[205] = new Dictionary<int, string> {
				{ -1, "Crimson Vines" }
			};
			TileIdentityHelpers.Data[206] = new Dictionary<int, string> {
				{ -1, "Ice Brick" }
			};
			TileIdentityHelpers.Data[207] = new Dictionary<int, string> {
				{ 2, "Desert Water Fountain" },
				{ 3, "Jungle Water Fountain" },
				{ 4, "Icy Water Fountain" },
				{ 5, "Corrupt Water Fountain" },
				{ 6, "Crimson Water Fountain" },
				{ 7, "Hallowed Water Fountain" },
				{ 8, "Blood Water Fountain" },
				{ 9, "Underground Water Fountain (unused)" },
				{ -1, "Pure Water Fountain" }
			};
			TileIdentityHelpers.Data[208] = new Dictionary<int, string> {
				{ -1, "Shadewood" }
			};
			TileIdentityHelpers.Data[209] = new Dictionary<int, string> {
				{ 2, "Bunny Cannon" },
				{ 3, "Confetti Cannon" },
				{ 4, "Portal Gun Station (blue portal)" },
				{ 5, "Portal Gun Station (orange portal)" },
				{ -1, "Cannon" }
			};
			TileIdentityHelpers.Data[210] = new Dictionary<int, string> {
				{ -1, "Land Mine" }
			};
			TileIdentityHelpers.Data[211] = new Dictionary<int, string> {
				{ -1, "Chlorophyte Ore" }
			};
			TileIdentityHelpers.Data[212] = new Dictionary<int, string> {
				{ -1, "Snowball Launcher" }
			};
			TileIdentityHelpers.Data[213] = new Dictionary<int, string> {
				{ -1, "Rope" }
			};
			TileIdentityHelpers.Data[214] = new Dictionary<int, string> {
				{ -1, "Chain" }
			};
			TileIdentityHelpers.Data[215] = new Dictionary<int, string> {
				{ 2, "Cursed Campfire" },
				{ 3, "Demon Campfire" },
				{ 4, "Frozen Campfire" },
				{ 5, "Ichor Campfire" },
				{ 6, "Rainbow Campfire" },
				{ 7, "Ultra Bright Campfire" },
				{ 8, "Bone Campfire" },
				{ -1, "Campfire" }
			};
			TileIdentityHelpers.Data[216] = new Dictionary<int, string> {
				{ 2, "Green Rocket" },
				{ 3, "Blue Rocket" },
				{ 4, "Yellow Rocket" },
				{ -1, "Red Rocket" }
			};
			TileIdentityHelpers.Data[217] = new Dictionary<int, string> {
				{ -1, "Blend-O-Matic" }
			};
			TileIdentityHelpers.Data[218] = new Dictionary<int, string> {
				{ -1, "Meat Grinder" }
			};
			TileIdentityHelpers.Data[219] = new Dictionary<int, string> {
				{ -1, "Extractinator" }
			};
			TileIdentityHelpers.Data[220] = new Dictionary<int, string> {
				{ -1, "Solidifier" }
			};
			TileIdentityHelpers.Data[221] = new Dictionary<int, string> {
				{ -1, "Palladium Ore" }
			};
			TileIdentityHelpers.Data[222] = new Dictionary<int, string> {
				{ -1, "Orichalcum Ore" }
			};
			TileIdentityHelpers.Data[223] = new Dictionary<int, string> {
				{ -1, "Titanium Ore" }
			};
			TileIdentityHelpers.Data[224] = new Dictionary<int, string> {
				{ -1, "Slush Block" }
			};
			TileIdentityHelpers.Data[225] = new Dictionary<int, string> {
				{ -1, "Hive" }
			};
			TileIdentityHelpers.Data[226] = new Dictionary<int, string> {
				{ -1, "Lihzahrd Brick" }
			};
			TileIdentityHelpers.Data[227] = new Dictionary<int, string> {
				{ 2, "Green Mushroom" },
				{ 3, "Sky Blue Flower" },
				{ 4, "Yellow Marigold" },
				{ 5, "Blue Berries" },
				{ 6, "Lime Kelp" },
				{ 7, "Pink Prickly Pear" },
				{ 8, "Orange Bloodroot" },
				{ -1, "Teal Mushroom" },
				{ 9-12, "Strange Plant" }
			};
			TileIdentityHelpers.Data[228] = new Dictionary<int, string> {
				{ -1, "Dye Vat" }
			};
			TileIdentityHelpers.Data[229] = new Dictionary<int, string> {
				{ -1, "Honey Block" }
			};
			TileIdentityHelpers.Data[230] = new Dictionary<int, string> {
				{ -1, "Crispy Honey Block" }
			};
			TileIdentityHelpers.Data[231] = new Dictionary<int, string> {
				{ -1, "larva" }
			};
			TileIdentityHelpers.Data[232] = new Dictionary<int, string> {
				{ -1, "Wooden Spike" }
			};
			TileIdentityHelpers.Data[233] = new Dictionary<int, string> {
				{ -1, "Plant Detritus" }
			};
			TileIdentityHelpers.Data[234] = new Dictionary<int, string> {
				{ -1, "Crimsand Block" }
			};
			TileIdentityHelpers.Data[235] = new Dictionary<int, string> {
				{ -1, "Teleporter" }
			};
			TileIdentityHelpers.Data[236] = new Dictionary<int, string> {
				{ -1, "Life Fruit" }
			};
			TileIdentityHelpers.Data[237] = new Dictionary<int, string> {
				{ -1, "Lihzahrd Altar" }
			};
			TileIdentityHelpers.Data[238] = new Dictionary<int, string> {
				{ -1, "Plantera's Bulb" }
			};
			TileIdentityHelpers.Data[239] = new Dictionary<int, string> {
				{ 2, "Tin Bar" },
				{ 3, "Iron Bar" },
				{ 4, "Lead Bar" },
				{ 5, "Silver Bar" },
				{ 6, "Tungsten Bar" },
				{ 7, "Gold Bar" },
				{ 8, "Platinum Bar" },
				{ 9, "Demonite Bar" },
				{ 10, "Meteorite Bar" },
				{ 11, "Hellstone Bar" },
				{ 12, "Cobalt Bar" },
				{ 13, "Palladium Bar" },
				{ 14, "Mythril Bar" },
				{ 15, "Orichalcum Bar" },
				{ 16, "Adamantite Bar" },
				{ 17, "Titanium Bar" },
				{ 18, "Chlorophyte Bar" },
				{ 19, "Hallowed Bar" },
				{ 20, "Crimtane Bar" },
				{ 21, "Shroomite Bar" },
				{ 22, "Spectre Bar" },
				{ 23, "Luminite Bar" },
				{ -1, "Copper Bar" }
			};
			TileIdentityHelpers.Data[240] = new Dictionary<int, string> {
				{ 2, "Eater of Worlds Trophy" },
				{ 3, "Brain of Cthulhu Trophy" },
				{ 4, "Skeletron Trophy" },
				{ 5, "Queen Bee Trophy" },
				{ 6, "Wall of Flesh Trophy" },
				{ 7, "Destroyer Trophy" },
				{ 8, "Skeletron Prime Trophy" },
				{ 9, "Retinazer Trophy" },
				{ 10, "Spazmatism Trophy" },
				{ 11, "Plantera Trophy" },
				{ 12, "Golem Trophy" },
				{ 13, "Blood Moon Rising" },
				{ 14, "The Hanged Man" },
				{ 15, "Glory of the Fire" },
				{ 16, "Bone Warp" },
				{ 17, "Wall Skeleton" },
				{ 18, "Hanging Skeleton" },
				{ 19, "Skellington J Skellingsworth" },
				{ 20, "The Cursed Man" },
				{ 21, "Sunflowers" },
				{ 22, "Terrarian Gothic" },
				{ 23, "Guide Picasso" },
				{ 24, "The Guardian's Gaze" },
				{ 25, "Father of Someone" },
				{ 26, "Nurse Lisa" },
				{ 27, "Discover" },
				{ 28, "Hand Earth" },
				{ 29, "Old Miner" },
				{ 30, "Skelehead" },
				{ 31, "Imp Face" },
				{ 32, "Ominous Presence" },
				{ 33, "Shining Moon" },
				{ 34, "The Merchant" },
				{ 35, "Crowno Devours His Lunch" },
				{ 36, "Rare Enchantment" },
				{ 37, "Mourning Wood Trophy" },
				{ 38, "Pumpking Trophy" },
				{ 39, "Ice Queen Trophy" },
				{ 41, "Everscream Trophy" },
				{ 42, "Blacksmith Rack" },
				{ 43, "Carpentry Rack" },
				{ 44, "Helmet Rack" },
				{ 45, "Spear Rack" },
				{ 46, "Sword Rack" },
				{ 47, "Life Preserver" },
				{ 48, "Ship's Wheel" },
				{ 49, "Compass Rose" },
				{ 51, "Goldfish Trophy" },
				{ 52, "Bunnyfish Trophy" },
				{ 53, "Swordfish Trophy" },
				{ 54, "Sharkteeth Trophy" },
				{ 55, "King Slime Trophy" },
				{ 56, "Duke Fishron Trophy" },
				{ 57, "Ancient Cultist Trophy" },
				{ 58, "Martian Saucer Trophy" },
				{ 59, "Flying Dutchman Trophy" },
				{ 60, "Moon Lord Trophy" },
				{ 61, "Dark Mage Trophy" },
				{ 62, "Betsy Trophy" },
				{ 63, "Ogre Trophy" },
				{ -1, "Eye of Cthulhu Trophy" }
			};
			TileIdentityHelpers.Data[241] = new Dictionary<int, string> {
				{ -1, "Catacomb" }
			};
			TileIdentityHelpers.Data[242] = new Dictionary<int, string> {
				{ 2, "Something Evil is Watching You" },
				{ 3, "The Twins Have Awoken" },
				{ 4, "The Screamer" },
				{ 5, "Goblins Playing Poker" },
				{ 6, "Dryadisque" },
				{ 7, "Impact" },
				{ 8, "Powered by Birds" },
				{ 9, "The Destroyer (item)" },
				{ 10, "The Persistency of Eyes" },
				{ 11, "Unicorn Crossing the Hallows" },
				{ 12, "Great Wave" },
				{ 13, "Starry Night" },
				{ 14, "Facing the Cerebral Mastermind" },
				{ 15, "Lake of Fire" },
				{ 16, "Trio Super Heroes" },
				{ 17, "The Creation of the Guide" },
				{ 18, "Jacking Skeletron" },
				{ 19, "Bitter Harvest" },
				{ 20, "Blood Moon Countess" },
				{ 21, "Hallow's Eve" },
				{ 22, "Morbid Curiosity" },
				{ 23, "Tiger Skin" },
				{ 24, "Leopard Skin" },
				{ 25, "Zebra Skin" },
				{ 26, "Treasure Map" },
				{ 27, "Pillagin Me Pixels" },
				{ 28, "Castle Marsberg" },
				{ 29, "Martia Lisa" },
				{ 30, "The Truth Is Up There" },
				{ 31, "Sparky" },
				{ 32, "Acorns" },
				{ 33, "Cold Snap" },
				{ 34, "Cursed Saint" },
				{ 35, "Snowfellas" },
				{ 36, "The Season" },
				{ 37, "Not a Kid, nor a Squid" },
				{ -1, "The Eye Sees the End" }
			};
			TileIdentityHelpers.Data[243] = new Dictionary<int, string> {
				{ -1, "Imbuing Station" }
			};
			TileIdentityHelpers.Data[244] = new Dictionary<int, string> {
				{ -1, "Bubble Machine" }
			};
			TileIdentityHelpers.Data[245] = new Dictionary<int, string> {
				{ 2, "Darkness (Painting)" },
				{ 3, "Dark Soul Reaper" },
				{ 4, "Land" },
				{ 5, "Trapped Ghost" },
				{ 6, "American Explosive" },
				{ 7, "Glorious Night" },
				{ -1, "Waldo" }
			};
			TileIdentityHelpers.Data[246] = new Dictionary<int, string> {
				{ 2, "Finding Gold" },
				{ 3, "First Encounter" },
				{ 4, "Good Morning" },
				{ 5, "Underground Reward" },
				{ 6, "Through the Window" },
				{ 7, "Place Above the Clouds" },
				{ 8, "Do Not Step on the Grass" },
				{ 9, "Cold Waters in the White Land" },
				{ 10, "Lightless Chasms" },
				{ 11, "The Land of Deceiving Looks" },
				{ 12, "Daylight" },
				{ 13, "Secret of the Sands" },
				{ 14, "Deadland Comes Alive" },
				{ 15, "Evil Presence" },
				{ 16, "Sky Guardian" },
				{ 17, "Living Gore" },
				{ 18, "Flowing Magma" },
				{ 19, "Holly" },
				{ -1, "Demon's Eye" }
			};
			TileIdentityHelpers.Data[247] = new Dictionary<int, string> {
				{ -1, "Autohammer" }
			};
			TileIdentityHelpers.Data[248] = new Dictionary<int, string> {
				{ -1, "Palladium Column" }
			};
			TileIdentityHelpers.Data[249] = new Dictionary<int, string> {
				{ -1, "Bubblegum Block" }
			};
			TileIdentityHelpers.Data[250] = new Dictionary<int, string> {
				{ -1, "Titanstone Block" }
			};
			TileIdentityHelpers.Data[251] = new Dictionary<int, string> {
				{ -1, "Pumpkin" }
			};
			TileIdentityHelpers.Data[252] = new Dictionary<int, string> {
				{ -1, "Hay" }
			};
			TileIdentityHelpers.Data[253] = new Dictionary<int, string> {
				{ -1, "Spooky Wood" }
			};
			TileIdentityHelpers.Data[254] = new Dictionary<int, string> {
				{ -1, "Pumpkin Seed" }
			};
			TileIdentityHelpers.Data[255] = new Dictionary<int, string> {
				{ -1, "Amethyst Gemspark Block (offline)" }
			};
			TileIdentityHelpers.Data[256] = new Dictionary<int, string> {
				{ -1, "Topaz Gemspark Block (offline)" }
			};
			TileIdentityHelpers.Data[257] = new Dictionary<int, string> {
				{ -1, "Sapphire Gemspark Block (offline)" }
			};
			TileIdentityHelpers.Data[258] = new Dictionary<int, string> {
				{ -1, "Emerald Gemspark Block(offline)" }
			};
			TileIdentityHelpers.Data[259] = new Dictionary<int, string> {
				{ -1, "Ruby Gemspark Block (offline)" }
			};
			TileIdentityHelpers.Data[260] = new Dictionary<int, string> {
				{ -1, "Diamond Gemspark Block (offline)" }
			};
			TileIdentityHelpers.Data[261] = new Dictionary<int, string> {
				{ -1, "Amber Gemspark Block (offline)" }
			};
			TileIdentityHelpers.Data[262] = new Dictionary<int, string> {
				{ -1, "Amethyst Gemspark Block (online)" }
			};
			TileIdentityHelpers.Data[263] = new Dictionary<int, string> {
				{ -1, "Topaz Gemspark Block (online)" }
			};
			TileIdentityHelpers.Data[264] = new Dictionary<int, string> {
				{ -1, "Sapphire Gemspark Block (online)" }
			};
			TileIdentityHelpers.Data[265] = new Dictionary<int, string> {
				{ -1, "Emerald Gemspark Block (online)" }
			};
			TileIdentityHelpers.Data[266] = new Dictionary<int, string> {
				{ -1, "Ruby Gemspark Block (online)" }
			};
			TileIdentityHelpers.Data[267] = new Dictionary<int, string> {
				{ -1, "Diamond Gemspark Block (online)" }
			};
			TileIdentityHelpers.Data[268] = new Dictionary<int, string> {
				{ -1, "Amber Gemspark Block (online)" }
			};
			TileIdentityHelpers.Data[269] = new Dictionary<int, string> {
				{ -1, "Womannequin" }
			};
			TileIdentityHelpers.Data[270] = new Dictionary<int, string> {
				{ -1, "Firefly in a Bottle" }
			};
			TileIdentityHelpers.Data[271] = new Dictionary<int, string> {
				{ -1, "Lightning Bug in a Bottle" }
			};
			TileIdentityHelpers.Data[272] = new Dictionary<int, string> {
				{ -1, "Cog" }
			};
			TileIdentityHelpers.Data[273] = new Dictionary<int, string> {
				{ -1, "Stone Slab" }
			};
			TileIdentityHelpers.Data[274] = new Dictionary<int, string> {
				{ -1, "Sandstone Slab" }
			};
			TileIdentityHelpers.Data[275] = new Dictionary<int, string> {
				{ -1, "Bunny Cage" }
			};
			TileIdentityHelpers.Data[276] = new Dictionary<int, string> {
				{ -1, "Squirrel Cage" }
			};
			TileIdentityHelpers.Data[277] = new Dictionary<int, string> {
				{ -1, "Mallard Duck Cage" }
			};
			TileIdentityHelpers.Data[278] = new Dictionary<int, string> {
				{ -1, "Duck Cage" }
			};
			TileIdentityHelpers.Data[279] = new Dictionary<int, string> {
				{ -1, "Bird Cage" }
			};
			TileIdentityHelpers.Data[280] = new Dictionary<int, string> {
				{ -1, "Blue Jay Cage" }
			};
			TileIdentityHelpers.Data[281] = new Dictionary<int, string> {
				{ -1, "Cardinal Cage" }
			};
			TileIdentityHelpers.Data[282] = new Dictionary<int, string> {
				{ -1, "Fish Bowl" }
			};
			TileIdentityHelpers.Data[283] = new Dictionary<int, string> {
				{ -1, "Heavy Work Bench" }
			};
			TileIdentityHelpers.Data[284] = new Dictionary<int, string> {
				{ -1, "Copper Plating" }
			};
			TileIdentityHelpers.Data[285] = new Dictionary<int, string> {
				{ -1, "Snail Cage" }
			};
			TileIdentityHelpers.Data[286] = new Dictionary<int, string> {
				{ -1, "Glowing Snail Cage" }
			};
			TileIdentityHelpers.Data[287] = new Dictionary<int, string> {
				{ -1, "Ammo Box" }
			};
			TileIdentityHelpers.Data[288] = new Dictionary<int, string> {
				{ -1, "Monarch Butterfly Jar" }
			};
			TileIdentityHelpers.Data[289] = new Dictionary<int, string> {
				{ -1, "Purple Emperor Butterfly Jar" }
			};
			TileIdentityHelpers.Data[290] = new Dictionary<int, string> {
				{ -1, "Red Admiral Butterfly Jar" }
			};
			TileIdentityHelpers.Data[291] = new Dictionary<int, string> {
				{ -1, "Ulysses Butterfly Jar" }
			};
			TileIdentityHelpers.Data[292] = new Dictionary<int, string> {
				{ -1, "Sulphur Butterfly Jar" }
			};
			TileIdentityHelpers.Data[293] = new Dictionary<int, string> {
				{ -1, "Tree Nymph Butterfly Jar" }
			};
			TileIdentityHelpers.Data[294] = new Dictionary<int, string> {
				{ -1, "Zebra Swallowtail Butterfly Jar" }
			};
			TileIdentityHelpers.Data[295] = new Dictionary<int, string> {
				{ -1, "Julia Butterfly Jar" }
			};
			TileIdentityHelpers.Data[296] = new Dictionary<int, string> {
				{ -1, "Scorpion Cage" }
			};
			TileIdentityHelpers.Data[297] = new Dictionary<int, string> {
				{ -1, "Black Scorpion Cage" }
			};
			TileIdentityHelpers.Data[298] = new Dictionary<int, string> {
				{ -1, "Frog Cage" }
			};
			TileIdentityHelpers.Data[299] = new Dictionary<int, string> {
				{ -1, "Mouse Cage" }
			};
			TileIdentityHelpers.Data[300] = new Dictionary<int, string> {
				{ -1, "Bone Welder" }
			};
			TileIdentityHelpers.Data[301] = new Dictionary<int, string> {
				{ -1, "Flesh Cloning Vat" }
			};
			TileIdentityHelpers.Data[302] = new Dictionary<int, string> {
				{ -1, "Glass Kiln" }
			};
			TileIdentityHelpers.Data[303] = new Dictionary<int, string> {
				{ -1, "Lihzahrd Furnace" }
			};
			TileIdentityHelpers.Data[304] = new Dictionary<int, string> {
				{ -1, "Living Loom" }
			};
			TileIdentityHelpers.Data[305] = new Dictionary<int, string> {
				{ -1, "Sky Mill" }
			};
			TileIdentityHelpers.Data[306] = new Dictionary<int, string> {
				{ -1, "Ice Machine" }
			};
			TileIdentityHelpers.Data[307] = new Dictionary<int, string> {
				{ -1, "Steampunk Boiler" }
			};
			TileIdentityHelpers.Data[308] = new Dictionary<int, string> {
				{ -1, "Honey Dispenser" }
			};
			TileIdentityHelpers.Data[309] = new Dictionary<int, string> {
				{ -1, "Penguin Cage" }
			};
			TileIdentityHelpers.Data[310] = new Dictionary<int, string> {
				{ -1, "Worm Cage" }
			};
			TileIdentityHelpers.Data[311] = new Dictionary<int, string> {
				{ -1, "Dynasty Wood" }
			};
			TileIdentityHelpers.Data[312] = new Dictionary<int, string> {
				{ -1, "Red Dynasty Shingles" }
			};
			TileIdentityHelpers.Data[313] = new Dictionary<int, string> {
				{ -1, "Blue Dynasty Shingles" }
			};
			TileIdentityHelpers.Data[314] = new Dictionary<int, string> {
				{ 2, "Pressure Plate Track" },
				{ 3, "Booster Track" },
				{ -1, "Minecart Track" }
			};
			TileIdentityHelpers.Data[315] = new Dictionary<int, string> {
				{ -1, "Coralstone Block" }
			};
			TileIdentityHelpers.Data[316] = new Dictionary<int, string> {
				{ -1, "Blue Jellyfish Jar" }
			};
			TileIdentityHelpers.Data[317] = new Dictionary<int, string> {
				{ -1, "Green Jellyfish Jar" }
			};
			TileIdentityHelpers.Data[318] = new Dictionary<int, string> {
				{ -1, "Pink Jellyfish Jar" }
			};
			TileIdentityHelpers.Data[319] = new Dictionary<int, string> {
				{ -1, "Ship in a Bottle" }
			};
			TileIdentityHelpers.Data[320] = new Dictionary<int, string> {
				{ -1, "Seaweed Planter" }
			};
			TileIdentityHelpers.Data[321] = new Dictionary<int, string> {
				{ -1, "Boreal Wood" }
			};
			TileIdentityHelpers.Data[322] = new Dictionary<int, string> {
				{ -1, "Palm Wood" }
			};
			TileIdentityHelpers.Data[323] = new Dictionary<int, string> {
				{ -1, "Palm Tree" }
			};
			TileIdentityHelpers.Data[324] = new Dictionary<int, string> {
				{ 2, "Starfish" },
				{ -1, "Seashell" }
			};
			TileIdentityHelpers.Data[325] = new Dictionary<int, string> {
				{ -1, "Tin Plating" }
			};
			TileIdentityHelpers.Data[326] = new Dictionary<int, string> {
				{ -1, "Waterfall Block" }
			};
			TileIdentityHelpers.Data[327] = new Dictionary<int, string> {
				{ -1, "Lavafall Block" }
			};
			TileIdentityHelpers.Data[328] = new Dictionary<int, string> {
				{ -1, "Confetti Block" }
			};
			TileIdentityHelpers.Data[329] = new Dictionary<int, string> {
				{ -1, "Midnight Confetti Block" }
			};
			TileIdentityHelpers.Data[330] = new Dictionary<int, string> {
				{ -1, "Copper Coin Pile" }
			};
			TileIdentityHelpers.Data[331] = new Dictionary<int, string> {
				{ -1, "Silver Coin Pile" }
			};
			TileIdentityHelpers.Data[332] = new Dictionary<int, string> {
				{ -1, "Gold Coin Pile" }
			};
			TileIdentityHelpers.Data[333] = new Dictionary<int, string> {
				{ -1, "Platinum Coin Pile" }
			};
			TileIdentityHelpers.Data[334] = new Dictionary<int, string> {
				{ -1, "Weapon Rack" }
			};
			TileIdentityHelpers.Data[335] = new Dictionary<int, string> {
				{ -1, "Fireworks Box" }
			};
			TileIdentityHelpers.Data[336] = new Dictionary<int, string> {
				{ -1, "Living Fire Block" }
			};
			TileIdentityHelpers.Data[337] = new Dictionary<int, string> {
				{ -1, "Text Statue" }
			};
			TileIdentityHelpers.Data[338] = new Dictionary<int, string> {
				{ -1, "Firework Fountain" }
			};
			TileIdentityHelpers.Data[339] = new Dictionary<int, string> {
				{ -1, "Grasshopper Cage" }
			};
			TileIdentityHelpers.Data[340] = new Dictionary<int, string> {
				{ -1, "Living Cursed Fire Block" }
			};
			TileIdentityHelpers.Data[341] = new Dictionary<int, string> {
				{ -1, "Living Demon Fire Block" }
			};
			TileIdentityHelpers.Data[342] = new Dictionary<int, string> {
				{ -1, "Living Frost Fire Block" }
			};
			TileIdentityHelpers.Data[343] = new Dictionary<int, string> {
				{ -1, "Living Ichor Fire Block" }
			};
			TileIdentityHelpers.Data[344] = new Dictionary<int, string> {
				{ -1, "Living Ultrabright Fire Block" }
			};
			TileIdentityHelpers.Data[345] = new Dictionary<int, string> {
				{ -1, "Honeyfall Block" }
			};
			TileIdentityHelpers.Data[346] = new Dictionary<int, string> {
				{ -1, "Chlorophyte Brick" }
			};
			TileIdentityHelpers.Data[347] = new Dictionary<int, string> {
				{ -1, "Crimtane Brick" }
			};
			TileIdentityHelpers.Data[348] = new Dictionary<int, string> {
				{ -1, "Shroomite Plating" }
			};
			TileIdentityHelpers.Data[349] = new Dictionary<int, string> {
				{ -1, "Mushroom Statue" }
			};
			TileIdentityHelpers.Data[350] = new Dictionary<int, string> {
				{ -1, "Martian Conduit Plating" }
			};
			TileIdentityHelpers.Data[351] = new Dictionary<int, string> {
				{ -1, "Smoke Block" }
			};
			TileIdentityHelpers.Data[352] = new Dictionary<int, string> {
				{ -1, "Crimson Thorny Bush" }
			};
			TileIdentityHelpers.Data[353] = new Dictionary<int, string> {
				{ -1, "Vine Rope" }
			};
			TileIdentityHelpers.Data[354] = new Dictionary<int, string> {
				{ -1, "Bewitching Table" }
			};
			TileIdentityHelpers.Data[355] = new Dictionary<int, string> {
				{ -1, "Alchemy Table" }
			};
			TileIdentityHelpers.Data[356] = new Dictionary<int, string> {
				{ -1, "Enchanted Sundial" }
			};
			TileIdentityHelpers.Data[357] = new Dictionary<int, string> {
				{ -1, "Smooth Marble Block" }
			};
			TileIdentityHelpers.Data[358] = new Dictionary<int, string> {
				{ -1, "Gold Bird Cage" }
			};
			TileIdentityHelpers.Data[359] = new Dictionary<int, string> {
				{ -1, "Gold Bunny Cage" }
			};
			TileIdentityHelpers.Data[360] = new Dictionary<int, string> {
				{ -1, "Gold Butterfly Jar" }
			};
			TileIdentityHelpers.Data[361] = new Dictionary<int, string> {
				{ -1, "Gold Frog Cage" }
			};
			TileIdentityHelpers.Data[362] = new Dictionary<int, string> {
				{ -1, "Gold Grasshopper Cage" }
			};
			TileIdentityHelpers.Data[363] = new Dictionary<int, string> {
				{ -1, "Gold Mouse Cage" }
			};
			TileIdentityHelpers.Data[364] = new Dictionary<int, string> {
				{ -1, "Gold Worm Cage" }
			};
			TileIdentityHelpers.Data[365] = new Dictionary<int, string> {
				{ -1, "Silk Rope" }
			};
			TileIdentityHelpers.Data[366] = new Dictionary<int, string> {
				{ -1, "Web Rope" }
			};
			TileIdentityHelpers.Data[367] = new Dictionary<int, string> {
				{ -1, "Marble Block" }
			};
			TileIdentityHelpers.Data[368] = new Dictionary<int, string> {
				{ -1, "Granite Block" }
			};
			TileIdentityHelpers.Data[369] = new Dictionary<int, string> {
				{ -1, "Smooth Granite Block" }
			};
			TileIdentityHelpers.Data[370] = new Dictionary<int, string> {
				{ -1, "Meteorite Brick" }
			};
			TileIdentityHelpers.Data[371] = new Dictionary<int, string> {
				{ -1, "Pink Slime Block" }
			};
			TileIdentityHelpers.Data[372] = new Dictionary<int, string> {
				{ -1, "Peace Candle" }
			};
			TileIdentityHelpers.Data[373] = new Dictionary<int, string> {
				{ -1, "Magic Water Dropper" }
			};
			TileIdentityHelpers.Data[374] = new Dictionary<int, string> {
				{ -1, "Magic Lava Dropper" }
			};
			TileIdentityHelpers.Data[375] = new Dictionary<int, string> {
				{ -1, "Magic Honey Dropper" }
			};
			TileIdentityHelpers.Data[376] = new Dictionary<int, string> {
				{ 2, "Iron Crate" },
				{ 3, "Golden Crate" },
				{ 4, "Corrupt Crate" },
				{ 5, "Crimson Crate" },
				{ 6, "Dungeon Crate" },
				{ 7, "Sky Crate" },
				{ 8, "Hallowed Crate" },
				{ 9, "Jungle Crate" },
				{ -1, "Wooden Crate" }
			};
			TileIdentityHelpers.Data[377] = new Dictionary<int, string> {
				{ -1, "Sharpening Station" }
			};
			TileIdentityHelpers.Data[378] = new Dictionary<int, string> {
				{ -1, "Target Dummy" }
			};
			TileIdentityHelpers.Data[379] = new Dictionary<int, string> {
				{ -1, "Bubble" }
			};
			TileIdentityHelpers.Data[380] = new Dictionary<int, string> {
				{ -1, "Planter Box" }
			};
			TileIdentityHelpers.Data[381] = new Dictionary<int, string> {
				{ -1, "Fire Moss" }
			};
			TileIdentityHelpers.Data[382] = new Dictionary<int, string> {
				{ -1, "Vine Flowers" }
			};
			TileIdentityHelpers.Data[383] = new Dictionary<int, string> {
				{ -1, "Living Mahogany" }
			};
			TileIdentityHelpers.Data[384] = new Dictionary<int, string> {
				{ -1, "Mahogany Leaf Block" }
			};
			TileIdentityHelpers.Data[385] = new Dictionary<int, string> {
				{ -1, "Crystal Block" }
			};
			TileIdentityHelpers.Data[386] = new Dictionary<int, string> {
				{ -1, "Trap Door  (open)" }
			};
			TileIdentityHelpers.Data[387] = new Dictionary<int, string> {
				{ -1, "Trap Door  (closed)" }
			};
			TileIdentityHelpers.Data[388] = new Dictionary<int, string> {
				{ -1, "Tall Gate  (closed)" }
			};
			TileIdentityHelpers.Data[389] = new Dictionary<int, string> {
				{ -1, "Tall Gate  (open)" }
			};
			TileIdentityHelpers.Data[390] = new Dictionary<int, string> {
				{ -1, "Lava Lamp" }
			};
			TileIdentityHelpers.Data[391] = new Dictionary<int, string> {
				{ -1, "Enchanted Nightcrawler Cage" }
			};
			TileIdentityHelpers.Data[392] = new Dictionary<int, string> {
				{ -1, "Buggy Cage" }
			};
			TileIdentityHelpers.Data[393] = new Dictionary<int, string> {
				{ -1, "Grubby Cage" }
			};
			TileIdentityHelpers.Data[394] = new Dictionary<int, string> {
				{ -1, "Sluggy Cage" }
			};
			TileIdentityHelpers.Data[395] = new Dictionary<int, string> {
				{ -1, "Item Frame" }
			};
			TileIdentityHelpers.Data[396] = new Dictionary<int, string> {
				{ -1, "Sandstone Block" }
			};
			TileIdentityHelpers.Data[397] = new Dictionary<int, string> {
				{ -1, "Hardened Sand Block" }
			};
			TileIdentityHelpers.Data[398] = new Dictionary<int, string> {
				{ -1, "Hardened Ebonsand Block" }
			};
			TileIdentityHelpers.Data[399] = new Dictionary<int, string> {
				{ -1, "Hardened Crimsand Block" }
			};
			TileIdentityHelpers.Data[400] = new Dictionary<int, string> {
				{ -1, "Ebonsandstone Block" }
			};
			TileIdentityHelpers.Data[401] = new Dictionary<int, string> {
				{ -1, "Crimsandstone Block" }
			};
			TileIdentityHelpers.Data[402] = new Dictionary<int, string> {
				{ -1, "Hardened Pearlsand Block" }
			};
			TileIdentityHelpers.Data[403] = new Dictionary<int, string> {
				{ -1, "Pearlsandstone Block" }
			};
			TileIdentityHelpers.Data[404] = new Dictionary<int, string> {
				{ -1, "Desert Fossil" }
			};
			TileIdentityHelpers.Data[405] = new Dictionary<int, string> {
				{ -1, "Fireplace" }
			};
			TileIdentityHelpers.Data[406] = new Dictionary<int, string> {
				{ -1, "Chimney" }
			};
			TileIdentityHelpers.Data[407] = new Dictionary<int, string> {
				{ -1, "Sturdy Fossil" }
			};
			TileIdentityHelpers.Data[408] = new Dictionary<int, string> {
				{ -1, "Luminite" }
			};
			TileIdentityHelpers.Data[409] = new Dictionary<int, string> {
				{ -1, "Luminite Brick" }
			};
			TileIdentityHelpers.Data[410] = new Dictionary<int, string> {
				{ 2, "Nebula Monolith" },
				{ 3, "Stardust Monolith" },
				{ 4, "Solar Monolith" },
				{ -1, "Vortex Monolith" }
			};
			TileIdentityHelpers.Data[411] = new Dictionary<int, string> {
				{ -1, "Detonator" }
			};
			TileIdentityHelpers.Data[412] = new Dictionary<int, string> {
				{ -1, "Ancient Manipulator" }
			};
			TileIdentityHelpers.Data[413] = new Dictionary<int, string> {
				{ -1, "Red Squirrel Cage" }
			};
			TileIdentityHelpers.Data[414] = new Dictionary<int, string> {
				{ -1, "Gold Squirrel Cage" }
			};
			TileIdentityHelpers.Data[415] = new Dictionary<int, string> {
				{ -1, "Solar Fragment Block" }
			};
			TileIdentityHelpers.Data[416] = new Dictionary<int, string> {
				{ -1, "Vortex Fragment Block" }
			};
			TileIdentityHelpers.Data[417] = new Dictionary<int, string> {
				{ -1, "Nebula Fragment Block" }
			};
			TileIdentityHelpers.Data[418] = new Dictionary<int, string> {
				{ -1, "Stardust Fragment Block" }
			};
			TileIdentityHelpers.Data[419] = new Dictionary<int, string> {
				{ -1, "Logic Gate Lamp" }
			};
			TileIdentityHelpers.Data[420] = new Dictionary<int, string> {
				{ -1, "Logic Gate" }
			};
			TileIdentityHelpers.Data[421] = new Dictionary<int, string> {
				{ -1, "Conveyor Belt (Clockwise)" }
			};
			TileIdentityHelpers.Data[422] = new Dictionary<int, string> {
				{ -1, "Conveyor Belt (Counter Clockwise)" }
			};
			TileIdentityHelpers.Data[423] = new Dictionary<int, string> {
				{ -1, "Logic Sensor" }
			};
			TileIdentityHelpers.Data[424] = new Dictionary<int, string> {
				{ -1, "Junction Box" }
			};
			TileIdentityHelpers.Data[425] = new Dictionary<int, string> {
				{ -1, "Announcement Box" }
			};
			TileIdentityHelpers.Data[426] = new Dictionary<int, string> {
				{ -1, "Red Team Block" }
			};
			TileIdentityHelpers.Data[427] = new Dictionary<int, string> {
				{ -1, "Red Team Platform" }
			};
			TileIdentityHelpers.Data[428] = new Dictionary<int, string> {
				{ -1, "Weighted Pressure Plate" }
			};
			TileIdentityHelpers.Data[429] = new Dictionary<int, string> {
				{ -1, "Wire Bulb" }
			};
			TileIdentityHelpers.Data[430] = new Dictionary<int, string> {
				{ -1, "Green Team Block" }
			};
			TileIdentityHelpers.Data[431] = new Dictionary<int, string> {
				{ -1, "Blue Team Block" }
			};
			TileIdentityHelpers.Data[432] = new Dictionary<int, string> {
				{ -1, "Yellow Team Block" }
			};
			TileIdentityHelpers.Data[433] = new Dictionary<int, string> {
				{ -1, "Pink Team Block" }
			};
			TileIdentityHelpers.Data[434] = new Dictionary<int, string> {
				{ -1, "White Team Block" }
			};
			TileIdentityHelpers.Data[435] = new Dictionary<int, string> {
				{ -1, "Green Team Platform" }
			};
			TileIdentityHelpers.Data[436] = new Dictionary<int, string> {
				{ -1, "Blue Team Platform" }
			};
			TileIdentityHelpers.Data[437] = new Dictionary<int, string> {
				{ -1, "Yellow Team Platform" }
			};
			TileIdentityHelpers.Data[438] = new Dictionary<int, string> {
				{ -1, "Pink Team Platform" }
			};
			TileIdentityHelpers.Data[439] = new Dictionary<int, string> {
				{ -1, "White Team Platform" }
			};
			TileIdentityHelpers.Data[440] = new Dictionary<int, string> {
				{ 2, "Sapphire Gem Lock" },
				{ 3, "Emerald Gem Lock" },
				{ 4, "Topaz Gem Lock" },
				{ 5, "Amethyst Gem Lock" },
				{ 6, "Diamond Gem Lock" },
				{ 7, "Amber Gem Lock" },
				{ -1, "Ruby Gem Lock" }
			};
			TileIdentityHelpers.Data[441] = new Dictionary<int, string> {
				{ 2, "Trapped Gold Chest" },
				{ 3, "Trapped Locked Gold Chest" },
				{ 4, "Trapped Shadow Chest" },
				{ 5, "Trapped Locked Shadow Chest" },
				{ 6, "Trapped Barrel" },
				{ 7, "Trapped Trash Can" },
				{ 8, "Trapped Ebonwood Chest" },
				{ 9, "Trapped Rich Mahogany Chest" },
				{ 10, "Trapped Pearlwood Chest" },
				{ 11, "Trapped Ivy Chest" },
				{ 12, "Trapped Ice Chest" },
				{ 13, "Trapped Living Wood Chest" },
				{ 14, "Trapped Skyware Chest" },
				{ 15, "Trapped Shadewood Chest" },
				{ 16, "Trapped Web Covered Chest" },
				{ 17, "Trapped Lihzahrd Chest" },
				{ 18, "Trapped Water Chest" },
				{ 19, "Trapped Jungle Chest" },
				{ 20, "Trapped Corruption Chest" },
				{ 21, "Trapped Crimson Chest" },
				{ 22, "Trapped Hallowed Chest" },
				{ 23, "Trapped Frozen Chest" },
				{ 24, "Trapped Locked Jungle Chest" },
				{ 25, "Trapped Locked Corruption Chest" },
				{ 26, "Trapped Locked Crimson Chest" },
				{ 27, "Trapped Locked Hallowed Chest" },
				{ 28, "Trapped Locked Frozen Chest" },
				{ 29, "Trapped Dynasty Chest" },
				{ 30, "Trapped Honey Chest" },
				{ 31, "Trapped Steampunk Chest" },
				{ 32, "Trapped Palm Wood Chest" },
				{ 33, "Trapped Mushroom Chest" },
				{ 34, "Trapped Boreal Wood Chest" },
				{ 35, "Trapped Slime Chest" },
				{ 36, "Trapped Green Dungeon Chest" },
				{ 37, "Trapped Locked Green Dungeon Chest" },
				{ 38, "Trapped Pink Dungeon Chest" },
				{ 39, "Trapped Locked Pink Dungeon Chest" },
				{ 40, "Trapped Blue Dungeon Chest" },
				{ 41, "Trapped Locked Blue Dungeon Chest" },
				{ 42, "Trapped Bone Chest" },
				{ 43, "Trapped Cactus Chest" },
				{ 44, "Trapped Flesh Chest" },
				{ 45, "Trapped Obsidian Chest" },
				{ 46, "Trapped Pumpkin Chest" },
				{ 47, "Trapped Spooky Chest" },
				{ 48, "Trapped Glass Chest" },
				{ 49, "Trapped Martian Chest" },
				{ 50, "Trapped Meteorite Chest" },
				{ 51, "Trapped Granite Chest" },
				{ 52, "Trapped Marble Chest" },
				{ -1, "Trapped Chest" }
			};
			TileIdentityHelpers.Data[442] = new Dictionary<int, string> {
				{ -1, "Teal Pressure Pad" }
			};
			TileIdentityHelpers.Data[443] = new Dictionary<int, string> {
				{ -1, "Geyser" }
			};
			TileIdentityHelpers.Data[444] = new Dictionary<int, string> {
				{ -1, "Beehive" }
			};
			TileIdentityHelpers.Data[445] = new Dictionary<int, string> {
				{ -1, "Pixel Box" }
			};
			TileIdentityHelpers.Data[446] = new Dictionary<int, string> {
				{ -1, "Silly Pink Balloon" }
			};
			TileIdentityHelpers.Data[447] = new Dictionary<int, string> {
				{ -1, "Silly Purple Balloon" }
			};
			TileIdentityHelpers.Data[448] = new Dictionary<int, string> {
				{ -1, "Silly Green Balloon" }
			};
			TileIdentityHelpers.Data[449] = new Dictionary<int, string> {
				{ -1, "Blue Streamer" }
			};
			TileIdentityHelpers.Data[450] = new Dictionary<int, string> {
				{ -1, "Green Streamer" }
			};
			TileIdentityHelpers.Data[451] = new Dictionary<int, string> {
				{ -1, "Pink Streamer" }
			};
			TileIdentityHelpers.Data[452] = new Dictionary<int, string> {
				{ -1, "Silly Balloon Machine" }
			};
			TileIdentityHelpers.Data[453] = new Dictionary<int, string> {
				{ -1, "Silly Tied Balloons" }
			};
			TileIdentityHelpers.Data[454] = new Dictionary<int, string> {
				{ -1, "Pigronata" }
			};
			TileIdentityHelpers.Data[455] = new Dictionary<int, string> {
				{ -1, "Party Center" }
			};
			TileIdentityHelpers.Data[456] = new Dictionary<int, string> {
				{ -1, "Silly Tied Bundle of Balloons" }
			};
			TileIdentityHelpers.Data[457] = new Dictionary<int, string> {
				{ -1, "Party Present" }
			};
			TileIdentityHelpers.Data[458] = new Dictionary<int, string> {
				{ -1, "Sandfall Block" }
			};
			TileIdentityHelpers.Data[459] = new Dictionary<int, string> {
				{ -1, "Snowfall Block" }
			};
			TileIdentityHelpers.Data[460] = new Dictionary<int, string> {
				{ -1, "Snow Cloud" }
			};
			TileIdentityHelpers.Data[461] = new Dictionary<int, string> {
				{ -1, "Magic Sand Dropper" }
			};
			TileIdentityHelpers.Data[462] = new Dictionary<int, string> {
				{ -1, "Desert Spirit Lamp" }
			};
			TileIdentityHelpers.Data[463] = new Dictionary<int, string> {
				{ -1, "Defender's Forge" }
			};
			TileIdentityHelpers.Data[464] = new Dictionary<int, string> {
				{ -1, "War Table" }
			};
			TileIdentityHelpers.Data[465] = new Dictionary<int, string> {
				{ -1, "War Table Banner" }
			};
			TileIdentityHelpers.Data[466] = new Dictionary<int, string> {
				{ -1, "Eternia Crystal Stand" }
			};
			TileIdentityHelpers.Data[467] = new Dictionary<int, string> {
				{ 2, "Golden Chest" },
				{ -1, "Crystal Chest" }
			};
			TileIdentityHelpers.Data[468] = new Dictionary<int, string> {
				{ 2, "Trapped Golden Chest" },
				{ -1, "Trapped Crystal Chest" }
			};
			TileIdentityHelpers.Data[469] = new Dictionary<int, string> {
				{ -1, "Crystal Table" }
			};
			} catch( Exception e ) {
				ErrorLogger.Log( e.ToString() );
			}
		}
	}
}
