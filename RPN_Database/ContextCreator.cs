using System.Data.Entity;

namespace RPN_Database
{
    public delegate T ContextCreator<T>() where T : DbContext;
}
