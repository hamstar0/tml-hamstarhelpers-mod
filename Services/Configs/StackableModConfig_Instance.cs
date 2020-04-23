using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using System;
using Terraria.ModLoader.Config;


namespace HamstarHelpers.Services.Configs {
	/// <summary>
	/// Helps implement mod config stacking behavior. Replace your ModConfig classes with this class.
	/// </summary>
	public abstract class StackableModConfig : ModConfig {
		/// @private
		public override void OnChanged() {
			ModConfigStack.Uncache( this.GetType() );
		}


		/// <summary>
		/// Convenience method for pulling changed settings from a given config instance into (the stack of) the current one
		/// (calls `ModConfigStack.SetStackedConfigChangesOnlyForType(...)`).
		/// </summary>
		/// <param name="changes"></param>
		public virtual void OverlayChanges( StackableModConfig changes ) {
			Type myType = changes.GetType();

			if( myType != this.GetType() ) {
				throw new ModHelpersException( "Mismatched StackableModConfig types; found "+changes.GetType().Name
					+", expected "+this.GetType().Name );
			}
			ModConfigStack.SetStackedConfigChangesOnlyForType( myType, changes );
		}

		/*public override bool NeedsReload( ModConfig pendingConfig ) {
			foreach( PropertyFieldWrapper variable in ConfigManager.GetFieldsAndProperties( this ) ) {
				var reloadRequired = ConfigManager.GetCustomAttribute<ReloadRequiredAttribute>( variable, this, null );

				if( reloadRequired != null ) {
					// Do we need to implement nested ReloadRequired? Right now only top level fields will trigger it.
					if( !ConfigManager.ObjectEquals( variable.GetValue(this), variable.GetValue(pendingConfig) ) ) {
LogHelpers.Log("RELOADS BECAUSE "+variable.Name+": "+variable.GetValue(this)+" vs "+variable.GetValue(pendingConfig));
						return true;
					}
				}
			}
			return false;
		}*/
	}
}
