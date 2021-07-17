using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppSample1.Models
{
    public interface IRegisterUserDAL
    {
        int Insert(RegisterUsers model);
        int Update(RegisterUsers model);
        RegisterUsers Select(int pCODUSU);
        bool Found { get; set; }
        List<RegisterUsers> GetUsers(int pCODATR, string pNOMUSU);
        List<ItemValue> GetEcvList();
        List<TextValue> GetGENList();
        List<TextValue> GetUFEList();
    }
}
