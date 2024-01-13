namespace LIBRARY;

public class Store
{
    private readonly int _storeId;
    private readonly string _storeName;
    private readonly string _location;
    private readonly string[] _employees;
    private readonly string[] _products;

    public int StoreId
    {
        get { return _storeId;  }
    }

    public string StoreName
    {
        get { return _storeName; }
    }

    public string Location
    {
        get { return _location; }
    }

    public string[] Employees
    {
        get { return _employees; }
    }
    
    public string[] Products
    {
        get { return _products; }
    }

    public Store(int storeId, string storeName, string location, string[] employees, string[] products)
    {
        if (storeName == null || location == null || employees == null || products == null)
        {
            throw new ArgumentNullException();
        }

        _storeId = storeId;
        _storeName = storeName;
        _location = location;
        _employees = employees;
        _products = products;
    }
}