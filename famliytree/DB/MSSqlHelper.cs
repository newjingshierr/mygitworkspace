using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using System.Data.Common;
using System.Collections;

namespace DB
{
    class MSSqlHelper
    {
    }

    /// <summary>
    /// 单例模式实例化数据库操作辅助类
    /// </summary>
    public class DbUtility
    {

        #region 私有字段

        private static readonly string connectionString = ConfigurationManager.AppSettings["AzureMediaPortalContext"];
        private readonly DbProviderFactory providerFactory;

        #endregion

        #region 公共字段
        /// <summary>
        /// 获取 Web.config文件中配置的 MSSQL 连接的字符串
        /// </summary>
        public string ConnectionString
        {
            get;
            private set;
        }
        #endregion

        #region 创建通用数据库操作类的单一实例

        static volatile DbUtility instance = null;
        static readonly object padlock = new object();

        /// <summary>
        /// 单例模式实例化数据库操作通用类
        /// </summary>
         DbUtility()
        {
            ConnectionString = connectionString;
            //providerFactory = ProviderFactory.GetDbProviderFactory(DbProviderType.SqlServer);
            if (providerFactory == null)
            {
                throw new ArgumentException("Can't load DbProviderFactory for given value of providerType");
            }
        }
        /// <summary>
        /// 单例模式实例化数据库操作通用类
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="providerType">数据库类型枚举</param>
        private DbUtility(string connectionString, DbProviderType providerType)
        {
            ConnectionString = connectionString;
          //  providerFactory = ProviderFactory.GetDbProviderFactory(providerType);
            if (providerFactory == null)
            {
                throw new ArgumentException("Can't load DbProviderFactory for given value of providerType");
            }
        }

        /// <summary>
        /// 创建MSSQL数据库操作通用类的单一实例
        /// </summary>
        /// <returns>获取<see cref="LiFeiLin.DBUtility.DbUtility"/>的实例</returns>
        public static DbUtility GetInstance()
        {
            return GetInstance(connectionString, DbProviderType.SqlServer);
        }
        /// <summary>
        /// 创建通用数据库操作类的单一实例
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="providerType">数据库类型枚举</param>
        /// <returns>获取<see cref="LiFeiLin.DBUtility.DbUtility"/>的实例</returns>
        public static DbUtility GetInstance(string connectionString, DbProviderType providerType)
        {
            if (instance == null)
            {

                lock (padlock)
                {

                    if (instance == null)
                    {
                        instance = new DbUtility(connectionString, providerType);
                    }
                }
            }
            return instance;
        }
        #endregion

        #region 获取某个表的记录数量
        /// <summary>
        /// 获取某个表的记录数量
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public int GetDataRecordCount(string tableName)
        {
            return GetDataRecordCount(tableName);
        }
        /// <summary>   
        /// 获取某个表的记录数量   
        /// </summary>   
        /// <param name="tableName">表名</param>   
        /// <param name="where">条件</param>   
        /// <returns></returns>   
        public int GetDataRecordCount(string tableName, string where)
        {
            string strsql = "select count(1) from " + tableName;
            if (where != "")
            {
                strsql += " where " + where;
            }
            object obj = ExecuteScalar(strsql);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }
        #endregion

        #region 获取指定表中指定列的最大值
        /// <summary>
        /// 获取指定表中指定列的最大值
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public int GetMaxID(string fieldName, string tableName)
        {
            string strsql = "select max(" + fieldName + ")+1 from " + tableName;
            object obj = ExecuteScalar(strsql);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }

        #endregion

        #region 执行一个查询 SQL 语句，并根据返回值判断执行结果是否存在
        /// <summary>
        /// 执行一个查询 SQL 语句，并根据返回值判断执行结果是否存在
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public bool Exists(string strSql)
        {
            object obj = ExecuteScalar(strSql);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 执行一个查询 SQL 语句，并根据返回值判断执行结果是否存在
        /// </summary>
        /// <param name="strSql">需要执行的 SQL 查询语句</param>
        /// <param name="cmdParms">结构化参数数组</param>
        /// <returns></returns>
        public bool Exists(string strSql, params DbParameter[] cmdParms)
        {
            object obj = ExecuteScalar(strSql, cmdParms);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 表是否存在
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public bool TabExists(string tableName)
        {
            string strsql = "select count(*) from sysobjects where id = object_id(N'[" + tableName + "]') and OBJECTPROPERTY(id, N'IsUserTable') = 1";
            object obj = ExecuteScalar(strsql);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region 执行多条SQL语句，实现数据库事务

        /// <summary>   
        /// 执行多条SQL语句，实现数据库事务。   
        /// </summary>   
        /// <param name="sqlStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>   
        public void ExecuteSqlTran(Hashtable sqlStringList)
        {
            using (DbConnection connection = providerFactory.CreateConnection())
            {
                connection.Open();
                using (DbTransaction trans = connection.BeginTransaction())
                {
                    DbCommand command = null;
                    try
                    {
                        foreach (DictionaryEntry myDE in sqlStringList)
                        {
                            string cmdText = myDE.Key.ToString();
                            DbParameter[] cmdParms = (DbParameter[])myDE.Value;
                            CreateDbCommand(command, connection, trans, cmdText, cmdParms);
                            command.ExecuteNonQuery();
                            command.Parameters.Clear();
                        }
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        #endregion

        #region 执行一个查询语句，返回一个关联的DataReader实例
        /// <summary> 
        /// 执行一个查询语句，返回一个关联的DataReader实例
        /// </summary> 
        /// <param name="sqlString">要执行的查询语句</param> 
        /// <returns></returns> 
        public DbDataReader ExecuteReader(string sqlString)
        {
            return ExecuteReader(sqlString, CommandType.Text, 0, null);
        }
        /// <summary>
        /// 执行一个查询语句，返回一个关联的DataReader实例
        /// </summary>
        /// <param name="sqlString">要执行的查询语句</param>
        /// <param name="parameters">执行SQL查询语句所需要的参数</param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(string sqlString, DbParameter[] parameters)
        {
            return ExecuteReader(sqlString, CommandType.Text, 0, parameters);
        }
        /// <summary> 
        ///  执行一个查询语句，返回一个关联的DataReader实例     
        /// </summary> 
        /// <param name="sqlString">要执行的查询语句</param> 
        /// <param name="commandType">要执行查询语句的类型，如存储过程或者SQl文本命令</param> 
        /// <returns></returns> 
        public DbDataReader ExecuteReader(string sqlString, CommandType commandType)
        {
            return ExecuteReader(sqlString, commandType, 0, null);
        }

        /// <summary>   
        /// 执行一个查询语句，返回一个关联的DataReader实例     
        /// </summary>   
        /// <param name="sqlString">要执行的查询语句</param>   
        /// <param name="times">超时时间</param>
        /// <param name="parameters">执行SQL查询语句所需要的参数</param>
        /// <returns>SqlDataReader</returns>   
        public DbDataReader ExecuteReader(string sqlString, int times, params DbParameter[] parameters)
        {
            return ExecuteReader(sqlString, CommandType.Text, times, parameters);
        }

        /// <summary>     
        /// 执行一个查询语句，返回一个关联的DataReader实例     
        /// </summary>     
        /// <param name="sqlString">要执行的查询语句</param>     
        /// <param name="parameters">执行SQL查询语句所需要的参数</param>  
        /// <param name="commandType">执行的SQL语句的类型</param>  
        /// <param name="times">超时时间</param>
        /// <returns></returns>   
        public DbDataReader ExecuteReader(string sqlString, CommandType commandType, int times, DbParameter[] parameters)
        {
            DbCommand command = CreateDbCommand(sqlString, commandType, times, parameters);
            command.Connection.Open();
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }
        #endregion

        #region 执行存储过程
        /// <summary>   
        /// 执行存储过程  (使用该方法切记要手工关闭SqlDataReader和连接)   
        /// </summary>   
        /// <param name="storedProcName">存储过程名</param>   
        /// <param name="parameters">存储过程参数</param>   
        /// <returns>SqlDataReader</returns>   
        public DbDataReader RunProcedure(string storedProcName, IDbDataParameter[] parameters)
        {
            DbConnection connection = providerFactory.CreateConnection();
            connection.ConnectionString = ConnectionString;
            DbDataReader returnReader;
            connection.Open();
            DbCommand command = CreateDbQueryCommand(connection, storedProcName, parameters);
            command.CommandType = CommandType.StoredProcedure;
            returnReader = command.ExecuteReader();
            //Connection.Close(); 不能在此关闭，否则，返回的对象将无法使用               
            return returnReader;
        }

        /// <summary>   
        /// 执行存储过程   
        /// </summary>   
        /// <param name="storedProcName">存储过程名</param>   
        /// <param name="parameters">存储过程参数</param>   
        /// <param name="tableName">DataSet结果中的表名</param>   
        /// <returns>DataSet</returns>   
        public DataSet RunProcedure(string storedProcName, IDbDataParameter[] parameters, string tableName)
        {
            using (DbConnection connection = providerFactory.CreateConnection())
            {
                DataSet dataSet = new DataSet();
                connection.ConnectionString = ConnectionString;
                connection.Open();
                DbDataAdapter sqlDA = providerFactory.CreateDataAdapter();
                sqlDA.SelectCommand = CreateDbQueryCommand(connection, storedProcName, parameters);
                sqlDA.Fill(dataSet, tableName);
                connection.Close();
                return dataSet;
            }
        }

        public DataTable CallProcedure(string storedProcName, IDbDataParameter[] parameters)
        {
            using (DbConnection connection = providerFactory.CreateConnection())
            {
                DataSet dataSet = new DataSet();
                connection.ConnectionString = ConnectionString;
                connection.Open();
                DbDataAdapter sqlDA = providerFactory.CreateDataAdapter();
                sqlDA.SelectCommand = CreateDbQueryCommand(connection, storedProcName, parameters);
                sqlDA.Fill(dataSet);
                connection.Close();
                return dataSet.Tables[0];
            }
        }
        /// <summary>   
        /// 执行存储过程   
        /// </summary>   
        /// <param name="storedProcName">存储过程名</param>   
        /// <param name="parameters">存储过程参数</param>   
        /// <param name="tableName">DataSet结果中的表名</param> 
        /// <param name="times">储存过程执行超时时间</param>
        /// <returns>DataSet</returns>   
        public DataSet RunProcedure(string storedProcName, IDbDataParameter[] parameters, string tableName, int times)
        {
            using (DbConnection connection = providerFactory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                DataSet dataSet = new DataSet();
                connection.Open();
                DbDataAdapter sqlDA = providerFactory.CreateDataAdapter();
                sqlDA.SelectCommand = CreateDbQueryCommand(connection, storedProcName, parameters);
                sqlDA.SelectCommand.CommandTimeout = times;
                sqlDA.Fill(dataSet, tableName);
                connection.Close();
                return dataSet;
            }
        }
        /// <summary>   
        /// 执行存储过程后返回执行结果（标识）   
        /// </summary>   
        /// <param name="storedProcName">储存过程名称</param>   
        /// <param name="parameters">储存过程参数</param>   
        /// <returns></returns>   
        public string RunProcedureState(string storedProcName, IDbDataParameter[] parameters)
        {
            using (DbConnection connection = providerFactory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();
                DbDataAdapter sqlDA = providerFactory.CreateDataAdapter();


                DbParameter parameter = providerFactory.CreateParameter();
                parameter.ParameterName = "ReturnValue";
                parameter.DbType = DbType.Int32;
                parameter.Size = 4;
                parameter.Direction = ParameterDirection.ReturnValue;
                parameter.SourceColumnNullMapping = false;
                parameter.SourceColumn = string.Empty;
                parameter.SourceVersion = DataRowVersion.Default;
                parameter.Value = null;

                sqlDA.SelectCommand = CreateDbQueryCommand(connection, storedProcName, parameters);

                sqlDA.SelectCommand.Parameters.Add(parameter); //增加存储过程的返回值参数   
                sqlDA.SelectCommand.ExecuteNonQuery();
                connection.Close();
                return sqlDA.SelectCommand.Parameters["ReturnValue"].Value.ToString();
            }
        }


        /// <summary>   
        /// 执行存储过程，返回影响的行数         
        /// </summary>   
        /// <param name="storedProcName">存储过程名</param>   
        /// <param name="parameters">存储过程参数</param>   
        /// <param name="rowsAffected">影响的行数</param>   
        /// <returns></returns>   
        public int RunProcedure(string storedProcName, IDbDataParameter[] parameters, out int rowsAffected)
        {
            using (DbConnection connection = providerFactory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                int result;
                connection.Open();
                DbCommand command = CreateDbCommand(connection, storedProcName, parameters);
                rowsAffected = command.ExecuteNonQuery();
                result = (int)command.Parameters["ReturnValue"].Value;
                return result;
            }
        }
        #endregion

        #region 执行查询语句并返回DataSet
        /// <summary>   
        /// 执行查询语句并返回 <see cref="System.Data.DataSet"/> 对象
        /// </summary>   
        /// <param name="sqlString">查询语句</param>   
        /// <returns>DataSet</returns>   
        public DataSet ExecuteQuery(string sqlString)
        {
            return ExecuteQuery(sqlString, CommandType.Text, 0, null);
        }
        /// <summary>   
        ///执行查询语句并返回 <see cref="System.Data.DataSet"/> 对象
        /// </summary>   
        /// <param name="sqlString"></param>   
        /// <param name="times"></param>   
        /// <returns></returns>   
        public DataSet ExecuteQuery(string sqlString, int times)
        {
            return ExecuteQuery(sqlString, CommandType.Text, times, null);
        }
        /// <summary>   
        /// 执行查询语句并返回 <see cref="System.Data.DataSet"/> 对象
        /// </summary>   
        /// <param name="sqlString">查询语句</param>   
        /// <param name="cmdParms">结构化参数</param>
        /// <returns>DataSet</returns>   
        public DataSet ExecuteQuery(string sqlString, DbParameter[] parameters)
        {
            return ExecuteQuery(sqlString, CommandType.Text, 0, parameters);
        }
        /// <summary>
        /// 执行查询语句并返回 <see cref="System.Data.DataSet"/> 对象
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="times"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public DataSet ExecuteQuery(string sqlString, CommandType commandType, int times, DbParameter[] parameters)
        {
            using (DbCommand command = CreateDbCommand(sqlString, commandType, times, parameters))
            {
                command.Connection.Open();
                using (DbDataAdapter da = providerFactory.CreateDataAdapter())
                {
                    da.SelectCommand = command;
                    DataSet ds = new DataSet();
                    da.Fill(ds, "ds");
                    command.Parameters.Clear();
                    command.Connection.Close();
                    return ds;
                }
            }
        }
        #endregion

        #region 对数据库执行增删改操作，返回受影响的行数。
        /// <summary> 
        /// 对数据库执行增删改操作，返回受影响的行数。
        /// </summary> 
        /// <param name="sqlString">要执行的sql命令</param> 
        /// <returns></returns> 
        public int ExecuteNonQuery(string sqlString)
        {
            return ExecuteNonQuery(sqlString, CommandType.Text, 0, null);
        }
        /// <summary>
        /// 对数据库执行增删改操作，返回受影响的行数。
        /// </summary>
        /// <param name="sqlString">要执行的增删改的SQL语句</param>
        /// <param name="times">超时时间</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sqlString, int times)
        {
            return ExecuteNonQuery(sqlString, CommandType.Text, times, null);
        }
        /// <summary>
        /// 对数据库执行增删改操作，返回受影响的行数。
        /// </summary>
        /// <param name="sqlString">要执行的增删改的SQL语句</param>
        /// <param name="parameters">结构化的参数列表</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sqlString, DbParameter[] parameters)
        {
            return ExecuteNonQuery(sqlString, CommandType.Text, 0, parameters);
        }
        /// <summary> 
        /// 对数据库执行增删改操作，返回受影响的行数。
        /// </summary> 
        /// <param name="sqlString">要执行的增删改的SQL语句</param> 
        /// <param name="commandType">要执行查询语句的类型，如存储过程或者sql文本命令</param> 
        /// <returns></returns> 
        public int ExecuteNonQuery(string sqlString, CommandType commandType)
        {
            return ExecuteNonQuery(sqlString, commandType, 0, null);
        }

        /// <summary>     
        /// 对数据库执行增删改操作，返回受影响的行数。     
        /// </summary>     
        /// <param name="sqlString">要执行的增删改的SQL语句</param>    
        /// <param name="commandType">执行的SQL语句的类型</param>  
        /// <param name="times">超时时间</param>
        /// <param name="parameters">执行增删改语句所需要的参数</param>  
        /// <returns></returns>  
        public int ExecuteNonQuery(string sqlString, CommandType commandType, int times, DbParameter[] parameters)
        {
            using (DbCommand command = CreateDbCommand(sqlString, commandType, times, parameters))
            {
                command.Connection.Open();
                int affectedRows = command.ExecuteNonQuery();
                command.Connection.Close();
                return affectedRows;
            }
        }
        #endregion

        #region 执行一个查询，返回结果集的首行首列。忽略其他行，其他列 
        /// <summary> 
        /// 执行一个查询，返回结果集的首行首列。忽略其他行，其他列 
        /// </summary> 
        /// <param name="sqlString">要执行的SQl命令</param> 
        /// <returns></returns> 
        public Object ExecuteScalar(string sqlString)
        {
            return ExecuteScalar(sqlString, CommandType.Text, 0, null);
        }

        /// <summary> 
        ///  执行一个查询，返回结果集的首行首列。忽略其他行，其他列 
        /// </summary> 
        /// <param name="sqlString">要执行的SQl命令</param> 
        /// <param name="times">执行超时时间</param> 
        /// <returns></returns> 
        public Object ExecuteScalar(string sqlString, int times)
        {
            return ExecuteScalar(sqlString, CommandType.Text, times, null);
        }
        /// <summary>
        /// 执行一个查询，返回结果集的首行首列。忽略其他行，其他列 
        /// </summary>
        /// <param name="sqlString">要执行的SQl命令</param>
        /// <param name="cmdParms">结构化的查询语句</param>
        /// <returns></returns>
        public object ExecuteScalar(string sqlString, params DbParameter[] cmdParms)
        {
            return ExecuteScalar(sqlString, CommandType.Text, 0, cmdParms);
        }

        /// <summary>     
        /// 执行一个查询语句，返回查询结果的第一行第一列     
        /// </summary>     
        /// <param name="sql">要执行的查询语句</param>     
        /// <param name="parameters">执行SQL查询语句所需要的参数</param>     
        /// <param name="commandType">执行的SQL语句的类型</param>  
        /// <returns></returns>     
        public Object ExecuteScalar(string sqlString, CommandType commandType, int times, DbParameter[] parameters)
        {
            using (DbCommand command = CreateDbCommand(sqlString, commandType, times, parameters))
            {
                command.Connection.Open();
                object result = command.ExecuteScalar();
                command.Connection.Close();
                return result;
            }
        }
        #endregion

        #region 执行一个查询语句，返回一个包含查询结果的DataTable
        /// <summary> 
        /// 执行一个查询语句，返回一个包含查询结果的DataTable   
        /// </summary> 
        /// <param name="sql">要执行的sql文本命令</param> 
        /// <returns>返回查询的结果集</returns> 
        public DataTable ExecuteDataTable(string sql)
        {
            return ExecuteDataTable(sql, CommandType.Text, 0, null);
        }
        /// <summary>
        /// 执行一个查询语句，返回一个包含查询结果的DataTable  
        /// </summary>
        /// <param name="sqlString">要执行的sql文本命令</param>
        /// <param name="parameters">执行SQL查询语句所需要的参数</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string sqlString, DbParameter[] parameters)
        {
            return ExecuteDataTable(sqlString, CommandType.Text, 0, parameters);
        }
        /// <summary> 
        /// 执行一个查询语句，返回一个包含查询结果的DataTable   
        /// </summary> 
        /// <param name="sqlString">要执行的sql语句</param> 
        /// <param name="commandType">要执行的查询语句的类型，如存储过程或者sql文本命令</param> 
        /// <returns>返回查询结果集</returns> 
        public DataTable ExecuteDataTable(string sqlString, CommandType commandType)
        {
            return ExecuteDataTable(sqlString, commandType, 0, null);
        }
        /// <summary>
        /// 执行一个查询语句，返回一个包含查询结果的DataTable   
        /// </summary>
        /// <param name="sqlString">要执行的sql语句</param>
        /// <param name="commandType">要执行的查询语句的类型</param>
        /// <param name="times">超时时间</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string sqlString, CommandType commandType, int times)
        {
            return ExecuteDataTable(sqlString, commandType, times, null);
        }
        /// <summary>     
        /// 执行一个查询语句，返回一个包含查询结果的DataTable     
        /// </summary>     
        /// <param name="sqlString">要执行的查询语句</param>     
        /// <param name="times">超时时间</param>
        /// <param name="commandType">执行的SQL语句的类型</param> 
        /// <param name="parameters">执行SQL查询语句所需要的参数</param>  
        /// <returns></returns>  
        public DataTable ExecuteDataTable(string sqlString, CommandType commandType, int times, DbParameter[] parameters)
        {
            using (DbCommand command = CreateDbCommand(sqlString, commandType, times, parameters))
            {
                using (DbDataAdapter adapter = providerFactory.CreateDataAdapter())
                {
                    adapter.SelectCommand = command;
                    DataTable data = new DataTable();
                    adapter.Fill(data);
                    return data;
                }
            }
        }
        #endregion

        #region 内部私有方法
        /// <summary>
        /// 构建 DbCommand 对象(用来返回一个结果集，而不是一个整数值) 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="storedProcName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private DbCommand CreateDbQueryCommand(DbConnection connection, string storedProcName, IDbDataParameter[] parameters)
        {
            DbCommand command = providerFactory.CreateCommand();
            command.CommandText = storedProcName;
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            foreach (DbParameter parameter in parameters)
            {
                if (parameter != null)
                {
                    // 检查未分配值的输出参数,将其分配以DBNull.Value.   
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) && (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    command.Parameters.Add(((ICloneable)parameter).Clone());
                }
            }
            return command;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="storedProcName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private DbCommand CreateDbCommand(DbConnection connection, string storedProcName, IDbDataParameter[] parameters)
        {
            DbCommand command = CreateDbQueryCommand(connection, storedProcName, parameters);
            DbParameter parameter = providerFactory.CreateParameter();
            parameter.ParameterName = "ReturnValue";
            parameter.DbType = DbType.Int32;
            parameter.Size = 4;
            parameter.Direction = ParameterDirection.ReturnValue;
            parameter.SourceColumnNullMapping = false;
            parameter.SourceColumn = string.Empty;
            parameter.SourceVersion = DataRowVersion.Default;
            parameter.Value = null;
            command.Parameters.Add(((ICloneable)parameter).Clone());
            return command;
        }
        /// <summary>
        /// 创建一个DbCommand对象  
        /// </summary>
        /// <param name="cmd">DbCommand对象</param>
        /// <param name="conn">DbConnection数据库连接</param>
        /// <param name="trans">DbTransaction事务对象</param>
        /// <param name="cmdText">执行的SQL语句</param>
        /// <param name="cmdParms">DbParameter参数数组</param>
        private void CreateDbCommand(DbCommand cmd, DbConnection conn, DbTransaction trans, string cmdText, DbParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
            {
                cmd.Transaction = trans;
            }
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {

                foreach (DbParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(((ICloneable)parameter).Clone());
                }
            }
        }
        /// <summary>  
        /// 创建一个DbCommand对象  
        /// </summary>  
        /// <param name="sqlString">要执行的查询语句</param>     
        /// <param name="parameters">执行SQL查询语句所需要的参数</param>  
        /// <param name="commandType">执行的SQL语句的类型</param>  
        /// <returns></returns>  
        private DbCommand CreateDbCommand(string sqlString, CommandType commandType, int times, DbParameter[] parameters)
        {
            DbConnection connection = providerFactory.CreateConnection();
            DbCommand command = providerFactory.CreateCommand();
            connection.ConnectionString = ConnectionString;
            command.CommandText = sqlString;
            command.CommandType = commandType;
            command.Connection = connection;
            if (times > 0) { command.CommandTimeout = times; }
            if (!(parameters == null || parameters.Length == 0))
            {
                foreach (DbParameter parameter in parameters)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    command.Parameters.Add(((ICloneable)parameter).Clone());
                }
            }
            return command;
        }
        /// <summary>
        /// 创建一个DbCommand对象  
        /// </summary>
        /// <param name="sqlString">要执行的查询语句</param>
        /// <param name="times">超时时间</param>
        /// <param name="parameters">执行SQL查询语句所需要的参数</param>
        /// <returns></returns>
        private DbCommand CreateDbCommand(string sqlString, int times, DbParameter[] parameters)
        {
            DbConnection connection = providerFactory.CreateConnection();
            DbCommand command = providerFactory.CreateCommand();
            connection.ConnectionString = ConnectionString;
            command.CommandText = sqlString;
            command.CommandType = CommandType.Text;
            command.Connection = connection;
            command.Transaction = connection.BeginTransaction();

            if (times > 0) { command.CommandTimeout = times; }
            if (!(parameters == null || parameters.Length == 0))
            {
                foreach (DbParameter parameter in parameters)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    command.Parameters.Add(((ICloneable)parameter).Clone());
                }
            }
            return command;
        }
        #endregion

        #region 获取安全的SQL字符串
        /// <summary>   
        /// 获取安全的SQL字符串   
        /// </summary>   
        /// <param name="sql"></param>   
        /// <returns></returns>   
        public string GetSafeSQLString(string sql)
        {
            sql = sql.Replace(",", "，");
            sql = sql.Replace(".", "。");
            sql = sql.Replace("(", "（");
            sql = sql.Replace(")", "）");
            sql = sql.Replace(">", "＞");
            sql = sql.Replace("<", "＜");
            sql = sql.Replace("-", "－");
            sql = sql.Replace("+", "＋");
            sql = sql.Replace("=", "＝");
            sql = sql.Replace("?", "？");
            sql = sql.Replace("*", "＊");
            sql = sql.Replace("|", "｜");
            sql = sql.Replace("&", "＆");
            return sql;
        }
        #endregion  

        #region 返回当前连接的数据库中所有用户创建的数据库
        /// <summary> 
        /// 返回当前连接的数据库中所有用户创建的数据库 
        /// </summary> 
        /// <returns></returns> 
        public DataTable GetTables()
        {
            DataTable table = null;
            using (DbConnection con = providerFactory.CreateConnection())
            {
                con.Open();
                table = con.GetSchema("Tables");

            }
            return table;
        }
        #endregion
    }
    #region 数据库类型枚举
    /// <summary>  
    /// 数据库类型枚举  
    /// </summary>  
    public enum DbProviderType : byte
    {
        /// <summary>
        /// 微软 SqlServer 数据库
        /// </summary>
        SqlServer,
        /// <summary>
        /// 开源 MySql数据库
        /// </summary>
        MySql,
        /// <summary>
        /// 嵌入式轻型数据库 SQLite
        /// </summary>
        SQLite,
        /// <summary>
        /// 甲骨文 Oracle
        /// </summary>
        Oracle,
        /// <summary>
        /// 开放数据库互连
        /// </summary>
        ODBC,
        /// <summary>
        /// 面向不同的数据源的低级应用程序接口
        /// </summary>
        OleDb,
        /// <summary>
        /// 跨平台的关系数据库系统 Firebird
        /// </summary>
        Firebird,
        /// <summary>
        ///加州大学伯克利分校计算机系开发的关系型数据库 PostgreSql
        /// </summary>
        PostgreSql,
        /// <summary>
        /// IBM出口的一系列关系型数据库管理系统 DB2
        /// </summary>
        DB2,
        /// <summary>
        /// IBM公司出品的关系数据库管理系统（RDBMS）家族  Informix
        /// </summary>
        Informix,
        /// <summary>
        /// 微软推出的一个适用于嵌入到移动应用的精简数据库产品 SqlServerCe
        /// </summary>
        SqlServerCe
    }
    #endregion

}
