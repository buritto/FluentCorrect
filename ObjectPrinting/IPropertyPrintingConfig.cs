namespace ObjectPrinting
{
    public interface IPropertyPrintingConfig<T, TOwner>
    {
        PrintingConfig<TOwner> Config { get; }
    }
}
