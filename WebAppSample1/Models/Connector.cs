using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;

namespace WebAppSample1.Models
{

    /// <summary>
    /// SQL SERVER(Connector)
    /// </summary>
    public class Connector 
    {
        #region "Membros Privados"
        private bool _hasrowset = false;
        private SqlCommand _command = null;

        private SqlTransaction _transacao;

        private bool _batchactive = false;
        private bool _transactiondefined = false;

        private string FileLog = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + String.Format("\\SQLSERVERLOG_{0}", DateTime.Now.ToString("yyyyMMdd"));

        /// <summary>
        /// Usuário de Conexão
        /// </summary>
        public string UserHost { get; internal set; } = "";

        /// <summary>
        /// Versão do Aplicativo
        /// </summary>
        public string Version
        {
            get { return ((AssemblyFileVersionAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyFileVersionAttribute), false)).Version; }
        }
        /// <summary>
        /// Ultima Instrução SQL Executa
        /// </summary>
        public string LastSQL { get; internal set; } = "";

        /// <summary>
        /// Retorna true se a conexão estiver aberta
        /// </summary>
        public bool Opened { get; internal set; } = false;



        /// <summary>
        /// Número de Registros afetados
        /// </summary>
        public int RecordsAffecteds { get; set; } = 0;




        #endregion



        /// <summary>
        /// Determina se o DataTable é válido para leitura
        /// </summary>
        /// <returns></returns>
        public bool HasRowSet()
        {
            return _hasrowset;
        }
        /// <summary>
        /// Objeto de Conexão
        /// </summary>
        public SqlConnection Connection = null;

        /// <summary>
        /// Define se aplica a transação
        /// </summary>
        public bool ApplyTransaction { get; set; } = false;

        /// <summary>
        /// Command de Execução
        /// </summary>
        public SqlCommand Comando
        {
            get { return _command; }
        }


        private void ResetLog()
        {
            string _version = ((AssemblyFileVersionAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyFileVersionAttribute), false)).Version;
        }


        internal void LogWrite(string text)
        {
            try
            {
                using (StreamWriter sw = File.AppendText(this.FileLog))
                {
                    sw.WriteLine(string.Format("{0} {1} {2} {3}", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff"), this.Version.ToString().Trim(), "", text));
                    sw.Flush();
                }
            }
            catch { }
        }



        public Connector(string connectionString)
        {
            this.Opened = false;
            try
            {
                if (connectionString != "")
                {
                    Connection = new SqlConnection(connectionString);
                    Connection.Open();
                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
                    this.UserHost = builder.UserID;
                    if (this.UserHost == "")
                        this.UserHost = "SQLServer";
                    string pass = builder.Password;

                    if (Connection.State == ConnectionState.Open)
                        this.Opened = true;
                }
                else
                {
                    LogWrite("STRING DE CONEXAO VAZIA");
                }
            }
            catch (Exception Error)
            {
                LogWrite(Error.Message);
                this.Opened = false;
            }
            finally
            {
            }
        }

        /// <summary>
        /// Fecha a conexão atual
        /// </summary>
        public void Close()
        {
            if (_transacao != null)
                _transacao.Dispose();
            if (_command != null)
                _command.Dispose();
            if (this.Connection != null)
            {
                if (this.Connection.State == ConnectionState.Open)
                {
                    this.Connection.Close();
                }
                this.Connection.Dispose();
            }
            _transacao = null;
            _command = null;
            this.Connection = null;
        }
        /// <summary>
        /// Executa uma Action Query
        /// </summary>
        /// <returns>int</returns>
        public int ExecuteNonQuery()
        {
            this.LastSQL = GetExtendedSql();
            LogWrite(this.LastSQL);
            this.RecordsAffecteds = this.Comando.ExecuteNonQuery();
            return this.RecordsAffecteds;
        }
        /// <summary>
        /// Executa uma Action Query
        /// </summary>
        /// <param name="SQL">Script SQL</param>
        /// <returns>int</returns>
        public int ExecuteNonQuery(string SQL)
        {
            LogWrite(SQL);
            try
            {
                SetCommand(SQL, CommandType.Text, true);
                this.RecordsAffecteds = this.Comando.ExecuteNonQuery();
                return this.RecordsAffecteds;
            }
            catch (Exception Error)
            {
                LogWrite("FALHA NA EXECUCAO DA PROCEDURE");
                LogWrite(Error.Message);
            }
            return 0;
        }
        /// <summary>
        /// Executa uma Query Escalar
        /// </summary>
        /// <param name="SQL">Script SQL</param>
        /// <returns>int</returns>
        public int ExecuteScalar(string SQL)
        {
            SetCommand(SQL, CommandType.Text, true);
            return ExecuteScalar();
        }

        /// <summary>
        /// Executa uma Query Escalar
        /// </summary>
        /// <returns></returns>
        public int ExecuteScalar()
        {
            int _retorno = 0;
            this.LastSQL = GetExtendedSql();
            LogWrite(this.LastSQL);
            try
            {
                if (this.Comando.Connection.State == ConnectionState.Open)
                    _retorno = (int)this.Comando.ExecuteScalar();
                else
                    LogWrite("CONNECTION IS CLOSED");
            }
            catch (Exception Error)
            {
                LogWrite(Error.Message);
            }
            return _retorno;
        }


        /// <summary>
        /// Define uma Comando 
        /// </summary>
        /// <param name="obj">SqlCommand</param>
        public void SetCommand(SqlCommand obj)
        {
            _command = obj;
        }
        /// <summary>
        /// Define uma Comando 
        /// </summary>
        /// <param name="SQL">Script SQL</param>
        public void SetCommand(string SQL)
        {
            SetCommand(SQL, CommandType.Text);
        }
        /// <summary>
        /// Define uma Comando 
        /// </summary>
        /// <param name="SQL">Script SQL</param>
        /// <param name="ptype">CommandType</param>
        public void SetCommand(string SQL, CommandType ptype)
        {
            _command = new SqlCommand(SQL, this.Connection);
            _command.CommandType = ptype;
            if (_transactiondefined)
            {
                if (_transacao != null)
                    _command.Transaction = _transacao;
            }
        }
        /// <summary>
        /// Define uma Comando 
        /// </summary>
        /// <param name="SQL">Script SQL</param>
        /// <param name="ptype">CommandType</param>
        /// <param name="pactivetransaction">define se usa uma transação ativa</param>
        public void SetCommand(string SQL, CommandType ptype, bool pactivetransaction)
        {
            _command = new SqlCommand(SQL, this.Connection);
            _command.CommandType = ptype;
            if (_transactiondefined)
            {
                if (_transacao != null)
                    _command.Transaction = _transacao;
            }
        }

        /// <summary>
        /// Retorna true se existe um Batch Ativo
        /// </summary>
        public bool BatchActive
        {
            get { return _batchactive; }
        }



        /// <summary>
        /// Retorna um conjunto de dados através de um SQL Script
        /// </summary>
        /// <param name="SQL">Script SQL</param>
        /// <returns>DataTable</returns>
        public DataTable GetTable(string SQL)
        {
            return GetTable(CommandType.Text, SQL);
        }
        /// <summary>
        /// Retorna um conjunto de dados através de um SQL Script
        /// </summary>
        /// <param name="commandtype">CommandType</param>
        /// <param name="SQL">Script SQL</param>
        /// <returns>DataTable</returns>
        public DataTable GetTable(CommandType commandtype, string SQL)
        {
            SetCommand(SQL);
            _command.CommandType = commandtype;
            return GetTable();
        }
        /// <summary>
        /// Retorna um conjunto de dados através de um SQL Script
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetTable()
        {
            DataTable table = new DataTable("item");
            _hasrowset = false;
            if (this.Connection != null)
            {
                if (this.Connection.State == ConnectionState.Open)
                {
                    try
                    {
                        SqlDataAdapter _adapter = new SqlDataAdapter(_command);
                        this.LastSQL = GetExtendedSql();
                        LogWrite(this.LastSQL);
                        _adapter.Fill(table);
                        _adapter.Dispose();
                        _adapter = null;


                        if ((table != null) && (table.Rows.Count > 0))
                            _hasrowset = true;
                    }
                    catch (Exception Error)
                    {
                        LogWrite(Error.Message);
                    }
                }
                else
                {
                    LogWrite("CONEXAO QUEBRADA");
                }
            }
            else
            {
                LogWrite("FALHA DE CONEXAO");
            }

            return table;
        }



        /// <summary>
        /// Define um parametro SQL
        /// </summary>
        /// <param name="pname">Nome do Parâmetro</param>
        /// <param name="pType">System.Data.SqlDbType</param>
        /// <param name="pValue">Object</param>
        /// <returns>SqlParameter</returns>
        public SqlParameter CreateParameter(string pname, System.Data.SqlDbType pType, object pValue)
        {
            SqlParameter param = new SqlParameter(pname, pType);
            param.Value = pValue;
            return param;
        }
        /// <summary>
        /// Define um parametro SQL (Output)
        /// </summary>
        /// <param name="pname">Nome do Parâmetro</param>
        /// <param name="pType">System.Data.SqlDbType</param>
        /// <returns>SqlParameter</returns>
        public SqlParameter CreateParameterOut(string pname, System.Data.SqlDbType pType)
        {
            SqlParameter param = new SqlParameter(pname, pType);
            param.Direction = ParameterDirection.Output;
            return param;
        }
        /// <summary>
        /// Define um parametro SQL (Output)
        /// </summary>
        /// <param name="pname">Nome do Parâmetro</param>
        /// <param name="pType">System.Data.SqlDbType</param>
        /// <param name="pSize">Tamanho</param>
        /// <returns>SqlParameter</returns>
        public SqlParameter CreateParameterOut(string pname, System.Data.SqlDbType pType, int pSize)
        {
            return CreateParameter(pname, pType, pSize, ParameterDirection.Output);
        }
        /// <summary>
        /// Define um parametro SQL (ReturnValue)
        /// </summary>
        /// <param name="pname">Nome do Parâmetro</param>
        /// <param name="pType">System.Data.SqlDbType</param>
        /// <param name="pSize">Tamanho</param>
        /// <returns>SqlParameter</returns>
        public SqlParameter CreateParameterReturnValue(string pname, System.Data.SqlDbType pType, int pSize = 4)
        {
            return CreateParameter(pname, pType, pSize, ParameterDirection.ReturnValue);
        }
        /// <summary>
        /// Define um parametro SQL (ReturnValue)
        /// </summary>
        /// <returns>SqlParameter</returns>
        public SqlParameter CreateParameterReturnValue()
        {
            return CreateParameter("RETORNO", System.Data.SqlDbType.Int, 4, ParameterDirection.ReturnValue);
        }
        private SqlParameter CreateParameter(string pname, System.Data.SqlDbType pType, int pSize, ParameterDirection direction)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = pname;
            param.SqlDbType = pType;
            param.Direction = direction;
            param.Size = pSize;
            return param;
        }
        private String GetExtendedSql()
        {
            string _sql = _command.CommandText + " ";
            if (_command.CommandType == CommandType.StoredProcedure)
            {
                foreach (SqlParameter sp in _command.Parameters)
                {
                    _sql += sp.ParameterName + " = " + ParameterValueForSQL(sp) + ",";
                }
                if (_sql != "")
                    _sql = _sql.Substring(0, _sql.Length - 1);
            }
            return _sql;
        }

        private String ParameterValueForSQL(SqlParameter sp)
        {
            String retval = "";
            string val = "";
            if (sp.Value != null)
                val = sp.Value.ToString();

            switch (sp.SqlDbType)
            {
                case SqlDbType.Char:
                case SqlDbType.NChar:
                case SqlDbType.NText:
                case SqlDbType.NVarChar:
                case SqlDbType.Text:
                case SqlDbType.Time:
                case SqlDbType.VarChar:
                case SqlDbType.Xml:
                case SqlDbType.Date:
                case SqlDbType.DateTime:
                case SqlDbType.DateTime2:
                case SqlDbType.DateTimeOffset:
                    retval = "'" + val.Replace("'", "''") + "'";
                    break;

                case SqlDbType.Bit:
                    if (!String.IsNullOrEmpty(val))
                    {
                        retval = (bool.Parse(val) == false) ? "1" : "0";
                    }
                    else
                        retval = "null";
                    //retval = (bool.Parse(val)== false) ? "1" : "0";
                    break;

                default:
                    retval = val.Replace("'", "''");
                    break;
            }
            return retval;
        }
    }
}
