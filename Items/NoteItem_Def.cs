using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace HamstarHelpers.Items {
	/// <summary>
	/// A readable note.
	/// </summary>
	public partial class NoteItem : ModItem {
		/// <summary></summary>
		public string TitleText { get; private set; } = "Lorem Ipsum";

		/// <summary></summary>
		public string[] BodyTexts { get; private set; } = new string[] {
			"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
			"Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
			"Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur."
		};


		////////////////

		/*private bool? _IsScribingEnabled = null;

		/// <summary>
		/// Reports if config enables scribing notes. Manually setting this value overrides config `NoteWritingEnabled`
		/// setting.
		/// </summary>
		public bool? IsScribingEnabled {
			get {
				if( !this._IsScribingEnabled.HasValue ) {
					return ModHelpersConfig.Instance.NoteWritingEnabled;
				}
				return this._IsScribingEnabled;
			}
			set {
				this._IsScribingEnabled = value;
			}
		}*/



		////////////////

		/// @private
		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Notebook" );
			this.Tooltip.SetDefault( "Contains written information." );
		}


		/// @private
		public override void SetDefaults() {
			this.item.maxStack = 1;
			this.item.width = 24;
			this.item.height = 24;
			this.item.value = Item.buyPrice( 0, 0, 0, 75 );
			this.item.rare = ItemRarityID.Blue;
		}


		////////////////
		
		/// @private
		public override void NetSend( BinaryWriter writer ) {
			writer.Write( this.TitleText );
			writer.Write( this.BodyTexts.Length );
			foreach( string text in this.BodyTexts ) {
				writer.Write( text );
			}
		}

		/// @private
		public override void NetRecieve( BinaryReader reader ) {
			this.TitleText = reader.ReadString();

			int pages = reader.ReadInt32();
			this.BodyTexts = new string[ pages ];

			for( int i=0; i<pages; i++ ) {
				this.BodyTexts[i] = reader.ReadString();
			}
		}


		////////////////

		/// @private
		public override bool CanRightClick() {
			this.DisplayCurrentNote();
			return false;
		}
	}
}
