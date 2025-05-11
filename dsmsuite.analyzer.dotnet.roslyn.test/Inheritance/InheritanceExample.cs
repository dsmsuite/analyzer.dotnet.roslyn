namespace dsmsuite.analyzer.dotnet.roslyn.test.Inheritance
{
    public class BaseClass
    {
        public virtual void BaseMethod() { }
    }

    public class DerivedClass : BaseClass
    {
        public override void BaseMethod() { }
        public void DerivedMethod() { }
    }
}
