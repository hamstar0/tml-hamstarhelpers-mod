using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Components.CustomEntity {
	class CustomEntityCoreContractResolver : DefaultContractResolver {
		protected override IList<JsonProperty> CreateProperties( Type type, MemberSerialization member_serialization ) {
			IList<JsonProperty> properties = base.CreateProperties( type, member_serialization );

			properties = properties.Where( ( p ) => {
				if( p.DeclaringType.Name != "Entity" ) { return true; }

				switch( p.PropertyName ) {
				//case "whoAmI":
				case "lavaWet":
				case "wetCount":
				case "wet":
				case "honeyWet":
				case "oldVelocity":
				case "oldPosition":
				case "velocity":
				case "active":
				case "oldDirection":
				case "BottomRight":
				case "BottomLeft":
				case "Bottom":
				case "TopRight":
				case "TopLeft":
				case "Top":
				case "Right":
				case "Left":
				case "Hitbox":
				case "Size":
				case "Center":
					return false;
				}

				return true;
			} ).ToList();

			return properties;
		}
	}




	public partial class CustomEntityCore : Entity {
		public string DisplayName = "";

		////////////////

		[PacketProtocolIgnore]
		[JsonIgnore]
		public int WhoAmI {
			get { return this.whoAmI; }
			private set { this.whoAmI = value; }
		}

		[PacketProtocolIgnore]
		[JsonIgnore]
		public Vector2 Position {
			get { return this.position; }
			set { this.position = value; }
		}

		[PacketProtocolIgnore]
		[JsonIgnore]
		public Vector2 Velocity {
			get { return this.velocity; }
			set { this.velocity = value; }
		}

		[PacketProtocolIgnore]
		[JsonIgnore]
		public int Width {
			get { return this.width; }
			set { this.width = value; }
		}

		[PacketProtocolIgnore]
		[JsonIgnore]
		public int Height {
			get { return this.height; }
			set { this.height = value; }
		}



		////////////////

		internal CustomEntityCore( CustomEntityCore copy ) {
			this.whoAmI = copy.whoAmI;
			this.DisplayName = copy.DisplayName;
			this.position = copy.position;
			this.velocity = copy.velocity;
			this.width = copy.width;
			this.height = copy.height;
			this.direction = copy.direction;
		}

		public CustomEntityCore( string display_name, int width, int height, Vector2 position, int direction ) {
			this.DisplayName = display_name;
			this.position = position;
			this.width = width;
			this.height = height;
			this.direction = direction;
		}


		////////////////

		public override string ToString() {
			return this.GetType().Name+" (who:"+this.whoAmI+", name:"+this.DisplayName+", pos:"+this.position+")";
		}
	}
}
