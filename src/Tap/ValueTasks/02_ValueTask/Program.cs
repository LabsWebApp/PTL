static ValueTask<int> Sum(int a, int b) => (a, b) switch
{
    (0, 0) => new ValueTask<int>(0),
    (0, _) => new ValueTask<int>(b),
    (_, 0) => new ValueTask<int>(a),
    _ => new ValueTask<int>(Task.Run(() => a + b))
};

//int res = ;
WriteLine(Sum(5, 3).Result);

ReadKey();