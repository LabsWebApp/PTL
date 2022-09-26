namespace AbstractFactory;

public class ToyotaFactory : CarsFactory
{
    public override ISedan CreateSedan() => new ToyotaSedan();

    public override ICoupe CreateCoupe() => new ToyotaCoupe();
}

public class FordFactory : CarsFactory
{
    public override ISedan CreateSedan() => new FordSedan();

    public override ICoupe CreateCoupe() => new FordCoupe();
}

public class ToyotaCoupe : ICoupe
{
    public ToyotaCoupe() => WriteLine("Create ToyotaCoupe");
}

public class ToyotaSedan : ISedan
{
    public ToyotaSedan() => WriteLine("Create ToyotaSedan");
}

public class FordCoupe : ICoupe
{
    public FordCoupe() => WriteLine("Create FordCoupe");
}

public class FordSedan : ISedan
{
    public FordSedan() => WriteLine("Create FordSedan");
}