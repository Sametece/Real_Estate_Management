using System;
using System.Linq.Expressions;
using LinqKit;

namespace RealEstate.Business.Extensions;

public static class PredicateExtensions
{
    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
    {
        return first.Expand().And(second.Expand());
    }

    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
    {
        return first.Expand().Or(second.Expand());
    }
}