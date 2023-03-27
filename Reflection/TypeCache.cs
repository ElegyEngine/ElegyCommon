﻿// SPDX-FileCopyrightText: 2022-2023 Admer Šuko
// SPDX-License-Identifier: MIT

using System;
using System.Reflection;

namespace Elegy.Reflection
{
	/// <summary>
	/// The global type cache.
	/// It accelerates queries about datatypes.
	/// </summary>
	public class TypeCache
	{
		public IReadOnlyList<TypeInfo> TypeInfos => mTypeInfos;
		private List<TypeInfo> mTypeInfos = new();
	}
}
