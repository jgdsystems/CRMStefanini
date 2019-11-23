using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSoccer.ViewModel
{
    /// <summary>
    /// ViewModel para controle de lista paginada
    /// Referencia: https://pt.stackoverflow.com/questions/23694/pagina%C3%A7%C3%A3o-mvc-asp-net
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListaPaginada<T>
    {
        public Decimal TotalItens { get; private set; }
        public Decimal ItensPorPagina { get; private set; }
        public Decimal PaginaAtual { get; private set; }
        public bool NewMatch { get; private set; }

        public Decimal TotalPaginas => Math.Ceiling(TotalItens / ItensPorPagina);
        public List<T> Itens { get; private set; }

        public ListaPaginada(List<T> itens, int totalItens, int itensPorPagina, int paginaAtual, bool newMatch)
        {
            this.Itens = itens;
            this.TotalItens = totalItens;
            this.ItensPorPagina = itensPorPagina;
            this.PaginaAtual = paginaAtual;
            this.NewMatch = newMatch;
        }
    }
    
}