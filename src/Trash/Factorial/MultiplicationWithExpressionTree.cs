using System.Linq.Expressions;

namespace factorial;

/// <summary>
/// Класс предоставляет метод Multiply, который возвращает произведение двух чисел.
/// (для ознакомления)
/// </summary>
public static class MultiplicationWithExpressionTree
{
    #region Кеширование функций
    private static readonly Lazy<Func<int, int, BigInteger>> MultiplyFuncIntInt = new(() =>
    {
        var paramX = Expression.Parameter(typeof(int), "x");
        var paramY = Expression.Parameter(typeof(int), "y");
        var multiplyExpr = Expression.Lambda<Func<int, int, BigInteger>>(
            Expression.Convert(
                Expression.Call(typeof(BigInteger), "Multiply", null,
                    Expression.Convert(paramX, typeof(BigInteger)),
                    Expression.Convert(paramY, typeof(BigInteger))),
                typeof(BigInteger)
            ),
            paramX, paramY);
        return multiplyExpr.Compile();
    });

    private static readonly Lazy<Func<BigInteger, int, BigInteger>> MultiplyFuncBigIntInt = new(() =>
    {
        var paramX = Expression.Parameter(typeof(BigInteger), "x");
        var paramY = Expression.Parameter(typeof(int), "y");
        var multiplyExpr = Expression.Lambda<Func<BigInteger, int, BigInteger>>(
            Expression.Call(typeof(BigInteger), "Multiply", null, paramX,
                Expression.Convert(paramY, typeof(BigInteger))),
            paramX, paramY
        );
        return multiplyExpr.Compile();
    });

    private static readonly Lazy<Func<BigInteger, BigInteger, BigInteger>> MultiplyFuncBigIntBigInt =
        new(() =>
        {
            var paramX = Expression.Parameter(typeof(BigInteger), "x");
            var paramY = Expression.Parameter(typeof(BigInteger), "y");
            var multiplyExpr = Expression.Lambda<Func<BigInteger, BigInteger, BigInteger>>(
                Expression.Call(typeof(BigInteger), "Multiply", null, paramX, paramY),
                paramX, paramY
            );
            return multiplyExpr.Compile();
        });
    #endregion

    #region Функция Multiply
    public static BigInteger Multiply(int x, int y) => MultiplyFuncIntInt.Value(x, y);
    public static BigInteger Multiply(BigInteger x, int y) => MultiplyFuncBigIntInt.Value(x, y);
    public static BigInteger Multiply(BigInteger x, BigInteger y) => MultiplyFuncBigIntBigInt.Value(x, y);
    #endregion
}