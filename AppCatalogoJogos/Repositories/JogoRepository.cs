using AppCatalogoJogos.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppCatalogoJogos.Repositories
{
    public class JogoRepository : IJogoRepository
    {

        private static Dictionary<Guid, Jogo> jogos = new Dictionary<Guid, Jogo>()
        {
            {Guid.Parse("b97c1a63-3992-4608-8156-50a9ade7354a"), new Jogo {Id = Guid.Parse("b97c1a63-3992-4608-8156-50a9ade7354a"), Nome = "Grand Theft Auto III", Produtora = "Rockstar", Preco = 50} },
            {Guid.Parse("c778bc0e-a9f8-41ec-bac5-ecc57145f6b1"), new Jogo {Id = Guid.Parse("c778bc0e-a9f8-41ec-bac5-ecc57145f6b1"), Nome = "Grand Theft Auto IV", Produtora = "Rockstar", Preco = 100} },
            {Guid.Parse("4e2d651d-6ee0-4156-a0ca-38197e76adf3"), new Jogo {Id = Guid.Parse("4e2d651d-6ee0-4156-a0ca-38197e76adf3"), Nome = "Grand Theft Auto V", Produtora = "Rockstar", Preco = 190} }

        };

        public Task<List<Jogo>> Obter(int pagina, int quantidade)
        {
            return Task.FromResult(jogos.Values.Skip((pagina - 1) * quantidade).Take(quantidade).ToList());
        }

        public Task<Jogo> Obter(Guid id)
        {
            if (!jogos.ContainsKey(id))
                return null;
            return Task.FromResult(jogos[id]);
        }

        public Task<List<Jogo>> Obter(string nome, string produtora)
        {
            var retorno = new List<Jogo>();

            foreach (var jogo in jogos.Values)
            {
                if (jogo.Nome.Equals(nome) && jogo.Produtora.Equals(produtora))
                    retorno.Add(jogo);
            }

            return Task.FromResult(jogos.Values.Where(jogo => jogo.Nome.Equals(nome) && jogo.Produtora.Equals(produtora)).ToList());
        }

        public Task<List<Jogo>> ObterSemLambda(string nome, string produtora)
        {
            var retorno = new List<Jogo>();

            foreach (var jogo in jogos.Values)
            {
                if (jogo.Nome.Equals(nome) && jogo.Produtora.Equals(produtora))
                    retorno.Add(jogo);
            }

            return Task.FromResult(retorno);
        }

        public Task Inserir(Jogo jogo)
        {
            jogos.Add(jogo.Id, jogo);
            return Task.CompletedTask;
        }

        public Task Atualizar(Jogo jogo)
        {
            jogos[jogo.Id] = jogo;
            return Task.CompletedTask;
        }
        public Task Remover(Guid id)
        {
            jogos.Remove(id);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
           //Fechar conexão com o banco
        }
    }
}
