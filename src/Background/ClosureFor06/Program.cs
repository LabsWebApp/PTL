// ReSharper disable All

//замыканий через лямбда-выражение

// метод или внешняя функция
var outerFn = () =>
{
    int x = 1; // лексическое окружение - локальная переменная

    // локальная функция. ++x - операции с лексическим окружением
    var innerFn = () => WriteLine(++x); 
    return innerFn; // возвращаем локальную функцию
};

var fn = outerFn();   // fn = innerFn, так как outerFn возвращает innerFn
// вызываем innerFn
fn();   // 2
fn();   // 3
fn();   // 4
ReadKey();