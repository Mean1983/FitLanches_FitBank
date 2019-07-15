using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardaPio
{
    public  class CommonClass
    {
        public string  prmRunner { get; set; }
        public string prmPath { get; set; } 
        public string prmArquivo { get; set; }
        public int CodigoUsuario { get; set; }
        public string sucoGratis { get; set; }
        public int tempoPreparo { get;  set; }


        public void CalculatempoPreparo(int valor)
        {
            this.tempoPreparo += valor;
        }
    }
}
