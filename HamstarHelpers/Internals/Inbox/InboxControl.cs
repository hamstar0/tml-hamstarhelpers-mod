﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Helpers.UI;
using HamstarHelpers.Services.Messages.Inbox;
using HamstarHelpers.Services.Hooks.LoadHooks;
using HamstarHelpers.Services.Timers;


namespace HamstarHelpers.Internals.Inbox {
	/// @private
	class InboxControl {
		private Texture2D ArrowLeft;
		private Texture2D ArrowRight;
		private Texture2D Icon;
		private Texture2D IconLit;
		
		private Vector2 IconAreaPos {
			get {
				return new Vector2( 
					ModHelpersConfig.Instance.InboxIconX < 0 ?
					Main.screenWidth + ModHelpersConfig.Instance.InboxIconX
					: ModHelpersConfig.Instance.InboxIconX,
					ModHelpersConfig.Instance.InboxIconY < 0 ?
					Main.screenHeight + ModHelpersConfig.Instance.InboxIconY
					: ModHelpersConfig.Instance.InboxIconY
				);
			}
		}
		private Vector2 IconPos {
			get {
				return this.IconAreaPos + new Vector2( 0f, 10f );
			}
		}
		private Vector2 IconMsgCountPos {
			get {
				return this.IconAreaPos + new Vector2( 11.5f, 17f );
			}
		}

		internal InboxMessages Messages = new InboxMessages();
		private int MessageScrollPos = 0;

		private bool IsIconClicked = false;
		private bool IsLeftArrowClicked = false;
		private bool IsRightArrowClicked = false;


		////////////////

		internal InboxControl() {
			if( Main.netMode == NetmodeID.Server ) { return; }

			var mymod = ModHelpersMod.Instance;

			this.Icon = mymod.GetTexture( "Internals/Inbox/MiniIcon" );
			this.IconLit = mymod.GetTexture( "Internals/Inbox/MiniIconLit2" );
			this.ArrowLeft = mymod.GetTexture( "Internals/Inbox/ArrowLeft" );
			this.ArrowRight = mymod.GetTexture( "Internals/Inbox/ArrowRight" );

			this.MessageScrollPos = this.Messages.Current;

			LoadHooks.AddWorldUnloadEachHook( this.OnWorldExit );
		}

		private void OnWorldExit() {
			this.Messages.OnWorldExit();
		}


		////////////////

		public bool ReadOldMessage( int pos ) {
			string msg;
			if( !InboxMessages.GetMessageAt(pos, out msg) || msg != null ) {
				Main.NewText( "Message "+(pos+1)+"/"+this.Messages.Current+": " + msg, Color.Gray );
				return true;
			}

			return false;
		}

		public void ReadLatestMessage() {
			string msg = InboxMessages.DequeueMessage();

			if( msg == null ) { return; }

			Main.NewText( "New message: " + msg, Color.LightYellow );
			this.MessageScrollPos = this.Messages.Current - 1;
		}


		////////////////

		internal void Draw( SpriteBatch sb ) {
			int unread = InboxMessages.CountUnreadMessages();

			var rect = new Rectangle( (int)this.IconPos.X, (int)this.IconPos.Y, this.Icon.Width, this.Icon.Height );
			bool isHover = UIHelpers.MouseInRectangle( rect );
			Vector2 mousePos = new Vector2( Main.mouseX + 16, Main.mouseY + 12 );

			if( Main.mouseLeft && isHover ) {
				if( !this.IsIconClicked ) {
					this.IsIconClicked = true;

					if( unread > 0 ) {
						Timers.RunNow( () => {
							Main.LocalPlayer.mouseInterface = true;

							this.ReadLatestMessage();
							unread--;
						} );
					}
				}
			} else {
				this.IsIconClicked = false;
			}
			
			if( unread <= 0 ) {
				if( Main.playerInventory ) {
					if( isHover ) {
						sb.DrawString( Main.fontMouseText, "No new messages", mousePos, Color.LightGray );
					}
					this.DrawIcon( sb );
				}
			} else {
				string msg = unread == 1 ? unread + " new message!" : unread + " new messages!";

				if( isHover ) {
					sb.DrawString( Main.fontMouseText, msg, mousePos, Color.White );
				}
				this.DrawIconLit( sb, unread );
			}
		}


		private void DrawArrows( SpriteBatch sb ) {
			bool hasLeft = this.Messages.Current == 1 || this.MessageScrollPos > 0;
			bool hasRight = this.MessageScrollPos < (this.Messages.Current - 1);
			
			if( hasLeft ) {
				var lArrowRect = new Rectangle( (int)this.IconAreaPos.X - 1, (int)this.IconAreaPos.Y - 2, this.ArrowLeft.Width, this.ArrowLeft.Height );

				if( Main.mouseLeft && UIHelpers.MouseInRectangle( lArrowRect ) ) {
					if( !this.IsLeftArrowClicked ) {
						this.IsLeftArrowClicked = true;

						Timers.RunNow( () => {
							Main.LocalPlayer.mouseInterface = true;

							if( this.ReadOldMessage( this.MessageScrollPos - 1 ) ) {
								this.MessageScrollPos--;
							} else {
								this.ReadOldMessage( this.MessageScrollPos );
							}
						} );
					}
				} else {
					this.IsLeftArrowClicked = false;
				}

				sb.Draw( this.ArrowLeft, lArrowRect, Color.White );
			}
			if( hasRight ) {
				var rArrowRect = new Rectangle( (int)( ( this.IconAreaPos.X + this.Icon.Width + 1 ) - this.ArrowRight.Width ),
					(int)this.IconAreaPos.Y - 2, this.ArrowRight.Width, this.ArrowRight.Height );

				if( Main.mouseLeft && UIHelpers.MouseInRectangle( rArrowRect ) ) {
					if( !this.IsRightArrowClicked ) {
						this.IsRightArrowClicked = true;

						Timers.RunNow( () => {
							Main.LocalPlayer.mouseInterface = true;

							if( this.ReadOldMessage( this.MessageScrollPos + 1 ) ) {
								this.MessageScrollPos++;
							}
						} );
					}
				} else {
					this.IsRightArrowClicked = false;
				}

				sb.Draw( this.ArrowRight, rArrowRect, Color.White );
			}
		}


		////

		private void DrawIcon( SpriteBatch sb ) {
			var iconRect = new Rectangle( (int)this.IconPos.X, (int)this.IconPos.Y, this.Icon.Width, this.Icon.Height );

			this.DrawArrows( sb );

			sb.Draw( this.Icon, iconRect, Color.White );
		}

		private void DrawIconLit( SpriteBatch sb, int unreadMsgCount ) {
			var iconRect = new Rectangle( (int)this.IconPos.X, (int)this.IconPos.Y, this.IconLit.Width, this.IconLit.Height );
			var numPos = unreadMsgCount == 1 ? this.IconMsgCountPos + new Vector2(1.5f,0f) : this.IconMsgCountPos;

			this.DrawArrows( sb );

			sb.Draw( this.IconLit, iconRect, Color.White );
			sb.DrawString( Main.fontMouseText, unreadMsgCount + "", numPos, Color.White, 0f, default(Vector2), 0.5f, SpriteEffects.None, 1f );
		}
	}
}
