using HamstarHelpers.Helpers.ItemHelpers;
using HamstarHelpers.Helpers.TmlHelpers.CommandsHelpers;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;


namespace HamstarHelpers.Commands {
	public class GetItemIdByNameCommand : ModCommand {
		public override CommandType Type => CommandType.Chat;
		public override string Command => "mh-get-item-id";
		public override string Usage => "/" +this.Command+" \"Gold Pickaxe\"";
		public override string Description => "Gets an item's id by name. Must be wrapped with quotes.";


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			var mymod = (ModHelpersMod)this.mod;

			if( args.Length == 0 ) {
				throw new UsageException("No arguments supplied.");
			}

			int _;
			string itemName = CommandsHelpers.GetQuotedStringFromArgsAt( args, 0, out _ );
			if( !ItemIdentityHelpers.NamesToIds.ContainsKey( itemName ) ) {
				throw new UsageException( "Invalid item type." );
			}

			caller.Reply( "Item id for " + itemName + ": " + ItemIdentityHelpers.NamesToIds[itemName], Color.Lime );
		}
	}
}
