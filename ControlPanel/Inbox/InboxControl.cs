using HamstarHelpers.Utilities.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;


namespace HamstarHelpers.ControlPanel.Inbox {
	class InboxControl {
		private Texture2D ArrowLeft;
		private Texture2D ArrowRight;
		private Texture2D Icon;
		private Texture2D IconLit;

		private Vector2 IconPos;
		private Vector2 MessageCountPos;

		internal InboxMessages Messages = new InboxMessages();
		private int MessageAt = 0;


		////////////////

		internal InboxControl() {
			if( Main.netMode == 2 ) { return; }

			var mymod = HamstarHelpersMod.Instance;

			this.Icon = mymod.GetTexture( "ControlPanel/Inbox/MiniIcon" );
			this.IconLit = mymod.GetTexture( "ControlPanel/Inbox/MiniIconLit2" );
			this.ArrowLeft = mymod.GetTexture( "ControlPanel/Inbox/ArrowLeft" );
			this.ArrowRight = mymod.GetTexture( "ControlPanel/Inbox/ArrowRight" );

			this.IconPos = new Vector2( 2, 80 );
			this.MessageCountPos = new Vector2( 18, 96 );

			this.MessageAt = this.Messages.Current;
		}


		////////////////

		public void ReadMessageAt() {
			bool _;
			string msg = InboxMessages.GetMessageAt( this.MessageAt, out _ );

			if( msg != null ) {
				Main.NewText( msg );
			}
		}

		public void ReadLatestMessage() {
			string msg = InboxMessages.DequeueMessage();
			if( msg != null ) {
				Main.NewText( msg );
			}
		}


		////////////////

		internal void Draw( SpriteBatch sb ) {
			int unread = InboxMessages.CountUnreadMessages();
			var rect = new Rectangle( (int)this.IconPos.X, (int)this.IconPos.Y, this.Icon.Width, this.Icon.Height );

			if( unread > 0 && Main.mouseLeft ) {
				if( UIHelpers.UIHelpers.MouseInRectangle(rect) ) {
					this.ReadLatestMessage();
					unread--;
				}
			}

			if( unread <= 0 ) {
				if( Main.playerInventory ) {
					this.DrawIcon( sb );
				}
			} else {
				rect.Width = this.IconLit.Width;
				rect.Height = this.IconLit.Height;

				this.DrawIconLit( sb, unread );
			}
		}


		private void DrawArrows( SpriteBatch sb ) {
			bool has_left = this.MessageAt > 0;
			bool has_right = this.MessageAt < this.Messages.Current;
			
			if( has_left ) {
				var l_arrow_rect = new Rectangle( (int)this.IconPos.X - 1, (int)this.IconPos.Y, this.ArrowLeft.Width, this.ArrowLeft.Height );

				if( UIHelpers.UIHelpers.MouseInRectangle( l_arrow_rect ) ) {
					this.MessageAt--;
					this.ReadMessageAt();
				}

				sb.Draw( this.ArrowLeft, l_arrow_rect, Color.White );
			}
			if( has_right ) {
				var r_arrow_rect = new Rectangle( (int)( ( this.IconPos.X + this.Icon.Width + 1 ) - this.ArrowRight.Width ),
					(int)this.IconPos.Y, this.ArrowRight.Width, this.ArrowRight.Height );

				if( UIHelpers.UIHelpers.MouseInRectangle( r_arrow_rect ) ) {
					this.MessageAt++;
					this.ReadMessageAt();
				}

				sb.Draw( this.ArrowRight, r_arrow_rect, Color.White );
			}
		}


		private void DrawIcon( SpriteBatch sb ) {
			var icon_rect = new Rectangle( (int)this.IconPos.X, (int)this.IconPos.Y + 10, this.Icon.Width, this.Icon.Height );

			this.DrawArrows( sb );

			sb.Draw( this.Icon, icon_rect, Color.White );
		}

		private void DrawIconLit( SpriteBatch sb, int unread_msg_count ) {
			var icon_rect = new Rectangle( (int)this.IconPos.X, (int)this.IconPos.Y + 10, this.IconLit.Width, this.IconLit.Height );

			this.DrawArrows( sb );

			sb.Draw( this.IconLit, icon_rect, Color.White );
			sb.DrawString( Main.fontMouseText, unread_msg_count + "", this.MessageCountPos, Color.White );
		}
	}
}
