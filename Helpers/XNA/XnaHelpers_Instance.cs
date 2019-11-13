using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;
using System;
using System.Reflection;
using Terraria;


namespace HamstarHelpers.Helpers.XNA {
	/// @private
	public partial class XNAHelpers {
		private FieldInfo SpriteBatchBegunField = null;



		////////////////

		internal XNAHelpers() {
			if( Main.dedServ || Main.netMode == 2 ) { return; }

			Type sbType = Main.spriteBatch.GetType();
			this.SpriteBatchBegunField = sbType.GetField( "inBeginEndPair", ReflectionHelpers.MostAccess );

			if( this.SpriteBatchBegunField == null ) {
				this.SpriteBatchBegunField = sbType.GetField( "_beginCalled", ReflectionHelpers.MostAccess );
			}
			if( this.SpriteBatchBegunField == null ) {
				this.SpriteBatchBegunField = sbType.GetField( "beginCalled", ReflectionHelpers.MostAccess );
			}
		}
	}
}
