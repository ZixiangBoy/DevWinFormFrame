using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Ultra.Surface.Common;

namespace PetaPoco
{
    public partial class Database
    {
        public Database(string connectionString, bool ismysql)
        {
            _connectionString = connectionString;
            _providerName = ismysql ? "MySql.Data.MySqlClient" : "System.Data.SqlClient";
            CommonConstruct();
        }

        public Database()//:this(true,Apex.Surmnt.Common.DbConnection.ConnectionString)
        {
            string connectionString = string.Empty;
            try
            {
                connectionString = Util.Decrypt(System.Configuration.ConfigurationManager.ConnectionStrings["dbconstr"].ConnectionString);
            }
            catch (Exception)
            {
#if DEBUG
                connectionString = @"server=113.105.237.98,4908\mssql2012;uid=sa;pwd=1qaz2wsx;database=erp_power";
#else
                connectionString = string.Empty;
#endif
            }
            _connectionString = connectionString;
            _providerName = true ? "System.Data.SqlClient" : "MySql.Data.MySqlClient";
            CommonConstruct();
        }


        public DataTable ExcuteDataTable(string sql, params object[] args) {
            DataTable dt = new DataTable();
            OpenSharedConnection();
            try {
                using (var cmd = CreateCommand(_sharedConnection, sql, args)) {
                    using (DbDataAdapter dbDataAdapter = _factory.CreateDataAdapter()) {
                        dbDataAdapter.SelectCommand = (DbCommand)cmd;
                        dbDataAdapter.Fill(dt);
                        return dt;
                    }
                }
            } catch (Exception x) {
                OnException(x);
                throw;
            } finally {
                CloseSharedConnection();
            }
        }
    }
}
