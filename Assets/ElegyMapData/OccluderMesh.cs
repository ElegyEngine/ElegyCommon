﻿// SPDX-FileCopyrightText: 2023 Admer Šuko
// SPDX-License-Identifier: MIT

namespace Elegy.Assets.ElegyMapData
{
	public class OccluderMesh
	{
		public List<Vector3> Positions { get; set; } = new();
		public List<int> Indices { get; set; } = new();
	}
}