using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using Dapper;

namespace WebPirates.Cilizhu.Data.SQLite
{
    public abstract class SQLiteDataAccessBase : IDisposable
    {
        private SQLiteConnection connection;
        private SQLiteTransaction transaction;

        public SQLiteDataAccessBase(string connectionString)
        {
            this.ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        protected string ConnectionString { get; }

        #region Common

        protected SQLiteConnection GetConnection()
        {
            if (this.connection == null)
            {
                this.connection = new SQLiteConnection(this.ConnectionString);
                this.connection.Open();
            }
            return this.connection;
        }

        public void Begin(IsolationLevel level)
        {
            if (this.transaction != null)
            {
                throw new InvalidOperationException("Transaction already begun.");
            }
            this.transaction = this.GetConnection().BeginTransaction(level);
        }

        public void Commit()
        {
            if (this.transaction == null)
            {
                throw new InvalidOperationException("No transcation.");
            }
            this.transaction.Commit();
            this.transaction = null;
        }

        public void Rollback()
        {
            if (this.transaction == null)
            {
                throw new InvalidOperationException("No transcation.");
            }
            this.transaction.Rollback();
            this.transaction = null;
        }

        protected void ReleaseConnection()
        {
            if (this.transaction != null)
            {
                this.transaction.Rollback();
                this.transaction = null;
            }
            if (this.connection != null)
            {
                if (this.connection.State != ConnectionState.Closed)
                {
                    this.connection.Close();
                }
                this.connection = null;
            }
        }

        public virtual void Initialize()
        {
            var type = this.GetType();
            var name = String.Concat(type.Namespace, ".Database.sql");
            using (var stream = type.Assembly.GetManifestResourceStream(name))
            {
                var reader = new StreamReader(stream, true);
                var sql = reader.ReadToEnd();
                this.GetConnection().Execute(sql);
            }
        }

        #endregion Common

        #region IDisposable Support

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.ReleaseConnection();
                }
                this.disposed = true;
            }
        }

        ~SQLiteDataAccessBase()
        {
            Dispose(false);
        }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}
