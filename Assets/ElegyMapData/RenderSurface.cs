﻿// SPDX-FileCopyrightText: 2023 Admer Šuko
// SPDX-License-Identifier: MIT

namespace Elegy.Assets.ElegyMapData
{
	/// <summary>
	/// An ELF render surface is concrete render data that describes
	/// a little part of world geometry. It is not necessarily a
	/// single face or a few coplanar faces, but rather a mesh consisting
	/// of all faces belonging to one octree leaf, where the faces share the same material.
	/// </summary>
	public class RenderSurface
	{
		/// <summary>
		/// AABB of this render surface.
		/// </summary>
		public Aabb BoundingBox { get; set; } = new();
		/// <summary>
		/// Position buffer.
		/// </summary>
		public List<Vector3> Positions { get; set; } = new();
		/// <summary>
		/// Normal buffer. Tangents and bitangents
		/// are built from normals and UVs.
		/// </summary>
		public List<Vector3> Normals { get; set; } = new();
		/// <summary>
		/// UV buffer.
		/// </summary>
		public List<Vector2> Uvs { get; set; } = new();
		/// <summary>
		/// Secondary UV buffer for lightmaps.
		/// </summary>
		public List<Vector2> LightmapUvs { get; set; } = new();
		/// <summary>
		/// Colour buffer.
		/// </summary>
		public List<Vector4> Colours { get; set; } = new();
		/// <summary>
		/// Index buffer.
		/// </summary>
		public List<int> Indices { get; set; } = new();
		/// <summary>
		/// Actual number of vertices.
		/// </summary>
		public int VertexCount { get; set; } = 0;

		/// <summary>
		/// The name of the material associated with this surface.
		/// Multimaterials could perhaps be autogenerated from a collection
		/// of surfaces with fundamentally similar materials.
		/// </summary>
		public string Material { get; set; } = string.Empty;
		/// <summary>
		/// Lightmap texture name. It's up to the engine to autogenerate
		/// materials for these lightmaps.
		/// </summary>
		public string LightmapTexture { get; set; } = string.Empty;
	}
}
