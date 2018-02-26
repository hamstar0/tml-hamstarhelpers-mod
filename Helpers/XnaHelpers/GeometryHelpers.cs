using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Helpers.XnaHelpers {
	/*public class Polygon {
		public IList<Line> Lines { get; private set; }
		private Rectangle? Bounds = null;

		public bool IsClosed = false;



		////////////////

		public Polygon() {
			this.Lines = new List<Line>();
		}
		public Polygon( IList<Line> lines ) {
			var first = lines.First();
			var last = lines.Last();

			this.Lines = lines;
			this.IsClosed = first.Start.X == last.End.X && first.Start.Y == last.End.Y;
		}

		////////////////

		public void AddLine( Vector2 point ) {
			Line last_line = this.Lines.Last();
			Vector2 start = last_line == null ? default( Vector2 ) : last_line.End;
			var line = new Line( start, point );

			this.Lines.Add( line );
		}

		public void AddLine( Line line ) {
			this.Lines.Add( line );
		}


		////////////////

		public Rectangle GetBounds() {
			if( this.Bounds != null ) {
				return (Rectangle)this.Bounds;
			}

			var top_left_most = new Vector2( Single.MaxValue, Single.MaxValue );
			var bottom_right_most = new Vector2( 0, 0 );

			this.Bounds = default( Rectangle );

			for( int i=0; i<this.Lines.Count; i++ ) {
				Line line = this.Lines[i];

				if( line.Start.X <= top_left_most.X && line.Start.Y <= top_left_most.Y ) {
					top_left_most = line.Start;
				} else if( line.Start.X >= bottom_right_most.X && line.Start.Y >= bottom_right_most.Y ) {
					bottom_right_most = line.Start;
				}
			}

			this.Bounds = new Rectangle( (int)top_left_most.X, (int)top_left_most.Y,
				(int)Math.Ceiling(top_left_most.X + bottom_right_most.X),
				(int)Math.Ceiling( top_left_most.Y = bottom_right_most.Y)
			);
			return (Rectangle)this.Bounds;
		}


		////////////////

		public bool ContainsPoint( Point point ) {
			return this.ContainsPoint( point.X, point.Y );
		}

		/// <summary>
		/// Point in polygon check.
		/// </summary>
		/// <param name="point">The point.</param>
		/// <param name="polygon">The polygon.</param>
		/// <returns>True if point is inside, false otherwise.</returns>
		/// <see cref="http://local.wasp.uwa.edu.au/~pbourke/geometry/insidepoly/"/>
		public bool ContainsPoint( int x, int y ) {
			bool inside = false;

			foreach( var side in this.Lines ) {
				// Below top left-most point of poly line
				if( y > Math.Min( side.Start.Y, side.End.Y ) ) {
					// Above bottom right-most point of poly line
					if( y <= Math.Max( side.Start.Y, side.End.Y ) ) {
						// Left of right-most point of poly line
						if( x <= Math.Max( side.Start.X, side.End.X ) ) {
							float y_rel_side_start_offset = y - side.Start.Y;
							float x_side_end_offset = side.End.X - side.Start.X;
							float y_side_end_offset = side.End.Y - side.Start.Y;
							float x_intersection = side.Start.X + ( y_rel_side_start_offset / y_side_end_offset ) * x_side_end_offset;

							if( x <= x_intersection ) {
								inside = !inside;
							}
						}
					}
				}
			}

			return inside;
		}
	}




	public class Line {
		public Vector2 Start;
		public Vector2 End;

		public Line( Vector2 start, Vector2 end ) {
			this.Start = start;
			this.End = end;
		}
	}*/
}

