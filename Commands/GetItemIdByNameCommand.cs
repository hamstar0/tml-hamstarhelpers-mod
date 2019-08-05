using HamstarHelpers.Helpers.Items;
using HamstarHelpers.Helpers.TModLoader.Commands;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;


namespace HamstarHelpers.Commands {
	/// @private
	public class GetItemIdByNameCommand : ModCommand {
		/// @private
		public override CommandType Type => CommandType.Chat;
		/// @private
		public override string Command => "mh-get-item-id";
		/// @private
		public override string Usage => "/" +this.Command+" \"Gold Pickaxe\"";
		/// @private
		public override string Description => "Gets an item's id by name. Must be wrapped with quotes.";


		////////////////

		/// @private
		public override void Action( CommandCaller caller, string input, string[] args ) {
			var mymod = (ModHelpersMod)this.mod;

			if( args.Length == 0 ) {
				throw new UsageException("No arguments supplied.");
			}

			int _;
			string itemName;
			if( CommandsHelpers.GetQuotedStringFromArgsAt(args, 0, out _, out itemName) ) {
				if( !ItemIdentityHelpers.DisplayNamesToIds.ContainsKey( itemName ) ) {
					throw new UsageException( "Invalid item type." );
				}

				caller.Reply( "Item id for " + itemName + ": " + ItemIdentityHelpers.DisplayNamesToIds[itemName], Color.Lime );
			}
		}
	}
}
