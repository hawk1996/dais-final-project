using Microsoft.Data.SqlClient;

namespace FinalProject.Repository
{
    public class UnitOfWork : IAsyncDisposable
    {
        private SqlConnection? _connection;
        private SqlTransaction? _transaction;

        public SqlConnection Connection => _connection ?? throw new InvalidOperationException("Connection not opened.");
        public SqlTransaction? Transaction => _transaction;

        public async Task BeginTransactionAsync()
        {
            _connection = await ConnectionFactory.CreateConnectionAsync();
            _transaction = _connection.BeginTransaction();
        }

        public async Task CommitAsync()
        {
            if (_transaction == null) throw new InvalidOperationException("Transaction not started.");

            await _transaction.CommitAsync();
            await DisposeAsync();
        }

        public async Task RollbackAsync()
        {
            if (_transaction == null) throw new InvalidOperationException("Transaction not started.");

            await _transaction.RollbackAsync();
            await DisposeAsync();
        }

        public async ValueTask DisposeAsync()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }

            if (_connection != null)
            {
                await _connection.DisposeAsync();
                _connection = null;
            }
        }
    }

}
