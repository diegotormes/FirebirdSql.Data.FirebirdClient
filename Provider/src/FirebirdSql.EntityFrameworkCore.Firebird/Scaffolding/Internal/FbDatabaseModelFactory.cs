﻿/*
 *    The contents of this file are subject to the Initial
 *    Developer's Public License Version 1.0 (the "License");
 *    you may not use this file except in compliance with the
 *    License. You may obtain a copy of the License at
 *    https://github.com/FirebirdSQL/NETProvider/blob/master/license.txt.
 *
 *    Software distributed under the License is distributed on
 *    an "AS IS" basis, WITHOUT WARRANTY OF ANY KIND, either
 *    express or implied. See the License for the specific
 *    language governing rights and limitations under the License.
 *
 *    All Rights Reserved.
 */

//$Authors = Jiri Cincura (jiri@cincura.net)

using System;
using System.Data;
using System.Data.Common;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace FirebirdSql.EntityFrameworkCore.Firebird.Scaffolding.Internal
{
	public class FbDatabaseModelFactory : IDatabaseModelFactory
	{
		DatabaseModel _databaseModel;
		FbConnection _connection;
		Version _serverVersion;

		public DatabaseModel Create(string connectionString, DatabaseModelFactoryOptions options)
		{
			using (var connection = new FbConnection(connectionString))
			{
				return Create(connection, options);
			}
		}

		public DatabaseModel Create(DbConnection connection, DatabaseModelFactoryOptions options)
		{
			ResetState();

			_connection = (FbConnection)connection;

			var connectionStartedOpen = _connection.State == ConnectionState.Open;
			if (!connectionStartedOpen)
			{
				_connection.Open();
			}
			try
			{
				_databaseModel.DatabaseName = _connection.Database;
				_serverVersion = Data.Services.FbServerProperties.ParseServerVersion(_connection.ServerVersion);
#warning Finish
				return _databaseModel;
			}
			finally
			{
				if (!connectionStartedOpen)
				{
					_connection.Close();
				}
			}
		}

		void ResetState()
		{
			_databaseModel = new DatabaseModel();
			_connection = null;
			_serverVersion = null;
		}
	}
}
