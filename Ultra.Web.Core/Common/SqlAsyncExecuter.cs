using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading;

namespace Ultra.Web.Core.Common
{
    [Serializable]
    public class SqlAsyncExecuter
    {
        private string _connstr;
        private EventHandler<SqlAsyncExecuteResultArg> onExecuteCompleted;

        public event EventHandler<SqlAsyncExecuteResultArg> OnExecuteCompleted
        {
            add
            {
                EventHandler<SqlAsyncExecuteResultArg> handler2;
                EventHandler<SqlAsyncExecuteResultArg> onExecuteCompleted = this.onExecuteCompleted;
                do
                {
                    handler2 = onExecuteCompleted;
                    EventHandler<SqlAsyncExecuteResultArg> handler3 = (EventHandler<SqlAsyncExecuteResultArg>) Delegate.Combine(handler2, value);
                    onExecuteCompleted = Interlocked.CompareExchange<EventHandler<SqlAsyncExecuteResultArg>>(ref this.onExecuteCompleted, handler3, handler2);
                }
                while (onExecuteCompleted != handler2);
            }
            remove
            {
                EventHandler<SqlAsyncExecuteResultArg> handler2;
                EventHandler<SqlAsyncExecuteResultArg> onExecuteCompleted = this.onExecuteCompleted;
                do
                {
                    handler2 = onExecuteCompleted;
                    EventHandler<SqlAsyncExecuteResultArg> handler3 = (EventHandler<SqlAsyncExecuteResultArg>) Delegate.Remove(handler2, value);
                    onExecuteCompleted = Interlocked.CompareExchange<EventHandler<SqlAsyncExecuteResultArg>>(ref this.onExecuteCompleted, handler3, handler2);
                }
                while (onExecuteCompleted != handler2);
            }
        }

        public SqlAsyncExecuter()
        {
            this._connstr = string.Empty;
        }

        public SqlAsyncExecuter(string con)
        {
            this._connstr = string.Empty;
            this.ConnectionString = con;
            this.OpenAsyncProcess();
        }

        public void Execute(string sql)
        {
            if (!string.IsNullOrEmpty(sql))
            {
                SqlConnection connection = new SqlConnection(this.ConnectionString);
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();
                command.BeginExecuteReader(delegate (IAsyncResult arg) {
                    if ((this.onExecuteCompleted != null) && arg.IsCompleted)
                    {
                        SqlAsyncExecuteResultArg e = new SqlAsyncExecuteResultArg {
                            AsyncResult = arg
                        };
                        this.onExecuteCompleted(this, e);
                    }
                }, null, CommandBehavior.CloseConnection);
            }
        }

        public void ExecuteSqlFile(string sqlFilePath)
        {
            if (File.Exists(sqlFilePath))
            {
                string str = File.ReadAllText(sqlFilePath);
                if (!string.IsNullOrEmpty(str))
                {
                    this.Execute(str);
                }
            }
        }

        private void OpenAsyncProcess()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(this.ConnectionString);
            if (!builder.AsynchronousProcessing)
            {
                builder.AsynchronousProcessing = true;
                this.ConnectionString = builder.ConnectionString;
            }
        }

        public string ConnectionString
        {
            get
            {
                return this._connstr;
            }
            set
            {
                this._connstr = value;
            }
        }
    }

    public class SqlAsyncExecuteResultArg : EventArgs {
        public IAsyncResult AsyncResult { get; set; }

        public object Extra { get; set; }
    }
}

