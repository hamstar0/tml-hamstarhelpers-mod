using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Utilities;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Loadable;
using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Helpers.TModLoader;


namespace HamstarHelpers.Services.NPCChat {
	/// <summary>
	/// Provides a service for adding or removing town NPC chats (based on weight values).
	/// </summary>
	public class NPCChat : ILoadable {
		/// <summary>
		/// Gets a chat message for the given NPC. Pulls from its default pool and any added chats, also excludes any
		/// default chats matching any removal patterns.
		/// </summary>
		/// <param name="npc"></param>
		/// <param name="chat">Unless removal patterns specify otherwise, may either be untouched, or else a new chat from
		/// the added pool.</param>
		/// <param name="defaultChatsTotalWeight"></param>
		/// <returns>Returns `true` if a new chat message was picked, `false` if the no new chat was picked, or `null` if
		/// another attempt (`npc.GetChat()`) is needed to get a new chat message.</returns>
		public static bool? GetChat( NPC npc, ref string chat, float defaultChatsTotalWeight = 1f ) {
			NPCChat nc = TmlHelpers.SafelyGetInstance<NPCChat>();
			
			UnifiedRandom rand = TmlHelpers.SafelyGetRand();
			float totalWeight = defaultChatsTotalWeight;
			totalWeight += nc.AddedChats.GetOrDefault(npc.type)?
					.Select( wc => wc.Weight )
					.Sum()
				?? 0f;
			
			if( (rand.NextFloat() * totalWeight) < defaultChatsTotalWeight ) {
				bool isRemoved = !string.IsNullOrEmpty( chat )
					? NPCChat.IsChatRemoved( npc.type, chat )
					: false;

				return isRemoved
					? null
					: (bool?)false;
			}

			chat = NPCChat.GetNewChat( npc.type );
			if( chat == null ) {
				throw new ModHelpersException( "No new chats available for "+npc.TypeName );
			}
			return true;
		}

		////

		/// <summary>
		/// Gets a random chat only from the given NPC's added chats pool.
		/// </summary>
		/// <param name="npcType"></param>
		/// <returns></returns>
		public static string GetNewChat( int npcType ) {
			NPCChat nc = TmlHelpers.SafelyGetInstance<NPCChat>();

			float totalWeight = nc.AddedChats.GetOrDefault( npcType )?
					.Select( wc => wc.Weight )
					.Sum()
				?? 0f;
			if( totalWeight == 0f ) {
				return null;
			}

			UnifiedRandom rand = TmlHelpers.SafelyGetRand();
			float guessWeight = rand.NextFloat() * totalWeight;

			float countedWeights = 0;
			foreach( (float weight, string chat) in nc.AddedChats[npcType] ) {
				countedWeights += weight;
				if( guessWeight > countedWeights ) {
					continue;
				}

				return chat;
			}
			return null;
		}


		////

		/// <summary>
		/// Indicates if a given chat messages is matched for removal by any of the removal patterns.
		/// </summary>
		/// <param name="npcType"></param>
		/// <param name="chat"></param>
		/// <returns></returns>
		public static bool IsChatRemoved( int npcType, string chat ) {
			NPCChat nc = TmlHelpers.SafelyGetInstance<NPCChat>();

			if( nc.RemovedChatFlatPatterns.ContainsKey( npcType ) ) {
				foreach( string pattern in nc.RemovedChatFlatPatterns[npcType] ) {
					if( chat.Contains( pattern ) ) {
						return true;
					}
				}
			}
			return false;
		}


		////////////////
		
		/// <summary>
		/// Gets all added chats (and their weights) for a given NPC.
		/// </summary>
		/// <param name="npcType"></param>
		/// <returns></returns>
		public static IEnumerable<(float Weight, string Chat)> GetAddedChats( int npcType ) {
			NPCChat nc = TmlHelpers.SafelyGetInstance<NPCChat>();

			return new List<(float, string)>(
				nc.AddedChats.GetOrDefault( npcType )
				?? new List<(float, string)>()
			);
		}

		/// <summary>
		/// Removes a specific added chat.
		/// </summary>
		/// <param name="npcType"></param>
		/// <param name="chat"></param>
		/// <returns></returns>
		public static bool RemoveAddedChat( int npcType, string chat ) {
			NPCChat nc = TmlHelpers.SafelyGetInstance<NPCChat>();
			if( !nc.AddedChats.ContainsKey(npcType) ) {
				return false;
			}

			IList<(float Weight, string Chat)> chats = nc.AddedChats[npcType];
			int count = chats.Count;
			for( int i=0; i<count; i++ ) {
				if( !chats[i].Chat.Equals(chat) ) {
					continue;
				}

				chats.RemoveAt( i );
				return true;
			}

			return false;
		}


		////////////////

		/// <summary>
		/// Adds a new chat message for a given NPC.
		/// </summary>
		/// <param name="npcType"></param>
		/// <param name="chat"></param>
		/// <param name="weight">How much favor is given to this chat to be picked from the NPC's pool. Note: All of an
		/// NPC's default chats have a total weight of `1f`.</param>
		public static void AddChatForNPC( int npcType, string chat, float weight=0.1f ) {
			NPCChat nc = TmlHelpers.SafelyGetInstance<NPCChat>();
			nc.AddedChats.Append2D( npcType, (weight, chat) );
		}


		/// <summary>
		/// Adds a pattern to match existing (non-pool added) chats.
		/// </summary>
		/// <param name="npcType"></param>
		/// <param name="chatFlatPattern"></param>
		public static void AddChatRemoveFlatPatternForNPC( int npcType, string chatFlatPattern ) {
			NPCChat nc = TmlHelpers.SafelyGetInstance<NPCChat>();
			nc.RemovedChatFlatPatterns.Append2D( npcType, chatFlatPattern );
		}


		////////////////

		/// <summary>
		/// Retrieves the current priority chat message getter.
		/// </summary>
		/// <param name="npcType"></param>
		/// <returns></returns>
		public static Func<string, string> GetPriorityChat( int npcType ) {
			NPCChat nc = TmlHelpers.SafelyGetInstance<NPCChat>();
			return nc.PriorityChat.GetOrDefault( npcType );
		}

		/// <summary>
		/// Sets the current priority chat message getter for a given NPC.
		/// </summary>
		/// <param name="npcType"></param>
		/// <param name="chatGet">Accepts an optional parameter of the current chat, and returns the priority chat.
		/// Return `null` if no priority chat exists.</param>
		public static void SetPriorityChat( int npcType, Func<string, string> chatGet ) {
			NPCChat nc = TmlHelpers.SafelyGetInstance<NPCChat>();
			nc.PriorityChat[npcType] = chatGet;
		}



		////////////////

		private IDictionary<int, Func<string, string>> PriorityChat = new Dictionary<int, Func<string, string>>();
		private IDictionary<int, IList<(float Weight, string Chat)>> AddedChats = new Dictionary<int, IList<(float, string)>>();
		private IDictionary<int, IList<string>> RemovedChatFlatPatterns = new Dictionary<int, IList<string>>();



		////////////////

		void ILoadable.OnModsLoad() { }

		void ILoadable.OnPostModsLoad() { }

		void ILoadable.OnModsUnload() { }
	}
}
