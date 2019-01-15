using HamstarHelpers.Helpers.DebugHelpers;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public partial class SaveableEntityComponent : CustomEntityComponent {
		protected class SaveableEntityComponentFactory<T> : CustomEntityComponentFactory<T> where T : SaveableEntityComponent {
			public readonly bool AsJson;


			////////////////

			public SaveableEntityComponentFactory( bool asJson ) {
				this.AsJson = asJson;
			}

			////

			protected sealed override void InitializeComponent( T data ) {
				data.AsJson = this.AsJson;

				this.InitializeSaveableEntityComponent( data );
			}

			protected virtual void InitializeSaveableEntityComponent( T data ) { }
		}



		////////////////

		public static SaveableEntityComponent CreateSaveableEntityComponent( bool asJson ) {
			var factory = new SaveableEntityComponentFactory<SaveableEntityComponent>( asJson );
			return factory.Create();
		}
	}
}
