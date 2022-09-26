namespace AbstractFactory;

public abstract class CarsFactory
{
    public abstract ISedan CreateSedan();
    public abstract ICoupe CreateCoupe();
}

public interface ISedan { }

public interface ICoupe { }
