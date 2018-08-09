using HamstarHelpers.Helpers.ItemHelpers;
using HamstarHelpers.Helpers.TmlHelpers.CommandsHelpers;
using Terraria.ModLoader;


namespace HamstarHelpers.Commands {
	public class GetItemIdByNameCommand : ModCommand {
		public override CommandType Type { get { return CommandType.Chat; } }
		public override string Command { get { return "mhgetitemid"; } }
		public override string Usage { get { return "/"+this.Command+" \"Gold Pickaxe\""; } }
		public override string Description { get { return "Gets an item's id by name. Must be wrapped with quotes."; } }


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			var mymod = (HamstarHelpersMod)this.mod;

			if( args.Length == 0 ) {
				throw new UsageException("No arguments supplied.");
			}

			int _;
			string item_name = CommandsHelpers.GetQuotedStringFromArgsAt( args, 0, out _ );
			if( !ItemIdentityHelpers.NamesToIds.ContainsKey( item_name ) ) {
				throw new UsageException( "Invalid item type." );
			}

			caller.Reply( "Item id for " + item_name + ": " + ItemIdentityHelpers.NamesToIds[item_name] );
		}
	}
}
