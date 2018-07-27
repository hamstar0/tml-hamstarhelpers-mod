using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.PlayerHelpers {
	public static class PlayerHeadDisplayHelpers {
		public static Color QuickAlpha( Color old_color, float alpha ) {
			Color result = old_color;
			result.R = (byte)((float)result.R * alpha);
			result.G = (byte)((float)result.G * alpha);
			result.B = (byte)((float)result.B * alpha);
			result.A = (byte)((float)result.A * alpha);
			return result;
		}


		public static void DrawPlayerHead( SpriteBatch sb, Player player, float x, float y, float alpha = 1f, float scale = 1f ) {
			PlayerHeadDrawInfo draw_info = new PlayerHeadDrawInfo {
				spriteBatch = sb,
				drawPlayer = player,
				alpha = alpha,
				scale = scale
			};

			int shader_id = 0;
			int skin_variant = player.skinVariant;
			short hair_dye = player.hairDye;
			if( player.head == 0 && hair_dye == 0 ) {
				hair_dye = 1;
			}
			draw_info.hairShader = hair_dye;

			if( player.face > 0 && player.face < 9 ) {
				PlayerHeadDisplayHelpers.LoadAccFace( (int)player.face );
			}
			if( player.dye[0] != null ) {
				shader_id = player.dye[0].dye;
			}
			draw_info.armorShader = shader_id;

			PlayerHeadDisplayHelpers.LoadHair( player.hair );

			Color color = PlayerHeadDisplayHelpers.QuickAlpha( Color.White, alpha );
			draw_info.eyeWhiteColor = color;
			Color color2 = PlayerHeadDisplayHelpers.QuickAlpha( player.eyeColor, alpha );
			draw_info.eyeColor = color2;
			Color color3 = PlayerHeadDisplayHelpers.QuickAlpha( player.GetHairColor( false ), alpha );
			draw_info.hairColor = color3;
			Color color4 = PlayerHeadDisplayHelpers.QuickAlpha( player.skinColor, alpha );
			draw_info.skinColor = color4;
			Color color5 = PlayerHeadDisplayHelpers.QuickAlpha( Color.White, alpha );
			draw_info.armorColor = color5;

			SpriteEffects sprite_effects = SpriteEffects.None;
			if( player.direction < 0 ) {
				sprite_effects = SpriteEffects.FlipHorizontally;
			}
			draw_info.spriteEffects = sprite_effects;

			Vector2 draw_origin = new Vector2( player.legFrame.Width * 0.5f, player.legFrame.Height * 0.4f );
			draw_info.drawOrigin = draw_origin;

			Vector2 position = player.position;
			Rectangle body_frame = player.bodyFrame;

			player.bodyFrame.Y = 0;
			player.position = Main.screenPosition;
			player.position.X = player.position.X + x;
			player.position.Y = player.position.Y + y;
			player.position.X = player.position.X - 6f;
			player.position.Y = player.position.Y - 4f;

			float head_offset = player.mount.PlayerHeadOffset;
			player.position.Y = player.position.Y - head_offset;

			if( player.head > 0 && player.head < 214 ) {
				PlayerHeadDisplayHelpers.LoadArmorHead( player.head );
			}
			if( player.face > 0 && player.face < 9 ) {
				PlayerHeadDisplayHelpers.LoadAccFace( player.face );
			}

			bool draw_hair = false;
			if( player.head == 10 || player.head == 12 || player.head == 28 || player.head == 62 || player.head == 97 || player.head == 106 || player.head == 113 || player.head == 116 || player.head == 119 || player.head == 133 || player.head == 138 || player.head == 139 || player.head == 163 || player.head == 178 || player.head == 181 || player.head == 191 || player.head == 198 ) {
				draw_hair = true;
			}
			bool draw_alt_hair = false;
			if( player.head == 161 || player.head == 14 || player.head == 15 || player.head == 16 || player.head == 18 || player.head == 21 || player.head == 24 || player.head == 25 || player.head == 26 || player.head == 40 || player.head == 44 || player.head == 51 || player.head == 56 || player.head == 59 || player.head == 60 || player.head == 67 || player.head == 68 || player.head == 69 || player.head == 114 || player.head == 121 || player.head == 126 || player.head == 130 || player.head == 136 || player.head == 140 || player.head == 145 || player.head == 158 || player.head == 159 || player.head == 184 || player.head == 190 || (double)player.head == 92.0 || player.head == 195 ) {
				draw_alt_hair = true;
			}
			ItemLoader.DrawHair( player, ref draw_hair, ref draw_alt_hair );
			draw_info.drawHair = draw_hair;
			draw_info.drawAltHair = draw_alt_hair;
			List<PlayerHeadLayer> draw_layers = Terraria.ModLoader.PlayerHooks.GetDrawHeadLayers( player );

			for( int i = 0; i < draw_layers.Count; i++ ) {
				if( draw_layers[i].ShouldDraw( draw_layers ) ) {
					PlayerHeadDisplayHelpers.DrawCompleteLayer( player, draw_layers[i], ref position, ref body_frame, ref draw_origin, ref draw_info,
						ref color, ref color2, ref color3, ref color4, ref color5, draw_hair, draw_alt_hair,
						shader_id, skin_variant, hair_dye, scale, sprite_effects );
				}
			}
			PlayerHeadDisplayHelpers.PostDrawLayer( player, ref position, ref body_frame );
		}


		private static void DrawCompleteLayer( Player player, PlayerHeadLayer draw_layer, ref Vector2 position, ref Rectangle body_frame, ref Vector2 draw_origin, ref PlayerHeadDrawInfo draw_info, ref Color color, ref Color color2, ref Color color3, ref Color color4, ref Color color5, bool draw_hair, bool draw_alt_hair, int shader_id, int skin_variant, short hair_dye, float scale, SpriteEffects sprite_effects ) {
			if( draw_layer == PlayerHeadLayer.Head ) {
				PlayerHeadDisplayHelpers.DrawHeadLayer( player, skin_variant, ref draw_origin, ref color4, ref color, ref color2, scale, sprite_effects );
			} else if( draw_layer == PlayerHeadLayer.Hair ) {
				if( draw_hair ) {
					PlayerHeadDisplayHelpers.DrawHairLayer( player, shader_id, hair_dye, ref color5, ref color3, ref draw_origin, scale, sprite_effects );
				}
			} else if( draw_layer == PlayerHeadLayer.AltHair ) {
				if( draw_alt_hair ) {
					PlayerHeadDisplayHelpers.DrawAltHairLayer( player, hair_dye, ref color3, ref draw_origin, scale, sprite_effects );
				}
			} else if( draw_layer == PlayerHeadLayer.Armor ) {
				PlayerHeadDisplayHelpers.DrawArmorLayer( player, shader_id, hair_dye, ref color3, ref color5, ref draw_origin, scale, sprite_effects );
			} else if( draw_layer == PlayerHeadLayer.FaceAcc ) {
				if( player.face > 0 ) {
					PlayerHeadDisplayHelpers.DrawFaceLayer( player, shader_id, ref color5, ref draw_origin, scale, sprite_effects );
				}
			} else {
				draw_layer.Draw( ref draw_info );
			}
		}


		private static void DrawHeadLayer( Player player, int skin_variant, ref Vector2 draw_origin, ref Color color1, ref Color color2, ref Color color3, float scale, SpriteEffects sprite_effects ) {
			if( player.head != 38 && player.head != 135 && ItemLoader.DrawHead( player ) ) {
				Texture2D p_tex1 = Main.playerTextures[skin_variant, 0];
				Texture2D p_tex2 = Main.playerTextures[skin_variant, 1];
				Texture2D p_tex3 = Main.playerTextures[skin_variant, 2];

				var pos = new Vector2( player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2),
										player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f
									 ) + player.headPosition + draw_origin;
				var rect = new Rectangle?( player.bodyFrame );

				Main.spriteBatch.Draw( p_tex1, pos, rect, color1, player.headRotation, draw_origin, scale, sprite_effects, 0f );
				Main.spriteBatch.Draw( p_tex2, pos, rect, color2, player.headRotation, draw_origin, scale, sprite_effects, 0f );
				Main.spriteBatch.Draw( p_tex3, pos, rect, color3, player.headRotation, draw_origin, scale, sprite_effects, 0f );
			}
		}


		private static void DrawHairLayer( Player player, int shader_id, short hair_dye, ref Color color1, ref Color color2, ref Vector2 draw_origin, float scale, SpriteEffects sprite_effects ) {
			Texture2D head_tex = Main.armorHeadTexture[player.head];
			var pos = new Vector2( player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2),
									player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f
								) + player.headPosition + draw_origin;

			DrawData value = new DrawData( head_tex, pos, new Rectangle?( player.bodyFrame ), color1, player.headRotation, draw_origin, scale, sprite_effects, 0 );

			GameShaders.Armor.Apply( shader_id, player, new DrawData?( value ) );
			value.Draw( Main.spriteBatch );
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();

			if( !player.invis ) {
				Rectangle body_frame_offset = player.bodyFrame;
				body_frame_offset.Y -= 336;
				if( body_frame_offset.Y < 0 ) {
					body_frame_offset.Y = 0;
				}

				Texture2D hair_tex = Main.playerHairTexture[player.hair];
				var hair_pos = new Vector2( player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2),
											player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f
										) + player.headPosition + draw_origin;
				var rect = new Rectangle?( body_frame_offset );

				value = new DrawData( hair_tex, hair_pos, rect, color2, player.headRotation, draw_origin, scale, sprite_effects, 0 );
				GameShaders.Hair.Apply( hair_dye, player, new DrawData?( value ) );
				value.Draw( Main.spriteBatch );
				Main.pixelShader.CurrentTechnique.Passes[0].Apply();
			}
		}


		private static void DrawAltHairLayer( Player player, short hair_dye, ref Color color, ref Vector2 draw_origin, float scale, SpriteEffects sprite_effects ) {
			Rectangle bodyFrame3 = player.bodyFrame;
			bodyFrame3.Y -= 336;
			if( bodyFrame3.Y < 0 ) {
				bodyFrame3.Y = 0;
			}
			if( !player.invis ) {
				Texture2D hair_alt_tex = Main.playerHairAltTexture[player.hair];
				var pos = new Vector2( player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2),
										player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f
									) + player.headPosition + draw_origin;

				DrawData draw = new DrawData( hair_alt_tex, pos, new Rectangle?( bodyFrame3 ), color, player.headRotation, draw_origin, scale, sprite_effects, 0 );

				GameShaders.Hair.Apply( hair_dye, player, new DrawData?( draw ) );
				draw.Draw( Main.spriteBatch );
				Main.pixelShader.CurrentTechnique.Passes[0].Apply();
			}
		}

		private static void DrawArmorLayer( Player player, int shader_id, short hair_dye, ref Color color1, ref Color color2, ref Vector2 draw_origin, float scale, SpriteEffects sprite_effects ) {
			if( player.head == 23 ) {
				Rectangle body_frame_offset = player.bodyFrame;
				body_frame_offset.Y -= 336;
				if( body_frame_offset.Y < 0 ) {
					body_frame_offset.Y = 0;
				}
				DrawData draw;
				if( !player.invis ) {
					Texture2D hair_tex = Main.playerHairTexture[player.hair];
					var hair_pos = new Vector2( player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2),
											player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f
										) + player.headPosition + draw_origin;

					draw = new DrawData( hair_tex, hair_pos, new Rectangle?( body_frame_offset ), color1, player.headRotation, draw_origin, scale, sprite_effects, 0 );

					GameShaders.Hair.Apply( hair_dye, player, new DrawData?( draw ) );
					draw.Draw( Main.spriteBatch );
					Main.pixelShader.CurrentTechnique.Passes[0].Apply();
				}

				Texture2D armor_head_tex = Main.armorHeadTexture[player.head];
				var armor_pos = new Vector2( player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2), player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f ) + player.headPosition + draw_origin;

				draw = new DrawData( armor_head_tex, armor_pos, new Rectangle?( player.bodyFrame ), color2, player.headRotation, draw_origin, scale, sprite_effects, 0 );

				GameShaders.Armor.Apply( shader_id, player, new DrawData?( draw ) );
				draw.Draw( Main.spriteBatch );
				Main.pixelShader.CurrentTechnique.Passes[0].Apply();
			} else if( player.head == 14 || player.head == 56 || player.head == 158 ) {
				Rectangle body_frame_offset = player.bodyFrame;
				if( player.head == 158 ) {
					body_frame_offset.Height -= 2;
				}
				int frame_y_offset = 0;
				if( body_frame_offset.Y == body_frame_offset.Height * 6 ) {
					body_frame_offset.Height -= 2;
				} else if( body_frame_offset.Y == body_frame_offset.Height * 7 ) {
					frame_y_offset = -2;
				} else if( body_frame_offset.Y == body_frame_offset.Height * 8 ) {
					frame_y_offset = -2;
				} else if( body_frame_offset.Y == body_frame_offset.Height * 9 ) {
					frame_y_offset = -2;
				} else if( body_frame_offset.Y == body_frame_offset.Height * 10 ) {
					frame_y_offset = -2;
				} else if( body_frame_offset.Y == body_frame_offset.Height * 13 ) {
					body_frame_offset.Height -= 2;
				} else if( body_frame_offset.Y == body_frame_offset.Height * 14 ) {
					frame_y_offset = -2;
				} else if( body_frame_offset.Y == body_frame_offset.Height * 15 ) {
					frame_y_offset = -2;
				} else if( body_frame_offset.Y == body_frame_offset.Height * 16 ) {
					frame_y_offset = -2;
				}
				body_frame_offset.Y += frame_y_offset;

				Texture2D armor_head_tex = Main.armorHeadTexture[player.head];
				var armor_pos = new Vector2( player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2), player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f + (float)frame_y_offset ) + player.headPosition + draw_origin;

				DrawData draw = new DrawData( armor_head_tex, armor_pos, new Rectangle?( body_frame_offset ), color2, player.headRotation, draw_origin, scale, sprite_effects, 0 );

				GameShaders.Armor.Apply( shader_id, player, new DrawData?( draw ) );
				draw.Draw( Main.spriteBatch );
				Main.pixelShader.CurrentTechnique.Passes[0].Apply();
			} else if( player.head > 0 && player.head < 214 && player.head != 28 ) {
				Texture2D armor_head_tex = Main.armorHeadTexture[player.head];
				var armor_pos = new Vector2( player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2),
												player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f
											) + player.headPosition + draw_origin;

				DrawData draw = new DrawData( armor_head_tex, armor_pos, new Rectangle?( player.bodyFrame ), color2, player.headRotation, draw_origin, scale, sprite_effects, 0 );

				GameShaders.Armor.Apply( shader_id, player, new DrawData?( draw ) );
				draw.Draw( Main.spriteBatch );
				Main.pixelShader.CurrentTechnique.Passes[0].Apply();
			} else {
				Rectangle body_frame_offset = player.bodyFrame;
				body_frame_offset.Y -= 336;
				if( body_frame_offset.Y < 0 ) {
					body_frame_offset.Y = 0;
				}

				Texture2D hair_tex = Main.playerHairTexture[player.hair];
				var hair_pos = new Vector2( player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2),
												player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f
											) + player.headPosition + draw_origin;

				DrawData draw = new DrawData( hair_tex, hair_pos, new Rectangle?( body_frame_offset ), color1, player.headRotation, draw_origin, scale, sprite_effects, 0 );

				GameShaders.Hair.Apply( hair_dye, player, new DrawData?( draw ) );
				draw.Draw( Main.spriteBatch );
				Main.pixelShader.CurrentTechnique.Passes[0].Apply();
			}
		}

		private static void DrawFaceLayer( Player player, int shader_id, ref Color color, ref Vector2 draw_origin, float scale, SpriteEffects sprite_effects ) {
			DrawData draw;
			Texture2D face_tex = Main.accFaceTexture[(int)player.face];
			var pos = new Vector2( (float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f)) ) + player.headPosition + draw_origin;

			if( player.face == 7 ) {
				draw = new DrawData( face_tex, pos, new Rectangle?( player.bodyFrame ), new Color( 200, 200, 200, 150 ), player.headRotation, draw_origin, scale, sprite_effects, 0 );
			} else {
				draw = new DrawData( face_tex, pos, new Rectangle?( player.bodyFrame ), color, player.headRotation, draw_origin, scale, sprite_effects, 0 );
			}

			GameShaders.Armor.Apply( shader_id, player, new DrawData?( draw ) );
			draw.Draw( Main.spriteBatch );
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();
		}

		private static void PostDrawLayer( Player player, ref Vector2 position, ref Rectangle body_frame ) {
			player.position = position;
			player.bodyFrame.Y = body_frame.Y;
		}


		private static void LoadAccFace( int i ) {
			if( !Main.accFaceLoaded[i] ) {
				Main.accFaceTexture[i] = Main.instance.OurLoad<Texture2D>( "Images/Acc_Face_" + i );
				Main.accFaceLoaded[i] = true;
			}
		}


		private static void LoadArmorHead( int i ) {
			if( !Main.armorHeadLoaded[i] ) {
				Main.armorHeadTexture[i] = Main.instance.OurLoad<Texture2D>( string.Concat( new object[] {
					"Images",
					Path.DirectorySeparatorChar,
					"Armor_Head_",
					i
				} ) );
				Main.armorHeadLoaded[i] = true;
			}
		}


		private static void LoadHair( int i ) {
			if( !Main.hairLoaded[i] ) {
				Main.playerHairTexture[i] = Main.instance.OurLoad<Texture2D>( string.Concat( new object[]
						{
							"Images",
							Path.DirectorySeparatorChar,
							"Player_Hair_",
							i + 1
						} ) );
				Main.playerHairAltTexture[i] = Main.instance.OurLoad<Texture2D>( string.Concat( new object[]
						{
							"Images",
							Path.DirectorySeparatorChar,
							"Player_HairAlt_",
							i + 1
						} ) );
				Main.hairLoaded[i] = true;
			}
		}
	}
}
