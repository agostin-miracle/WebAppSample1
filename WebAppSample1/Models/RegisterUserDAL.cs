using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;

namespace WebAppSample1.Models
{
    public class RegisterUserDAL:IRegisterUserDAL
    {
        public bool Found { get; set; } = false;
        IOptions<ConnectionData> _ConnectionString;
        public RegisterUserDAL(IOptions<ConnectionData> ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }
        /// <summary>
        /// Insere um registro na tabela TBCADUSU (Cadastro de Usuarios)
        /// </summary>
        ///<param name="model">RegisterUsers</param>
        /// <returns>int</returns>

        public int Insert(RegisterUsers model)
        {
            int RETURN_VALUE = 0;
            try
            {
                string cs = _ConnectionString.Value.ConnectionString;
                if (!String.IsNullOrWhiteSpace(cs))
                {
                    Connector _connector = new Connector(cs);
                    _connector.SetCommand("PRCADUSUINS", CommandType.StoredProcedure, true);
                    _connector.Comando.Parameters.Add(_connector.CreateParameterOut("@RETURN_VALUE", SqlDbType.Int));
                    _connector.Comando.Parameters.Add(_connector.CreateParameter("@STAREC", SqlDbType.TinyInt, model.STAREC));
                    _connector.Comando.Parameters.Add(_connector.CreateParameter("@TIPPES", SqlDbType.VarChar, model.TIPPES));
                    _connector.Comando.Parameters.Add(_connector.CreateParameter("@NUMIRG", SqlDbType.VarChar, model.NUMIRG));
                    _connector.Comando.Parameters.Add(_connector.CreateParameter("@CODCMF", SqlDbType.VarChar, model.CODCMF));
                    _connector.Comando.Parameters.Add(_connector.CreateParameter("@NOMUSU", SqlDbType.VarChar, model.NOMUSU.ToUpper()));
                    _connector.Comando.Parameters.Add(_connector.CreateParameter("@NOMMAE", SqlDbType.VarChar, model.NOMMAE.ToUpper()));
                    _connector.Comando.Parameters.Add(_connector.CreateParameter("@DATNAC", SqlDbType.DateTime, model.DATNAC));
                    _connector.Comando.Parameters.Add(_connector.CreateParameter("@CODATR", SqlDbType.Int, model.CODATR));
                    _connector.Comando.Parameters.Add(_connector.CreateParameter("@ATRPPE", SqlDbType.Bit, model.ATRPPE));
                    _connector.Comando.Parameters.Add(_connector.CreateParameter("@UFEEMI", SqlDbType.VarChar, model.UFEEMI));
                    _connector.Comando.Parameters.Add(_connector.CreateParameter("@ORGEMI", SqlDbType.VarChar, model.ORGEMI.ToUpper()));
                    _connector.Comando.Parameters.Add(_connector.CreateParameter("@CODECV", SqlDbType.TinyInt, model.CODECV));
                  
                    _connector.ExecuteNonQuery();
                    RETURN_VALUE = (int)_connector.Comando.Parameters["@RETURN_VALUE"].Value;
                    _connector.Close();
                }
            }
            catch (Exception Error)
            {
            }
            return RETURN_VALUE;
        }

        /// <summary>
        /// Altera um registro da tabela TBCADUSU (Cadastro de Usuarios)  de acordo com a chave primaria
        /// </summary>
        ///<param name="model">RegisterUsers</param>
        /// <returns>int</returns>

        public int Update(RegisterUsers model)
        {
            int RETURN_VALUE = 0;

            try
            {
                string cs = _ConnectionString.Value.ConnectionString;
                if (!String.IsNullOrWhiteSpace(cs))
                {
                    Connector _connector = new Connector(cs);
                    _connector.SetCommand("PRCADUSUUPD", CommandType.StoredProcedure, true);
                    _connector.Comando.Parameters.Add(_connector.CreateParameterOut("@RETURN_VALUE", SqlDbType.Int));
                    _connector.Comando.Parameters.Add(_connector.CreateParameter("@CODUSU", SqlDbType.Int, model.CODUSU));
                    _connector.Comando.Parameters.Add(_connector.CreateParameter("@STAREC", SqlDbType.TinyInt, model.STAREC));
                    _connector.Comando.Parameters.Add(_connector.CreateParameter("@TIPPES", SqlDbType.VarChar, model.TIPPES));
                    _connector.Comando.Parameters.Add(_connector.CreateParameter("@NUMIRG", SqlDbType.VarChar, model.NUMIRG));
                    _connector.Comando.Parameters.Add(_connector.CreateParameter("@CODCMF", SqlDbType.VarChar, model.CODCMF));
                    _connector.Comando.Parameters.Add(_connector.CreateParameter("@NOMUSU", SqlDbType.VarChar, model.NOMUSU));
                    _connector.Comando.Parameters.Add(_connector.CreateParameter("@NOMMAE", SqlDbType.VarChar, model.NOMMAE));
                    _connector.Comando.Parameters.Add(_connector.CreateParameter("@DATNAC", SqlDbType.DateTime, model.DATNAC));
                    _connector.Comando.Parameters.Add(_connector.CreateParameter("@CODATR", SqlDbType.Int, model.CODATR));
                    _connector.Comando.Parameters.Add(_connector.CreateParameter("@ATRPPE", SqlDbType.Bit, model.ATRPPE));
                    _connector.Comando.Parameters.Add(_connector.CreateParameter("@UFEEMI", SqlDbType.VarChar, model.UFEEMI));
                    _connector.Comando.Parameters.Add(_connector.CreateParameter("@ORGEMI", SqlDbType.VarChar, model.ORGEMI));
                    _connector.Comando.Parameters.Add(_connector.CreateParameter("@CODECV", SqlDbType.TinyInt, model.CODECV));
                    _connector.ExecuteNonQuery();
                    RETURN_VALUE = (int)_connector.Comando.Parameters["@RETURN_VALUE"].Value;
                    _connector.Close();
                }
            }
            catch (Exception Error) { 
            }
            return RETURN_VALUE;
        }
        /// <summary>
        /// Seleciona o registro de acordo com o Código do Usuário
        /// </summary>
        /// <param name="pCODUSU">Código do Usuario</param>
        /// <returns>RegisterUsers</returns>

        public RegisterUsers Select(int pCODUSU)
        {
            Found = false;
            DataTable _table = null;
            RegisterUsers RETURN_VALUE = new RegisterUsers();
            try
            {
                string cs = _ConnectionString.Value.ConnectionString;
                if (!String.IsNullOrWhiteSpace(cs))
                {
                    Connector _connector = new Connector(cs);
                    _connector.SetCommand("PRCADUSUSEL", CommandType.StoredProcedure);
                    _connector.Comando.Parameters.Add(_connector.CreateParameter("@CODUSU", SqlDbType.Int, pCODUSU));
                    _table = _connector.GetTable();
                        if (_connector.HasRowSet())
                        {
                            RETURN_VALUE = new RegisterUsers
                            {
                                CODUSU = Converter.IntValue(_table.Rows[0]["CODUSU"].ToString()),
                                STAREC = Converter.ByteValue(_table.Rows[0]["STAREC"].ToString()),
                                DATCAD = Converter.DateValue(_table.Rows[0]["DATCAD"].ToString()),
                                DATUPD = Converter.DateValue(_table.Rows[0]["DATUPD"].ToString()),
                                TIPPES = _table.Rows[0]["TIPPES"].ToString(),
                                CODPJU = _table.Rows[0]["CODPJU"].ToString(),
                                NUMIRG = _table.Rows[0]["NUMIRG"].ToString().Trim(),
                                CODCMF = _table.Rows[0]["CODCMF"].ToString().Trim(),
                                NOMUSU = _table.Rows[0]["NOMUSU"].ToString().Trim(),
                                NOMMAE = _table.Rows[0]["NOMMAE"].ToString().Trim(),
                                DATNAC = Converter.DateValue(_table.Rows[0]["DATNAC"].ToString()),
                                CODATR = Converter.IntValue(_table.Rows[0]["CODATR"].ToString()),
                                ATRPPE = Converter.BoolValue(_table.Rows[0]["ATRPPE"].ToString()),
                                UFEEMI = _table.Rows[0]["UFEEMI"].ToString(),
                                ORGEMI = _table.Rows[0]["ORGEMI"].ToString().ToUpper().Trim(),
                                CODECV = Converter.ByteValue(_table.Rows[0]["CODECV"].ToString()),
                            };
                            this.Found = true;
                        
                    }
                    else
                    {
                    }
                }
            }
            catch (Exception Error)
            {

            }
            return RETURN_VALUE;
        }


        public List<RegisterUsers> GetUsers(int pCODATR,  string pNOMUSU)
        {
            List<RegisterUsers> RETURN_VALUE = new List<RegisterUsers>();
            try
            {
                string cs = _ConnectionString.Value.ConnectionString;
                if (!String.IsNullOrWhiteSpace(cs))
                {
                    Connector _connector = new Connector(cs);
                    _connector.SetCommand("PRCADUSUSELALL", CommandType.StoredProcedure, true);
                    _connector.Comando.Parameters.Add(_connector.CreateParameter("@CODATR", SqlDbType.Int, pCODATR));
                    _connector.Comando.Parameters.Add(_connector.CreateParameter("@NOMUSU", SqlDbType.VarChar, pNOMUSU));
                    DataTable _table = _connector.GetTable();
                    if (_connector.HasRowSet())
                    {
                        for (int i = 0; i < _table.Rows.Count; i++)
                        {
                            RETURN_VALUE.Add(new RegisterUsers
                            {
                                CODUSU = Converter.IntValue(_table.Rows[i]["CODUSU"].ToString()),
                                STAREC = Converter.ByteValue(_table.Rows[i]["STAREC"].ToString()),
                                DATCAD = Converter.DateValue(_table.Rows[i]["DATCAD"].ToString()),
                                DATUPD = Converter.DateValue(_table.Rows[i]["DATUPD"].ToString()),
                                TIPPES = _table.Rows[i]["TIPPES"].ToString(),
                                CODPJU = _table.Rows[i]["CODPJU"].ToString(),
                                NUMIRG = _table.Rows[i]["NUMIRG"].ToString(),
                                CODCMF = _table.Rows[i]["CODCMF"].ToString(),
                                NOMUSU = _table.Rows[i]["NOMUSU"].ToString().ToUpper(),
                                NOMMAE = _table.Rows[i]["NOMMAE"].ToString().ToUpper(),
                                DATNAC = Converter.DateValue(_table.Rows[i]["DATNAC"].ToString()),
                                CODATR = Converter.IntValue(_table.Rows[i]["CODATR"].ToString()),
                                ATRPPE = Converter.BoolValue(_table.Rows[i]["ATRPPE"].ToString()),
                                UFEEMI = _table.Rows[i]["UFEEMI"].ToString(),
                                ORGEMI = _table.Rows[i]["ORGEMI"].ToString().ToUpper(),
                                CODECV = Converter.ByteValue(_table.Rows[i]["CODECV"].ToString()),
                                DSCGEN = _table.Rows[i]["DSCGEN"].ToString(),
                                DSCECV = _table.Rows[i]["DSCECV"].ToString(),
                                DSCUFE = _table.Rows[i]["DSCUFE"].ToString()
                            });
                            this.Found = true;
                        }
                    }

                }
            }
            catch (Exception Error)
            {

            }
            return RETURN_VALUE;
        }


        public List<ItemValue> GetEcvList()
        {
            List<ItemValue> RETURN_VALUE = new List<ItemValue>();
            try
            {
                string cs = _ConnectionString.Value.ConnectionString;
                if (!String.IsNullOrWhiteSpace(cs))
                {
                    Connector _connector = new Connector(cs);
                    _connector.SetCommand("SELECT CODECV, DSCECV FROM TBCADECV ORDER BY CODECV", CommandType.Text);
                    DataTable _table = _connector.GetTable();

                    if (_connector.HasRowSet())
                    {
                        for (int i = 0; i < _table.Rows.Count; i++)
                        {
                            RETURN_VALUE.Add(new ItemValue
                            {
                                Id = Converter.IntValue(_table.Rows[i][0].ToString()),
                                Text = _table.Rows[i][1].ToString()
                            });

                        }

                    }

                }
            }
            catch (Exception Error)
            {

            }
            return RETURN_VALUE;
        }
        public List<TextValue> GetUFEList()
        {
            List<TextValue> RETURN_VALUE = new List<TextValue>();
            try
            {
                string cs = _ConnectionString.Value.ConnectionString;
                if (!String.IsNullOrWhiteSpace(cs))
                {
                    Connector _connector = new Connector(cs);
                    _connector.SetCommand("SELECT CODUFE, DSCUFE FROM TBCADUFE ORDER BY 1", CommandType.Text);
                    DataTable _table = _connector.GetTable();

                    if (_connector.HasRowSet())
                    {
                        for (int i = 0; i < _table.Rows.Count; i++)
                        {
                            RETURN_VALUE.Add(new TextValue
                            {

                                Id = _table.Rows[i][0].ToString(),
                                Text = _table.Rows[i][1].ToString()
                            });

                        }

                    }

                }
            }
            catch (Exception Error)
            {

            }
            return RETURN_VALUE;
        }
        public List<TextValue> GetGENList()
        {
            List<TextValue> RETURN_VALUE = new List<TextValue>();
            try
            {
                string cs = _ConnectionString.Value.ConnectionString;
                if (!String.IsNullOrWhiteSpace(cs))
                {
                    Connector _connector = new Connector(cs);
                    _connector.SetCommand("SELECT CODGEN, DSCGEN FROM TBCADGEN ORDER BY 1", CommandType.Text);
                    DataTable _table = _connector.GetTable();

                    if (_connector.HasRowSet())
                    {
                        for (int i = 0; i < _table.Rows.Count; i++)
                        {
                            RETURN_VALUE.Add(new TextValue
                            {

                                Id = _table.Rows[i][0].ToString(),
                                Text = _table.Rows[i][1].ToString()
                            });

                        }

                    }

                }
            }
            catch (Exception Error)
            {

            }
            return RETURN_VALUE;
        }

    }
}