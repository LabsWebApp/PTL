// ReSharper disable All

//замыканий через лямбда-выражение

// метод или внешняя функция
var outerFunc = () =>
{
    int x = 1; // лексическое окружение - локальная переменная

    // локальная функция. ++x - операции с лексическим окружением
    var innerFunc = () => WriteLine(++x); 
    return innerFunc; // возвращаем локальную функцию
};

var func = outerFunc();   // func = innerFunc, так как outerFunc возвращает innerFunc
// вызываем innerFunc
func();   // 2
func();   // 3
func();   // 4
ReadKey();