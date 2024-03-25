using System;
using POC.Artifacts.SQL.Transactions.Interfaces;

namespace POC.Artifacts.SQL.Transactions
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly SQLDbContextBase _dbContext;
        private int _totalTransactions { get; set; }
        public UoWTransactionStatus currentTransactionState { get; private set; }

        public UnitOfWork(SQLDbContextBase dbContext)
        {
            _dbContext = dbContext;
        }

        public void Begin()
        {
            if (_dbContext.Connection.State == System.Data.ConnectionState.Closed)
                _dbContext.Connection.Open();

            if (currentTransactionState != UoWTransactionStatus.Active)
            {
                _dbContext.Transaction = _dbContext.Connection.BeginTransaction();
                currentTransactionState = UoWTransactionStatus.Active;
            }

            _totalTransactions++;
        }

        public void Commit()
        {
            if (currentTransactionState == UoWTransactionStatus.Active
                && _totalTransactions == 1)
            {
                _dbContext.Transaction.Commit();
                currentTransactionState = UoWTransactionStatus.Commited;
            }

            if (_totalTransactions > 0)
                _totalTransactions--;
        }

        public void Rollback()
        {
            if (_dbContext.Connection.State != System.Data.ConnectionState.Closed
                && currentTransactionState == UoWTransactionStatus.Active)
            {
                _dbContext.Transaction.Rollback();
                _totalTransactions = 0;
                currentTransactionState = UoWTransactionStatus.Rollbacked;
            }
        }

        public void Scope(Action<Action> act)
        {
            try
            {
                bool commited = false;
                Action execCommit = () => {
                    Commit();
                    commited = true;
                };

                Begin();
                act(execCommit);

                if (!commited)
                    this.Rollback();
            }
            catch (Exception)
            {
                this.Rollback();
                throw;
            }
        }

        public async Task ScopeAsync(Func<Action, Task> act)
        {
            try
            {
                bool commited = false;
                Action execCommit = () => {
                    Commit();
                    commited = true;
                };

                Begin();
                await act(execCommit);

                if (!commited)
                    this.Rollback();
            }
            catch (Exception)
            {
                this.Rollback();
                throw;
            }
        }

        public void Dispose()
        {
            _dbContext.Transaction?.Dispose();
            _dbContext.Connection?.Close();
        }

        public enum UoWTransactionStatus
        {
            InDoubt,
            Active,
            Commited,
            Rollbacked,
            Aborted
        }
    }
}

