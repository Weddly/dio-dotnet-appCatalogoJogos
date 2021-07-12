using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppCatalogoJogos.Exceptions
{
    public class JogoJaCadastradoException: Exception
    {
        public JogoJaCadastradoException()
            : base("Esse jogo já está cadastrado") { }
    }
}
