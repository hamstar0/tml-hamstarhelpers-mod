using HamstarHelpers.Helpers.DebugHelpers;


namespace HamstarHelpers.Components.CustomEntity.Components {
	public partial class ShootsAtPlayerEntityComponent : CustomEntityComponent {
		protected class ShootsAtPlayerEntityComponentFactory<T> : CustomEntityComponentFactory<T> where T : ShootsAtPlayerEntityComponent {
			public int ProjectileType;
			public int MinRange;
			public int MaxRange;
			public bool IgnoresSolidCover;
			public int Cooldown;
			public int MaxCooldown;


			////////////////

			public ShootsAtPlayerEntityComponentFactory( int projectileType, int minRange, int maxRange, bool ignoresSolidCover,
					int cooldown, int maxCooldown ) {
				this.ProjectileType = projectileType;
				this.MinRange = minRange;
				this.MaxRange = maxRange;
				this.IgnoresSolidCover = ignoresSolidCover;
				this.Cooldown = cooldown;
				this.MaxCooldown = maxCooldown;
			}

			////

			protected sealed override void InitializeComponent( T data ) {
				data.ProjectileType = this.ProjectileType;

				this.InitializeShootsProjectileEntityComponent( data );
			}

			protected virtual void InitializeShootsProjectileEntityComponent( T data ) { }
		}



		////////////////

		public static ShootsAtPlayerEntityComponent CreateShootsProjectileEntityComponent( int projectileType, int minRange, int maxRange,
				bool ignoresSolidCover, int cooldown, int maxCooldown ) {
			var factory = new ShootsAtPlayerEntityComponentFactory<ShootsAtPlayerEntityComponent>( projectileType, minRange, maxRange,
				ignoresSolidCover, cooldown, maxCooldown );
			return factory.Create();
		}
	}
}
