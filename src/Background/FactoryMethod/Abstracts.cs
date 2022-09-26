namespace FactoryMethod;

// абстрактный класс строительной компании
abstract class Developer
{
    public string Name { get; set; }

    protected Developer(string n) => Name = n;

    // фабричный метод
    public abstract House Create();
}

abstract class House { }