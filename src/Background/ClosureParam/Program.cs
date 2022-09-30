// ReSharper disable All

// Применение параметров
// Кроме внешних переменных к лексическому окружению также относятся
// параметры окружающего метода.

var fn = Multiply(5);

WriteLine(fn(5));   // 25
WriteLine(fn(6));   // 30
WriteLine(fn(7));   // 35

Func<int, int> Multiply(int n)
{
    int Inner(int m) => n * m;
    return Inner;
}

ReadKey();

// всё через Лямбда
var multiply = (int n) => (int m) => n * m;

fn = multiply(5);

WriteLine(fn(5));   // 25
WriteLine(fn(6));   // 30
WriteLine(fn(7));   // 35

ReadKey();