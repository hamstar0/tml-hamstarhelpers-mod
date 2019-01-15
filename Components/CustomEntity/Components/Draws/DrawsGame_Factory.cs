using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public partial class DrawsInGameEntityComponent : CustomEntityComponent {
		protected class DrawsInGameEntityComponentFactory<T> : CustomEntityComponentFactory<T> where T : DrawsInGameEntityComponent {
			public readonly string SourceModName;
			public readonly string TexturePath;
			public readonly int FrameCount;


			////////////////

			public DrawsInGameEntityComponentFactory( string srcModName, string relTexturePath, int frameCount ) {
				this.SourceModName = srcModName;
				this.TexturePath = relTexturePath;
				this.FrameCount = frameCount;
			}

			////

			protected sealed override void InitializeComponent( T data ) {
				data.ModName = this.SourceModName;
				data.TexturePath = this.TexturePath;
				data.FrameCount = this.FrameCount;
				
				this.InitializeDrawsInGameEntityComponent( data );
			}

			protected virtual void InitializeDrawsInGameEntityComponent( T data ) { }
		}



		////////////////

		public static DrawsInGameEntityComponent CreateDrawsInGameEntityComponent( string srcModName, string relTexturePath, int frameCount ) {
			var factory = new DrawsInGameEntityComponentFactory<DrawsInGameEntityComponent>( srcModName, relTexturePath, frameCount );
			return factory.Create();
		}
	}
}
