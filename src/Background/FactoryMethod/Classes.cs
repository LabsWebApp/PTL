namespace FactoryMethod;

// строит кирпичные дома
class BrickDeveloper : Developer
{
    public BrickDeveloper(string n) : base(n) { }

    public override House Create() => new BrickHouse();
}

// строит деревянные дома
class WoodDeveloper : Developer
{
    public WoodDeveloper(string n) : base(n) { }

    public override House Create() => new WoodHouse();
}

class BrickHouse : House
{
    public BrickHouse() => WriteLine("Кирпичный дом построен");
}

class WoodHouse : House
{
    public WoodHouse() => WriteLine("Деревянный дом построен");
}