using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public partial class DrawsOnMapEntityComponent : CustomEntityComponent {
		protected class DrawsOnMapEntityComponentFactory<T> : CustomEntityComponentFactory<T> where T : DrawsOnMapEntityComponent {
			public readonly string SourceModName;
			public readonly string RelativeTexturePath;
			public readonly int FrameCount;
			public readonly float Scale;
			public readonly bool Zooms;


			////////////////

			public DrawsOnMapEntityComponentFactory( string srcModName, string relTexturePath, int frameCount, float scale, bool zooms ) {
				this.SourceModName = srcModName;
				this.RelativeTexturePath = relTexturePath;
				this.FrameCount = frameCount;
				this.Scale = scale;
				this.Zooms = zooms;
			}

			////

			protected sealed override void InitializeComponent( T data ) {
				data.ModName = this.SourceModName;
				data.TexturePath = this.RelativeTexturePath;
				data.FrameCount = this.FrameCount;
				data.Scale = this.Scale;
				data.Zooms = this.Zooms;

				this.InitializeDrawsOnMapEntityComponent( data );
			}

			protected virtual void InitializeDrawsOnMapEntityComponent( T data ) { }
		}



		////////////////

		public static DrawsOnMapEntityComponent CreateDrawsOnMapEntityComponent( string srcModName, string relTexturePath, int frameCount, float scale, bool zooms ) {
			var factory = new DrawsOnMapEntityComponentFactory<DrawsOnMapEntityComponent>( srcModName, relTexturePath, frameCount, scale, zooms );
			return factory.Create();
		}
	}
}
