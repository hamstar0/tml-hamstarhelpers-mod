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
		public static Color QuickAlpha( Color oldColor, float alpha ) {
			Color result = oldColor;
			result.R = (byte)((float)result.R * alpha);
			result.G = (byte)((float)result.G * alpha);
			result.B = (byte)((float)result.B * alpha);
			result.A = (byte)((float)result.A * alpha);
			return result;
		}


		public static void DrawPlayerHead( SpriteBatch sb, Player player, float x, float y, float alpha = 1f, float scale = 1f ) {
			PlayerHeadDrawInfo drawInfo = new PlayerHeadDrawInfo {
				spriteBatch = sb,
				drawPlayer = player,
				alpha = alpha,
				scale = scale
			};

			int shaderId = 0;
			int skinVariant = player.skinVariant;
			short hairDye = player.hairDye;
			if( player.head == 0 && hairDye == 0 ) {
				hairDye = 1;
			}
			drawInfo.hairShader = hairDye;

			if( player.face > 0 && player.face < 9 ) {
				PlayerHeadDisplayHelpers.LoadAccFace( (int)player.face );
			}
			if( player.dye[0] != null ) {
				shaderId = player.dye[0].dye;
			}
			drawInfo.armorShader = shaderId;

			PlayerHeadDisplayHelpers.LoadHair( player.hair );

			Color color = PlayerHeadDisplayHelpers.QuickAlpha( Color.White, alpha );
			drawInfo.eyeWhiteColor = color;
			Color color2 = PlayerHeadDisplayHelpers.QuickAlpha( player.eyeColor, alpha );
			drawInfo.eyeColor = color2;
			Color color3 = PlayerHeadDisplayHelpers.QuickAlpha( player.GetHairColor( false ), alpha );
			drawInfo.hairColor = color3;
			Color color4 = PlayerHeadDisplayHelpers.QuickAlpha( player.skinColor, alpha );
			drawInfo.skinColor = color4;
			Color color5 = PlayerHeadDisplayHelpers.QuickAlpha( Color.White, alpha );
			drawInfo.armorColor = color5;

			SpriteEffects spriteEffects = SpriteEffects.None;
			if( player.direction < 0 ) {
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
			drawInfo.spriteEffects = spriteEffects;

			Vector2 drawOrigin = new Vector2( player.legFrame.Width * 0.5f, player.legFrame.Height * 0.4f );
			drawInfo.drawOrigin = drawOrigin;

			Vector2 position = player.position;
			Rectangle bodyFrame = player.bodyFrame;

			player.bodyFrame.Y = 0;
			player.position = Main.screenPosition;
			player.position.X = player.position.X + x;
			player.position.Y = player.position.Y + y;
			player.position.X = player.position.X - 6f;
			player.position.Y = player.position.Y - 4f;

			float headOffset = player.mount.PlayerHeadOffset;
			player.position.Y = player.position.Y - headOffset;

			if( player.head > 0 && player.head < 214 ) {
				PlayerHeadDisplayHelpers.LoadArmorHead( player.head );
			}
			if( player.face > 0 && player.face < 9 ) {
				PlayerHeadDisplayHelpers.LoadAccFace( player.face );
			}

			bool drawHair = false;
			if( player.head == 10 || player.head == 12 || player.head == 28 || player.head == 62 || player.head == 97 || player.head == 106 || player.head == 113 || player.head == 116 || player.head == 119 || player.head == 133 || player.head == 138 || player.head == 139 || player.head == 163 || player.head == 178 || player.head == 181 || player.head == 191 || player.head == 198 ) {
				drawHair = true;
			}
			bool drawAltHair = false;
			if( player.head == 161 || player.head == 14 || player.head == 15 || player.head == 16 || player.head == 18 || player.head == 21 || player.head == 24 || player.head == 25 || player.head == 26 || player.head == 40 || player.head == 44 || player.head == 51 || player.head == 56 || player.head == 59 || player.head == 60 || player.head == 67 || player.head == 68 || player.head == 69 || player.head == 114 || player.head == 121 || player.head == 126 || player.head == 130 || player.head == 136 || player.head == 140 || player.head == 145 || player.head == 158 || player.head == 159 || player.head == 184 || player.head == 190 || (double)player.head == 92.0 || player.head == 195 ) {
				drawAltHair = true;
			}
			ItemLoader.DrawHair( player, ref drawHair, ref drawAltHair );
			drawInfo.drawHair = drawHair;
			drawInfo.drawAltHair = drawAltHair;
			List<PlayerHeadLayer> drawLayers = Terraria.ModLoader.PlayerHooks.GetDrawHeadLayers( player );

			for( int i = 0; i < drawLayers.Count; i++ ) {
				if( drawLayers[i].ShouldDraw( drawLayers ) ) {
					PlayerHeadDisplayHelpers.DrawCompleteLayer( player, drawLayers[i], ref position, ref bodyFrame, ref drawOrigin, ref drawInfo,
						ref color, ref color2, ref color3, ref color4, ref color5, drawHair, drawAltHair,
						shaderId, skinVariant, hairDye, scale, spriteEffects );
				}
			}
			PlayerHeadDisplayHelpers.PostDrawLayer( player, ref position, ref bodyFrame );
		}


		private static void DrawCompleteLayer( Player player, PlayerHeadLayer drawLayer, ref Vector2 position, ref Rectangle bodyFrame, ref Vector2 drawOrigin, ref PlayerHeadDrawInfo drawInfo, ref Color color, ref Color color2, ref Color color3, ref Color color4, ref Color color5, bool drawHair, bool drawAltHair, int shaderId, int skinVariant, short hairDye, float scale, SpriteEffects spriteEffects ) {
			if( drawLayer == PlayerHeadLayer.Head ) {
				PlayerHeadDisplayHelpers.DrawHeadLayer( player, skinVariant, ref drawOrigin, ref color4, ref color, ref color2, scale, spriteEffects );
			} else if( drawLayer == PlayerHeadLayer.Hair ) {
				if( drawHair ) {
					PlayerHeadDisplayHelpers.DrawHairLayer( player, shaderId, hairDye, ref color5, ref color3, ref drawOrigin, scale, spriteEffects );
				}
			} else if( drawLayer == PlayerHeadLayer.AltHair ) {
				if( drawAltHair ) {
					PlayerHeadDisplayHelpers.DrawAltHairLayer( player, hairDye, ref color3, ref drawOrigin, scale, spriteEffects );
				}
			} else if( drawLayer == PlayerHeadLayer.Armor ) {
				PlayerHeadDisplayHelpers.DrawArmorLayer( player, shaderId, hairDye, ref color3, ref color5, ref drawOrigin, scale, spriteEffects );
			} else if( drawLayer == PlayerHeadLayer.FaceAcc ) {
				if( player.face > 0 ) {
					PlayerHeadDisplayHelpers.DrawFaceLayer( player, shaderId, ref color5, ref drawOrigin, scale, spriteEffects );
				}
			} else {
				drawLayer.Draw( ref drawInfo );
			}
		}


		private static void DrawHeadLayer( Player player, int skinVariant, ref Vector2 drawOrigin, ref Color color1, ref Color color2, ref Color color3, float scale, SpriteEffects spriteEffects ) {
			if( player.head != 38 && player.head != 135 && ItemLoader.DrawHead( player ) ) {
				Texture2D pTex1 = Main.playerTextures[skinVariant, 0];
				Texture2D pTex2 = Main.playerTextures[skinVariant, 1];
				Texture2D pTex3 = Main.playerTextures[skinVariant, 2];

				var pos = new Vector2( player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2),
										player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f
									 ) + player.headPosition + drawOrigin;
				var rect = new Rectangle?( player.bodyFrame );

				Main.spriteBatch.Draw( pTex1, pos, rect, color1, player.headRotation, drawOrigin, scale, spriteEffects, 0f );
				Main.spriteBatch.Draw( pTex2, pos, rect, color2, player.headRotation, drawOrigin, scale, spriteEffects, 0f );
				Main.spriteBatch.Draw( pTex3, pos, rect, color3, player.headRotation, drawOrigin, scale, spriteEffects, 0f );
			}
		}


		private static void DrawHairLayer( Player player, int shaderId, short hairDye, ref Color color1, ref Color color2, ref Vector2 drawOrigin, float scale, SpriteEffects spriteEffects ) {
			Texture2D headTex = Main.armorHeadTexture[player.head];
			var pos = new Vector2( player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2),
									player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f
								) + player.headPosition + drawOrigin;

			DrawData value = new DrawData( headTex, pos, new Rectangle?( player.bodyFrame ), color1, player.headRotation, drawOrigin, scale, spriteEffects, 0 );

			GameShaders.Armor.Apply( shaderId, player, new DrawData?( value ) );
			value.Draw( Main.spriteBatch );
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();

			if( !player.invis ) {
				Rectangle bodyFrameOffset = player.bodyFrame;
				bodyFrameOffset.Y -= 336;
				if( bodyFrameOffset.Y < 0 ) {
					bodyFrameOffset.Y = 0;
				}

				Texture2D hairTex = Main.playerHairTexture[player.hair];
				var hairPos = new Vector2( player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2),
											player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f
										) + player.headPosition + drawOrigin;
				var rect = new Rectangle?( bodyFrameOffset );

				value = new DrawData( hairTex, hairPos, rect, color2, player.headRotation, drawOrigin, scale, spriteEffects, 0 );
				GameShaders.Hair.Apply( hairDye, player, new DrawData?( value ) );
				value.Draw( Main.spriteBatch );
				Main.pixelShader.CurrentTechnique.Passes[0].Apply();
			}
		}


		private static void DrawAltHairLayer( Player player, short hairDye, ref Color color, ref Vector2 drawOrigin, float scale, SpriteEffects spriteEffects ) {
			Rectangle bodyFrame3 = player.bodyFrame;
			bodyFrame3.Y -= 336;
			if( bodyFrame3.Y < 0 ) {
				bodyFrame3.Y = 0;
			}
			if( !player.invis ) {
				Texture2D hairAltTex = Main.playerHairAltTexture[player.hair];
				var pos = new Vector2( player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2),
										player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f
									) + player.headPosition + drawOrigin;

				DrawData draw = new DrawData( hairAltTex, pos, new Rectangle?( bodyFrame3 ), color, player.headRotation, drawOrigin, scale, spriteEffects, 0 );

				GameShaders.Hair.Apply( hairDye, player, new DrawData?( draw ) );
				draw.Draw( Main.spriteBatch );
				Main.pixelShader.CurrentTechnique.Passes[0].Apply();
			}
		}

		private static void DrawArmorLayer( Player player, int shaderId, short hairDye, ref Color color1, ref Color color2, ref Vector2 drawOrigin, float scale, SpriteEffects spriteEffects ) {
			if( player.head == 23 ) {
				Rectangle bodyFrameOffset = player.bodyFrame;
				bodyFrameOffset.Y -= 336;
				if( bodyFrameOffset.Y < 0 ) {
					bodyFrameOffset.Y = 0;
				}
				DrawData draw;
				if( !player.invis ) {
					Texture2D hairTex = Main.playerHairTexture[player.hair];
					var hairPos = new Vector2( player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2),
											player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f
										) + player.headPosition + drawOrigin;

					draw = new DrawData( hairTex, hairPos, new Rectangle?( bodyFrameOffset ), color1, player.headRotation, drawOrigin, scale, spriteEffects, 0 );

					GameShaders.Hair.Apply( hairDye, player, new DrawData?( draw ) );
					draw.Draw( Main.spriteBatch );
					Main.pixelShader.CurrentTechnique.Passes[0].Apply();
				}

				Texture2D armorHeadTex = Main.armorHeadTexture[player.head];
				var armorPos = new Vector2( player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2), player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f ) + player.headPosition + drawOrigin;

				draw = new DrawData( armorHeadTex, armorPos, new Rectangle?( player.bodyFrame ), color2, player.headRotation, drawOrigin, scale, spriteEffects, 0 );

				GameShaders.Armor.Apply( shaderId, player, new DrawData?( draw ) );
				draw.Draw( Main.spriteBatch );
				Main.pixelShader.CurrentTechnique.Passes[0].Apply();
			} else if( player.head == 14 || player.head == 56 || player.head == 158 ) {
				Rectangle bodyFrameOffset = player.bodyFrame;
				if( player.head == 158 ) {
					bodyFrameOffset.Height -= 2;
				}
				int frameYOffset = 0;
				if( bodyFrameOffset.Y == bodyFrameOffset.Height * 6 ) {
					bodyFrameOffset.Height -= 2;
				} else if( bodyFrameOffset.Y == bodyFrameOffset.Height * 7 ) {
					frameYOffset = -2;
				} else if( bodyFrameOffset.Y == bodyFrameOffset.Height * 8 ) {
					frameYOffset = -2;
				} else if( bodyFrameOffset.Y == bodyFrameOffset.Height * 9 ) {
					frameYOffset = -2;
				} else if( bodyFrameOffset.Y == bodyFrameOffset.Height * 10 ) {
					frameYOffset = -2;
				} else if( bodyFrameOffset.Y == bodyFrameOffset.Height * 13 ) {
					bodyFrameOffset.Height -= 2;
				} else if( bodyFrameOffset.Y == bodyFrameOffset.Height * 14 ) {
					frameYOffset = -2;
				} else if( bodyFrameOffset.Y == bodyFrameOffset.Height * 15 ) {
					frameYOffset = -2;
				} else if( bodyFrameOffset.Y == bodyFrameOffset.Height * 16 ) {
					frameYOffset = -2;
				}
				bodyFrameOffset.Y += frameYOffset;

				Texture2D armorHeadTex = Main.armorHeadTexture[player.head];
				var armorPos = new Vector2( player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2), player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f + (float)frameYOffset ) + player.headPosition + drawOrigin;

				DrawData draw = new DrawData( armorHeadTex, armorPos, new Rectangle?( bodyFrameOffset ), color2, player.headRotation, drawOrigin, scale, spriteEffects, 0 );

				GameShaders.Armor.Apply( shaderId, player, new DrawData?( draw ) );
				draw.Draw( Main.spriteBatch );
				Main.pixelShader.CurrentTechnique.Passes[0].Apply();
			} else if( player.head > 0 && player.head < 214 && player.head != 28 ) {
				Texture2D armorHeadTex = Main.armorHeadTexture[player.head];
				var armorPos = new Vector2( player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2),
												player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f
											) + player.headPosition + drawOrigin;

				DrawData draw = new DrawData( armorHeadTex, armorPos, new Rectangle?( player.bodyFrame ), color2, player.headRotation, drawOrigin, scale, spriteEffects, 0 );

				GameShaders.Armor.Apply( shaderId, player, new DrawData?( draw ) );
				draw.Draw( Main.spriteBatch );
				Main.pixelShader.CurrentTechnique.Passes[0].Apply();
			} else {
				Rectangle bodyFrameOffset = player.bodyFrame;
				bodyFrameOffset.Y -= 336;
				if( bodyFrameOffset.Y < 0 ) {
					bodyFrameOffset.Y = 0;
				}

				Texture2D hairTex = Main.playerHairTexture[player.hair];
				var hairPos = new Vector2( player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2),
												player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f
											) + player.headPosition + drawOrigin;

				DrawData draw = new DrawData( hairTex, hairPos, new Rectangle?( bodyFrameOffset ), color1, player.headRotation, drawOrigin, scale, spriteEffects, 0 );

				GameShaders.Hair.Apply( hairDye, player, new DrawData?( draw ) );
				draw.Draw( Main.spriteBatch );
				Main.pixelShader.CurrentTechnique.Passes[0].Apply();
			}
		}

		private static void DrawFaceLayer( Player player, int shaderId, ref Color color, ref Vector2 drawOrigin, float scale, SpriteEffects spriteEffects ) {
			DrawData draw;
			Texture2D faceTex = Main.accFaceTexture[(int)player.face];
			var pos = new Vector2( (float)((int)(player.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(player.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f)) ) + player.headPosition + drawOrigin;

			if( player.face == 7 ) {
				draw = new DrawData( faceTex, pos, new Rectangle?( player.bodyFrame ), new Color( 200, 200, 200, 150 ), player.headRotation, drawOrigin, scale, spriteEffects, 0 );
			} else {
				draw = new DrawData( faceTex, pos, new Rectangle?( player.bodyFrame ), color, player.headRotation, drawOrigin, scale, spriteEffects, 0 );
			}

			GameShaders.Armor.Apply( shaderId, player, new DrawData?( draw ) );
			draw.Draw( Main.spriteBatch );
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();
		}

		private static void PostDrawLayer( Player player, ref Vector2 position, ref Rectangle bodyFrame ) {
			player.position = position;
			player.bodyFrame.Y = bodyFrame.Y;
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
