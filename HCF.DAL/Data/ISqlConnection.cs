namespace HCF.DAL
{
    public interface ISqlConnection
    {
        string CommonConnectionString();
        string ConnectionString();
    }
}