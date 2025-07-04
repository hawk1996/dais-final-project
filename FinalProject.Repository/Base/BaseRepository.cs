﻿using Microsoft.Data.SqlClient;

namespace FinalProject.Repository.Base
{
    public abstract class BaseRepository<TObj, TFilter, TUpdate>
        where TObj : class
    {

        protected abstract string GetTableName();
        protected abstract string GetIdColumnName();
        protected abstract string[] GetColumns();
        protected abstract TObj MapEntity(SqlDataReader reader);

        public async Task<int> CreateAsync(TObj entity)
        {
            var table = GetTableName();
            var columns = GetColumns();

            var columnNames = string.Join(", ", columns);
            var paramNames = string.Join(", ", columns.Select(c => "@" + c));

            var sql = $@"INSERT INTO {table} ({columnNames})
                    VALUES ({paramNames}); 
                    SELECT CAST(SCOPE_IDENTITY() as int)";

            using var connection = await ConnectionFactory.CreateConnectionAsync();
            using var command = new SqlCommand(sql, connection);

            foreach (var column in columns)
            {
                var value = typeof(TObj).GetProperty(column)?.GetValue(entity);
                command.Parameters.AddWithValue("@" + column, value ?? DBNull.Value);
            }

            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<TObj?> RetrieveByIdAsync(int id)
        {
            var table = GetTableName();
            var idColumn = GetIdColumnName();
            var sql = $"SELECT * FROM {table} WHERE {idColumn} = @id";

            using var connection = await ConnectionFactory.CreateConnectionAsync();
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                return MapEntity(reader);

            return default;
        }

        public async IAsyncEnumerable<TObj> RetrieveCollectionAsync(TFilter? filterParam = default)
        {

            var filters = new Dictionary<string, object>();
            if (filterParam != null)
            {
                var properties = typeof(TFilter).GetProperties();
                foreach (var property in properties)
                {
                    var value = property.GetValue(filterParam);
                    if (value != null)
                    {
                        filters[property.Name] = value;
                    }
                }
            }

            var table = GetTableName();
            var sql = $"SELECT * FROM {table} WHERE 1 = 1";
            using var connection = await ConnectionFactory.CreateConnectionAsync();
            using var command = new SqlCommand(sql, connection);

            if (filters.Count != 0)
            {
                foreach (var filter in filters)
                {
                    command.CommandText += $" AND {filter.Key} = @{filter.Key}";
                    var paramName = $"@{filter.Key}";
                    var value = filter.Value;

                    if (value.GetType().IsEnum)
                    {
                        command.Parameters.AddWithValue(paramName, Convert.ToInt32(value));
                    }
                    else
                    {
                        command.Parameters.AddWithValue(paramName, value);
                    }

                }
            }

            await using SqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                TObj entity = MapEntity(reader);
                yield return entity;
            }
        }

        public async Task<bool> UpdateAsync(
            int id,
            TUpdate? updateParam = default,
            SqlConnection? externalConnection = null,
            SqlTransaction? externalTransaction = null)
        {
            var updatedValues = new Dictionary<string, object>();
            if (updateParam != null)
            {
                var properties = typeof(TUpdate).GetProperties();
                foreach (var property in properties)
                {
                    var value = property.GetValue(updateParam);
                    if (value != null)
                    {
                        updatedValues[property.Name] = value;
                    }
                }
            }

            if (updatedValues.Count == 0)
                return false;

            var table = GetTableName();
            var idColumn = GetIdColumnName();

            var validColumns = GetColumns().ToHashSet(StringComparer.OrdinalIgnoreCase);
            foreach (var col in updatedValues.Keys)
            {
                if (!validColumns.Contains(col))
                    throw new ArgumentException($"Invalid column: {col}");
            }

            var setClause = string.Join(", ", updatedValues.Keys.Select(c => $"{c} = @{c}"));
            var sql = $"UPDATE {table} SET {setClause} WHERE {idColumn} = @{idColumn}";

            bool createdConnection = false;
            SqlConnection? connection = externalConnection;
            if (connection == null)
            {
                connection = await ConnectionFactory.CreateConnectionAsync();
                createdConnection = true;
            }

            try
            {
                using var command = new SqlCommand(sql, connection);

                if (externalTransaction != null)
                    command.Transaction = externalTransaction;

                foreach (var kvp in updatedValues)
                {
                    var paramName = "@" + kvp.Key;
                    var value = kvp.Value;

                    if (value == null)
                    {
                        command.Parameters.AddWithValue(paramName, DBNull.Value);
                    }
                    else if (value.GetType().IsEnum)
                    {
                        command.Parameters.AddWithValue(paramName, Convert.ToInt32(value));
                    }
                    else
                    {
                        command.Parameters.AddWithValue(paramName, value);
                    }
                }

                command.Parameters.AddWithValue("@" + idColumn, id);

                var rows = await command.ExecuteNonQueryAsync();
                return rows != 0;
            }

            finally
            {
                if (createdConnection)
                {
                    await connection.DisposeAsync();
                }
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var table = GetTableName();
            var idColumn = GetIdColumnName();
            var sql = $"DELETE FROM {table} WHERE {idColumn} = @id";

            using var connection = await ConnectionFactory.CreateConnectionAsync();
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);

            var rows = await command.ExecuteNonQueryAsync();
            return rows != 0;
        }

    }
}
