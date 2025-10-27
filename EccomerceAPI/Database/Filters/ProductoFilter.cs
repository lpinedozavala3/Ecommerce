using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Database.Filters
{
    public class ProductoFilter
    {
        public string? SearchText { get; set; }
        public Guid? CategoriaId { get; set; }
        public Guid? EmisorId { get; set; }
        public bool? VisibleEnTienda { get; set; } = true;
        public bool? EsNovedad { get; set; }

        private Expression<Func<Producto, bool>> FiltroEmisor()
            => EmisorId.HasValue ? (p => p.EmisorId == EmisorId.Value) : (p => true);

        private Expression<Func<Producto, bool>> FiltroVisible()
            => VisibleEnTienda.HasValue ? (p => p.VisibleEnTienda == VisibleEnTienda.Value) : (p => true);

        private Expression<Func<Producto, bool>> FiltroTexto()
        {
            if (string.IsNullOrWhiteSpace(SearchText)) return p => true;
            var term = SearchText.Trim();
            return p =>
                (p.NombrePublico ?? "").Contains(term) ||
                (p.Nombre ?? "").Contains(term) ||
                (p.DescripcionCorta ?? "").Contains(term);
        }

        private Expression<Func<Producto, bool>> FiltroCategoria()
            => CategoriaId.HasValue ? (p => p.IdCategoria.Any(c => c.IdCategoria == CategoriaId.Value)) : (p => true);

        private Expression<Func<Producto, bool>> FiltroNovedad()
        {
            if (!EsNovedad.HasValue) return p => true;
            return p => p.Novedad == EsNovedad.Value;
        }

        public Expression<Func<Producto, bool>> BuildFilter()
            => FiltroEmisor()
                .And(FiltroVisible())
                .And(FiltroTexto())
                .And(FiltroCategoria())
                .And(FiltroNovedad());
    }
    internal static class ExprExtensions
    {
        public static Expression<Func<T, bool>> And<T>(
            this Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right)
        {
            var param = Expression.Parameter(typeof(T));
            var body = Expression.AndAlso(
                Expression.Invoke(left, param),
                Expression.Invoke(right, param));
            return Expression.Lambda<Func<T, bool>>(body, param);
        }
    }
}
