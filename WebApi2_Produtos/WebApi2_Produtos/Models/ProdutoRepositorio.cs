using System;
using System.Collections.Generic;

namespace WebApi2_Produtos.Models
{
    public class ProdutoRepositorio : IProdutoRepositorio
    {
        private List<Produto> produtos = new List<Produto>();
        private int _nextId = 1;

        public ProdutoRepositorio()
        {
            Add(new Produto { Nome = "Guaraná Antartica", Categoria = "Refrigerantes", Preco = 4.59M, Tempo = 3 });
            Add(new Produto { Nome = "Suco de Laranja Prats", Categoria = "Sucos", Preco = 5.75M ,Tempo = 5 });
            Add(new Produto { Nome = "Mostarda Hammer", Categoria = "Condimentos", Preco = 3.90M ,Tempo = 8 });
            Add(new Produto { Nome = "Molho de Tomate Cepera", Categoria = "Condimentos", Preco = 2.99M ,Tempo = 5 });
            Add(new Produto { Nome = "Suco de Uva Aurora", Categoria = "Sucos", Preco = 6.50M ,Tempo = 8 });
            Add(new Produto { Nome = "Pepsi-Cola", Categoria = "Refrigerantes", Preco = 4.25M ,Tempo = 4 });
            Add(new Produto { Nome = "X-Salada", Categoria = "Lanches", Preco = 4.25M, Tempo = 12 });
            Add(new Produto { Nome = "X-Tudo", Categoria = "Lanches", Preco = 4.25M, Tempo = 13 });
            Add(new Produto { Nome = "X-Bacon", Categoria = "Lanches", Preco = 4.25M, Tempo = 15 });
            Add(new Produto { Nome = "X-CalaBresa", Categoria = "Lanches", Preco = 4.25M, Tempo = 16 });
            Add(new Produto { Nome = "Bauru", Categoria = "Lanches", Preco = 4.25M, Tempo = 12 });
            Add(new Produto { Nome = "Hamburger", Categoria = "Lanches", Preco = 4.25M, Tempo = 18 });
            Add(new Produto { Nome = "XBurger", Categoria = "Lanches", Preco = 4.25M, Tempo = 20 });
            Add(new Produto { Nome = "TriploBurger", Categoria = "Lanches", Preco = 4.25M, Tempo = 30 });

        }

        public Produto Add(Produto item)
        {
            if(item == null)
            {
                throw new ArgumentNullException("item");
            }
            item.Id = _nextId++;
            produtos.Add(item);
            return item;
        }

        public Produto Get(int id)
        {
            return produtos.Find(p => p.Id == id);
        }

        public IEnumerable<Produto> GetAll()
        {
            return produtos;
        }

        public void Remove(int id)
        {
            produtos.RemoveAll(p => p.Id == id);
        }

        public bool Update(Produto item)
        {
            if( item == null)
            {
                throw new ArgumentNullException("item");
            }

            int index = produtos.FindIndex(p => p.Id == item.Id);

            if(index == -1)
            {
                return false;
            }
            produtos.RemoveAt(index);
            produtos.Add(item);
            return true;
        }
    }
}