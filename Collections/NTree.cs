// SPDX-FileCopyrightText: 2023 Admer Šuko
// SPDX-License-Identifier: MIT

namespace Elegy.Collections
{
	/// <summary>
	/// N-dimensional spatial tree that stores <typeparamref name="TItem"/>s
	/// inside a <typeparamref name="TBound"/>.
	/// </summary>
	/// <typeparam name="TBound">
	/// For example: <seealso cref="Aabb"/> or <seealso cref="Rect2"/>.
	/// </typeparam>
	/// <typeparam name="TItem">
	/// For example: <seealso cref="Vector3"/> or anything, really!
	/// </typeparam>
	public class NTree<TBound, TItem> where TBound : struct
	{
		#region Private fields
		private int mDimensions;
		private TBound mBound;
		private List<TItem> mItems;
		private List<NTreeNode<TBound, TItem>> mNodes;
		private List<NTreeNode<TBound, TItem>> mLeaves;
		#endregion

		#region Delegates
		/// <summary>
		/// Does the item intersect the bound?
		/// </summary>
		public delegate bool IntersectsBoundDelegate( TItem item, TBound bound );

		/// <summary>
		/// With these elements loaded, should this node subdivide any further?
		/// </summary>
		public delegate bool ShouldSubdivideDelegate( NTreeNode<TBound, TItem> node );

		/// <summary>
		/// Get a subdivided bounding volume for the Nth child node
		/// </summary>
		public delegate TBound GetSubdividedVolumeForChildDelegate( TBound bound, int childIndex );

		/// <summary>
		/// If two or more intersections occur, resolve them. This gets called for any number of intersections actually.
		/// </summary>
		/// <returns>True on successful resolution, false if there were no usable intersections at all.</returns>
		public delegate bool ResolveIntersectionsDelegate( TBound[] bounds, TItem item, out int[] hits );
		#endregion

		#region Properties
		/// <summary>
		/// 
		/// </summary>
		public int Combinations => 1 << Dimensions;

		/// <summary>
		/// Dimensionality of this tree (2 or 3 usually).
		/// </summary>
		public int Dimensions => mDimensions;

		/// <summary>
		/// Bounding volume of this tree.
		/// </summary>
		public TBound Bound => mBound;

		/// <summary>
		/// This tree's items.
		/// </summary>
		public IReadOnlyList<TItem> Items => mItems;

		/// <summary>
		/// All of the nodes belonging to this tree.
		/// </summary>
		public IReadOnlyList<NTreeNode<TBound, TItem>> Nodes => mNodes;

		/// <summary>
		/// A subset of <c>Nodes</c> which are leaf nodes.
		/// </summary>
		public IReadOnlyList<NTreeNode<TBound, TItem>> Leaves => mLeaves;

		/// <summary>
		/// See <see cref="IntersectsBoundDelegate"/>.
		/// </summary>
		public IntersectsBoundDelegate IntersectsBound { get; set; }

		/// <summary>
		/// See <see cref="ShouldSubdivideDelegate"/>.
		/// </summary>
		public ShouldSubdivideDelegate ShouldSubdivide { get; set; }

		/// <summary>
		/// See <see cref="GetSubdividedVolumeForChildDelegate"/>.
		/// </summary>
		public GetSubdividedVolumeForChildDelegate GetSubdividedVolumeForChild { get; set; }

		/// <summary>
		/// See <see cref="ResolveIntersectionsDelegate"/>.
		/// </summary>
		public ResolveIntersectionsDelegate ResolveIntersections { get; set; }
		#endregion

		public NTree( TBound rootBound, IReadOnlyList<TItem> items, int dimensions )
		{
			mItems = items.ToList();
			mBound = rootBound;
			mDimensions = dimensions;
		}

		/// <summary>
		/// Clears the tree.
		/// </summary>
		public void Clear()
		{
			mLeaves.Clear();
			mNodes.Clear();
			mItems.Clear();
		}

		public void Add( TItem item ) => mItems.Add( item );

		/// <summary>
		/// Builds the tree.
		/// </summary>
		public void Build()
		{
			mLeaves.Clear();
			mNodes.Clear();

			NTreeNode<TBound, TItem> node = new( mBound, mDimensions );
			mNodes.Add( node );

			// No elements, root node is empty
			if ( mItems.Count <= 0 )
			{
				return;
			}

			// Fill with items
			for ( int i = 0; i < mItems.Count; i++ )
			{
				if ( IntersectsBound( mItems[i], mBound ) )
				{
					node.Add( i );
				}
			}

			// Recursively build nodes in the tree
			BuildNode( node );

			// Now that the tree is built, extract the leaf nodes
			for ( int i = 0; i < mNodes.Count; i++ )
			{
				if ( mNodes[i].IsLeaf() )
				{
					mLeaves.Add( mNodes[i] );
				}
			}
		}

		private void BuildNode( NTreeNode<TBound, TItem> node )
		{
			// Bail out, no need to subdivide
			if ( !ShouldSubdivide( node ) )
			{
				return;
			}

			// The node can be subdivided, create the child nodes
			// and figure out which element belongs to which node
			int itemIndex = -1;
			node.CreateChildren( mNodes, GetSubdividedVolumeForChild );
			node.ForEachItem( mItems, item =>
			{
				itemIndex++;

				TBound[] childrenBounds = new TBound[Combinations];
				for ( int i = 0; i < childrenBounds.Length; i++ )
				{
					childrenBounds[i] = node.Children[i].Bound;
				}

				if ( ResolveIntersections( childrenBounds, item, out int[] hits ) )
				{
					for ( int i = 0; i < hits.Length; i++ )
					{
						node.Children[hits[i]].Add( itemIndex );
					}
				}
			} );

			// Now that we've done that work, let's move down the tree
			node.ForEachChildNode( child =>
			{
				BuildNode( child );
			} );
		}
	}
}
