using System.Data;
namespace InvestmentResearch.Data;


public interface IDALAccess
{
    DataTable GetDataByQuery(string Isin);

    int SaveDataByQuery(string Sql);
}
