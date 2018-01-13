using System;
using WebPirates.Cilizhu.Core.Data;

namespace WebPirates.Cilizhu.Data.SQLite
{
    public class SQLiteDataAccessFactory : IDataAccessFactory
    {
        public SQLiteDataAccessFactory(string connectionString)
        {
            this.ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        protected string ConnectionString { get; }

        public IDataAccessService Create()
        {
            return new SQLiteDataAccessService(this.ConnectionString);
        }
    }
}
