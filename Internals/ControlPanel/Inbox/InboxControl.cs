using HamstarHelpers.Services.Messages;
using HamstarHelpers.Services.Promises;
using HamstarHelpers.TmlHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;


namespace HamstarHelpers.Internals.ControlPanel.Inbox {
	class InboxControl {
		private Texture2D ArrowLeft;
		private Texture2D ArrowRight;
		private Texture2D Icon;
		private Texture2D IconLit;
		
		private Vector2 IconAreaPos {
			get {
				var config = HamstarHelpersMod.Instance.Config;
				return new Vector2( config.InboxIconPosX, config.InboxIconPosY );
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
			if( Main.netMode == 2 ) { return; }

			var mymod = HamstarHelpersMod.Instance;

			this.Icon = mymod.GetTexture( "Internals/ControlPanel/Inbox/MiniIcon" );
			this.IconLit = mymod.GetTexture( "Internals/ControlPanel/Inbox/MiniIconLit2" );
			this.ArrowLeft = mymod.GetTexture( "Internals/ControlPanel/Inbox/ArrowLeft" );
			this.ArrowRight = mymod.GetTexture( "Internals/ControlPanel/Inbox/ArrowRight" );

			this.MessageScrollPos = this.Messages.Current;

			Promises.AddWorldUnloadEachPromise( this.OnWorldExit );
		}

		private void OnWorldExit() {
			this.Messages.OnWorldExit();
		}


		////////////////

		public bool ReadOldMessage( int pos ) {
			bool is_unread;
			string msg = InboxMessages.GetMessageAt( pos, out is_unread );

			if( msg != null && !is_unread ) {
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
			bool is_hover = UIHelpers.UIHelpers.MouseInRectangle( rect );
			Vector2 mouse_pos = new Vector2( Main.mouseX + 16, Main.mouseY );

			if( Main.mouseLeft && is_hover ) {
				if( !this.IsIconClicked ) {
					this.IsIconClicked = true;

					if( unread > 0 ) {
						this.ReadLatestMessage();
						unread--;
					}
				}
			} else {
				this.IsIconClicked = false;
			}
			
			if( unread <= 0 ) {
				if( Main.playerInventory ) {
					if( is_hover ) {
						sb.DrawString( Main.fontMouseText, "No new messages", mouse_pos, Color.LightGray );
					}
					this.DrawIcon( sb );
				}
			} else {
				string msg = unread == 1 ? unread + " new message!" : unread + " new messages!";

				if( is_hover ) {
					sb.DrawString( Main.fontMouseText, msg, mouse_pos, Color.White );
				}
				this.DrawIconLit( sb, unread );
			}
		}


		private void DrawArrows( SpriteBatch sb ) {
			bool has_left = this.Messages.Current == 1 || this.MessageScrollPos > 0;
			bool has_right = this.MessageScrollPos < (this.Messages.Current - 1);
			
			if( has_left ) {
				var l_arrow_rect = new Rectangle( (int)this.IconAreaPos.X - 1, (int)this.IconAreaPos.Y, this.ArrowLeft.Width, this.ArrowLeft.Height );

				if( Main.mouseLeft && UIHelpers.UIHelpers.MouseInRectangle( l_arrow_rect ) ) {
					if( !this.IsLeftArrowClicked ) {
						this.IsLeftArrowClicked = true;
						if( this.ReadOldMessage( this.MessageScrollPos - 1 ) ) {
							this.MessageScrollPos--;
						} else {
							this.ReadOldMessage( this.MessageScrollPos );
						}
					}
				} else {
					this.IsLeftArrowClicked = false;
				}

				sb.Draw( this.ArrowLeft, l_arrow_rect, Color.White );
			}
			if( has_right ) {
				var r_arrow_rect = new Rectangle( (int)( ( this.IconAreaPos.X + this.Icon.Width + 1 ) - this.ArrowRight.Width ),
					(int)this.IconAreaPos.Y, this.ArrowRight.Width, this.ArrowRight.Height );

				if( Main.mouseLeft && UIHelpers.UIHelpers.MouseInRectangle( r_arrow_rect ) ) {
					if( !this.IsRightArrowClicked ) {
						this.IsRightArrowClicked = true;
						if( this.ReadOldMessage( this.MessageScrollPos + 1 ) ) {
							this.MessageScrollPos++;
						}
					}
				} else {
					this.IsRightArrowClicked = false;
				}

				sb.Draw( this.ArrowRight, r_arrow_rect, Color.White );
			}
		}


		private void DrawIcon( SpriteBatch sb ) {
			var icon_rect = new Rectangle( (int)this.IconPos.X, (int)this.IconPos.Y, this.Icon.Width, this.Icon.Height );

			this.DrawArrows( sb );

			sb.Draw( this.Icon, icon_rect, Color.White );
		}

		private void DrawIconLit( SpriteBatch sb, int unread_msg_count ) {
			var icon_rect = new Rectangle( (int)this.IconPos.X, (int)this.IconPos.Y, this.IconLit.Width, this.IconLit.Height );
			var num_pos = unread_msg_count == 1 ? this.IconMsgCountPos + new Vector2(1.5f,0f) : this.IconMsgCountPos;

			this.DrawArrows( sb );

			sb.Draw( this.IconLit, icon_rect, Color.White );
			sb.DrawString( Main.fontMouseText, unread_msg_count + "", num_pos, Color.White, 0f, default(Vector2), 0.5f, SpriteEffects.None, 1f );
		}
	}
}
