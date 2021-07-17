using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppSample1.Models
{
    public class RegisterUsers
    {

        /// <summary>
        /// Código do Usuário
        /// </summary>
         
        [Key]
        [Required]
        [Display(Name ="Código")]
        public int CODUSU { get; set; }

        /// <summary>
        /// Código do Status de Registro
        /// </summary>
        [Display(Name = "Status do Registro")]
        public byte STAREC { get; set; } = 1;


        /// <summary>
        /// Data de Inclusão ou cadastramento
        /// </summary>
        [Display(Name = "Data de Cadastro")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime DATCAD { get; set; } = DateTime.Now;

        /// <summary>
        /// Data da Ultima Atualização
        /// </summary>
        [Display(Name = "Última Atualização")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime DATUPD { get; set; } = DateTime.Now;

        /// <summary>
        /// Gênero
        /// </summary>
        /// <remarks>
        /// <para>M - Masculino</para>
        /// <para>F - Feminino</para>
        /// <para>I - Indeterminado</para>
        /// </remarks>
        [Display(Name = "Gênero")]
        public string TIPPES { get; set; } = "F";

        /// <summary>
        /// Personalidade Juridica
        /// </summary>
        /// <remarks>
        /// <para>F - Pessoa Física,</para>
        /// <para>J - Pessoa Jurídica,</para>
        /// <para>I - Indeterminado</para>
        /// </remarks>
        [Display(Name = "Personalidade Juridica")]
        public string CODPJU { get; set; } = "I";

        /// <summary>
        /// Número do RG para pessoa física/Inscrição Estadual para pessoa jurídica 
        /// </summary>
        [Display(Name = "RG")]
        public string NUMIRG { get; set; } 

        /// <summary>
        /// CPF/CNPJ
        /// </summary>
        [Display(Name = "CMF")]
        public string CODCMF { get; set; }


        /// <summary>
        /// Nome do usuário
        /// </summary>
        [Required(ErrorMessage = "O nome é obrigatorio", AllowEmptyStrings = false)]
        [Display(Name = "Nome")]
        public string NOMUSU { get; set; }

        /// <summary>
        /// Nome da mãe
        /// </summary>
        [Required(ErrorMessage = "O nome da mãe é obrigatorio", AllowEmptyStrings = false)]
        [Display(Name = "Nome da Mãe")]
        public string NOMMAE { get; set; }

        /// <summary>
        /// Data de Nascimento
        /// </summary>
        /// <remarks>
        /// <para>A data de nascimento pode ser nula na entrada</para>
        /// </remarks>
        [Display(Name = "Nascimento")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DATNAC { get; set; }


        /// <summary>
        /// Codigo do Atributo do Cadastro
        /// </summary>
        [Display(Name = "Tipo de Cadastro")]
        public int CODATR { get; set; } = 1;


        /// <summary>
        /// Pessoa Politacamente Exposta
        /// </summary>
        /// <remarks>
        /// <para>pessoa politicamente exposta é uma pessoa que trabalha ou trabalhou nos últimos cinco anos, no Brasil ou no exterior, em cargos, empregos ou funções públicas. Pessoas que tenham familiares, cônjuges, representantes, parentes, relacionados direta ou indiretamente, com uma pessoa politicamente exposta também são assim considerados.</para>
        /// </remarks>
        [Display(Name = "Pessoa Politicamente Exposta")]
        public bool ATRPPE { get; set; } 


        /// <summary>
        /// Unidade da Federeção do Emissor
        /// </summary>
        [Display(Name = "UF Emissor")]         
        public string UFEEMI { get; set; }

        /// <summary>
        /// Orgão emissor
        /// </summary>
        [Display(Name = "Orgão Emissor")]
        public string ORGEMI { get; set; }

        /// <summary>
        /// Código do Estado Civil
        /// </summary>
        [Display(Name = "Estado Civil")]
        public byte CODECV { get; set; } = 0;

        /// <summary>
        /// Código do Estado Civil
        /// </summary>
        [Display(Name = "Estado Civil")]
        public string DSCECV { get; set; } = "";

        /// <summary>
        /// Região
        /// </summary>
        [Display(Name = "Emissor")]
        public string DSCUFE { get; set; } = "";

        /// <summary>
        /// Gênero
        /// </summary>
        [Display(Name = "Gênero")]
        public string DSCGEN { get; set; } = "";

    }
}
