// ReSharper disable All

// Применение параметров
// Кроме внешних переменных к лексическому окружению также относятся
// параметры окружающего метода.

var func = Multiply(5);

WriteLine(func(5));   // 25
WriteLine(func(6));   // 30
WriteLine(func(7));   // 35

Func<int, int> Multiply(int n)
{
    int Inner(int m) => n * m;
    return Inner;
}

ReadKey();

// всё через Лямбда
var multiply = (int n) => (int m) => n * m;

func = multiply(5);

WriteLine(func(5));   // 25
WriteLine(func(6));   // 30
WriteLine(func(7));   // 35

ReadKey();